// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning


namespace WriteBulkData.Model.Mapping
{
    using WriteBulkData.Model;

    // DetalleFactura
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.1.0")]
    public partial class DetalleFacturaMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DetalleFactura>
    {
        public DetalleFacturaMap()
            : this("dbo")
        {
        }

        public DetalleFacturaMap(string schema)
        {
            ToTable("DetalleFactura", schema);
            HasKey(x => new { x.NumeroFactura, x.Serie, x.CodigoProducto });

            Property(x => x.NumeroFactura).HasColumnName(@"NumeroFactura").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Serie).HasColumnName(@"Serie").HasColumnType("char").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(1).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.CodigoProducto).HasColumnName(@"CodigoProducto").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Cantidad).HasColumnName(@"Cantidad").HasColumnType("int").IsRequired();

            // Foreign keys
            HasRequired(a => a.EncabezadoFactura).WithMany(b => b.DetalleFacturas).HasForeignKey(c => new { c.NumeroFactura, c.Serie }).WillCascadeOnDelete(false); // FK_DetalleFactura_EncabezadoFactura
            HasRequired(a => a.Producto).WithMany(b => b.DetalleFacturas).HasForeignKey(c => c.CodigoProducto).WillCascadeOnDelete(false); // FK_DetalleFactura_Producto
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
