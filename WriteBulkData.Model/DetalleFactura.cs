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


namespace WriteBulkData.Model
{
    using WriteBulkData.Model.Mapping;

    // DetalleFactura
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.37.1.0")]
    public partial class DetalleFactura
    {
        public int NumeroFactura { get; set; } // NumeroFactura (Primary key)
        public string Serie { get; set; } // Serie (Primary key) (length: 1)
        public int CodigoProducto { get; set; } // CodigoProducto (Primary key)
        public int Cantidad { get; set; } // Cantidad

        // Foreign keys

        /// <summary>
        /// Parent EncabezadoFactura pointed by [DetalleFactura].([NumeroFactura], [Serie]) (FK_DetalleFactura_EncabezadoFactura)
        /// </summary>
        public virtual EncabezadoFactura EncabezadoFactura { get; set; } // FK_DetalleFactura_EncabezadoFactura

        /// <summary>
        /// Parent Producto pointed by [DetalleFactura].([CodigoProducto]) (FK_DetalleFactura_Producto)
        /// </summary>
        public virtual Producto Producto { get; set; } // FK_DetalleFactura_Producto
    }

}
// </auto-generated>