using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WriteBulkData.Model;
using Bogus;
using FizzWare.NBuilder;
using Newtonsoft.Json;

namespace WriteBulkData.Console
{
    class Program
    {
        static readonly PerformanceDbContext context = new PerformanceDbContext();
        static bool CleanDb = true;
        static bool UseBatch = true;

        static int Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 4)
            {
                System.Console.WriteLine("Invalid number of parameters.");
                PrintUsage();
                return 1;
            }

            if (args.Any(a => a.Equals("--CleanDb")))
            {
                var argInfo = args.Single(a => a.StartsWith("--CleanDb")).Split(':')[1];
                if (bool.TryParse(argInfo, out var cleanDb))
                {
                    CleanDb = cleanDb;
                }
            }

            if (args.Any(a => a.Equals("--UseBatch")))
            {
                var argInfo = args.Single(a => a.StartsWith("--UseBatch")).Split(':')[1];
                if (bool.TryParse(argInfo, out var useBatch))
                {
                    UseBatch = useBatch;
                }
            }

            if (args.Any(p => p.StartsWith("--Products")))
            {
                var argInfo = args.Single(a => a.StartsWith("--Products")).Split(':')[1];
                if (int.TryParse(argInfo, out var numberOfProductsToGenerate))
                {
                    RunGenerateProducts(numberOfProductsToGenerate, CleanDb, UseBatch);
                }
            }

            if (args.Any(p => p.StartsWith("--Invoices")))
            {
                var argInfo = args.Single(a => a.StartsWith("--Invoices")).Split(':')[1];
                if (int.TryParse(argInfo, out var numberOfInvoicesToGenerate))
                {
                    RunGenerateInvoices(numberOfInvoicesToGenerate, CleanDb, UseBatch);
                }
            }

            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
            return 0;
        }

        static void PrintUsage()
        {
            System.Console.WriteLine("Usage: WriteBulkData [--Products:<quantity of products to generate>] [--Invoices:<quantity of invoices to generate>] [--CleanDb:<true / false>] [--UseBatch:<true / false>]");
        }


        static void RunGenerateProducts(int numberOfProductsToGenerate, bool dropData = false, bool batchInsert = true)
        {
            var mode = (batchInsert) ? "in batch mode" : "in one by one mode";
            System.Console.WriteLine($"TEST: {numberOfProductsToGenerate} products {mode}.");
            var watch = Stopwatch.StartNew();
            var completeOperation = GenerateProducts(numberOfProductsToGenerate, dropData, batchInsert);
            watch.Stop();
            System.Console.WriteLine($"Total time of the call {watch.ElapsedMilliseconds} ms.");
            System.Console.WriteLine($"Total time of the batch {completeOperation.totalElapsed} ms.");
            System.Console.WriteLine($"Total time per transaction {completeOperation.coreEllapsed} ms.");
        }

        static (double coreEllapsed, long totalElapsed) GenerateProducts(int numberOfProductsToGenerate, bool dropData = false, bool batchInsert = true)
        {
            Stopwatch watch;
            var faker = new Faker("es");
            if (dropData)
            {
                context.EncabezadoFacturas.Where(f => 1 == 1).DeleteFromQuery();
                context.Productos.Where(x => 1 == 1).DeleteFromQuery();
            }

            var range = new Queue<int>(Enumerable.Range(1, numberOfProductsToGenerate));
            faker.IndexFaker = 1;
            var fakeProducts = Builder<Producto>.CreateListOfSize(numberOfProductsToGenerate)
                .All()
                    .With(p => p.CodigoProducto = faker.IndexFaker++)
                    .With(p => p.Descripcion = faker.Commerce.ProductName())
                    .With(p => p.PrecioVenta = decimal.Parse(faker.Commerce.Price()))
                    .With(p => p.Existencia = int.MaxValue)
                .Build();


            if (batchInsert)
            {
                watch = Stopwatch.StartNew();
                context.BulkInsert<Producto>(fakeProducts);
                context.SaveChanges();
                watch.Stop();

                return ((double)watch.ElapsedMilliseconds / (double)numberOfProductsToGenerate, watch.ElapsedMilliseconds);
            }

            var times = new List<long>(numberOfProductsToGenerate);
            fakeProducts.ToList().ForEach(p =>
            {
                watch = Stopwatch.StartNew();
                context.Productos.Add(p);
                context.SaveChanges();
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
            });

            return (times.Average(), times.Sum());
        }

        static void RunGenerateInvoices(int numberOfInvoicesToGenerate, bool dropData = false, bool batchInsert = true)
        {
            var mode = (batchInsert) ? "in batch mode" : "in one by one mode";
            System.Console.WriteLine($"TEST: {numberOfInvoicesToGenerate} invoices {mode}.");
            var watch = Stopwatch.StartNew();
            var completeOperation = GenerateInvoices(numberOfInvoicesToGenerate, dropData, batchInsert);
            watch.Stop();
            System.Console.WriteLine($"Total time of the call {watch.ElapsedMilliseconds} ms.");
            System.Console.WriteLine($"Total time of the batch {completeOperation.totalElapsed} ms.");
            System.Console.WriteLine($"Total time per transaction {completeOperation.coreEllapsed} ms.");
        }

        static (double coreEllapsed, long totalElapsed) GenerateInvoices(int numberOfInvoicesToGenerate, bool dropData = false, bool batchInsert = true)
        {
            Stopwatch watch;
            var faker = new Faker();
            if (dropData)
            {
                context.EncabezadoFacturas.Where(x => 1 == 1).DeleteFromQuery();
                context.Productos.Where(x => 1 == 1).DeleteFromQuery();
            }

            if (!context.Productos.Any())
            {
                System.Console.WriteLine($"There are not products to run the test. Generating sample products...");
                var (coreEllapsed, totalElapsed) = GenerateProducts(numberOfInvoicesToGenerate * 100, true, true);
                System.Console.WriteLine($"{numberOfInvoicesToGenerate * 100} sampling products has been generated. Generation time {totalElapsed} ms.");
            }


            var series = new string[] { "A", "B", "C", "D" };
            faker.IndexFaker = 1;

            var rnd = new Random();
            var randomCodes = context.Productos.Select(p => p.CodigoProducto).ToArray().OrderBy(x => rnd.Next()).ToArray();
            var codigos = new Queue<int>(randomCodes);
            var productosLookup = context.Productos.ToDictionary(p => p.CodigoProducto);

            var fakeDetails = new Faker<DetalleFactura>()
                                    .RuleFor(d => d.CodigoProducto, f => codigos.Dequeue())
                                    .RuleFor(d => d.Cantidad, f => f.Random.Int(1, 1000));

            var fakeInvoices = Builder<EncabezadoFactura>.CreateListOfSize(numberOfInvoicesToGenerate)
                .All()
                    .With(f => f.NumeroFactura = faker.IndexFaker++)
                    .With(f => f.Serie = faker.PickRandom(series))
                    .With(f => f.Fecha = faker.Date.Between(new DateTime(2018, 1, 1), DateTime.Today))
                    .With(f => {
                        var fakes = fakeDetails.Generate(faker.Random.Int(1, 25)).ToList();
                        fakes.ForEach(d =>
                        {
                            d.NumeroFactura = f.NumeroFactura;
                            d.Serie = f.Serie;
                            f.DetalleFacturas.Add(d);
                        });
                        return f.DetalleFacturas;
                    })
                    .With(f => f.Monto = f.DetalleFacturas.Select(d => d.Cantidad * productosLookup[d.CodigoProducto].PrecioVenta).Sum())
                .Build();


            if (batchInsert)
            {
                watch = Stopwatch.StartNew();
                context.BulkInsert<EncabezadoFactura>(fakeInvoices);
                context.SaveChanges();
                watch.Stop();

                return ((double)watch.ElapsedMilliseconds / (double)numberOfInvoicesToGenerate, watch.ElapsedMilliseconds);
            }

            var test = JsonConvert.SerializeObject(fakeInvoices.First(), Formatting.Indented);


            var times = new List<long>(numberOfInvoicesToGenerate);
            fakeInvoices.ToList().ForEach(i =>
            {
                watch = Stopwatch.StartNew();
                context.EncabezadoFacturas.Add(i);
                context.SaveChanges();
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
            });

            return (times.Average(), times.Sum());
        }


    }
}
