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

    // EncabezadoFactura
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.1.0")]
    public partial class EncabezadoFacturaMap : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<EncabezadoFactura>
    {
        public EncabezadoFacturaMap()
            : this("dbo")
        {
        }

        public EncabezadoFacturaMap(string schema)
        {
            ToTable("EncabezadoFactura", schema);
            HasKey(x => new { x.NumeroFactura, x.Serie });

            Property(x => x.NumeroFactura).HasColumnName(@"NumeroFactura").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Serie).HasColumnName(@"Serie").HasColumnType("char").IsRequired().IsFixedLength().IsUnicode(false).HasMaxLength(1).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Monto).HasColumnName(@"Monto").HasColumnType("decimal").IsRequired().HasPrecision(18,0);
            Property(x => x.Fecha).HasColumnName(@"Fecha").HasColumnType("date").IsRequired();
            Property(x => x.Nit).HasColumnName(@"Nit").HasColumnType("varchar").IsRequired().IsUnicode(false).HasMaxLength(50);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
