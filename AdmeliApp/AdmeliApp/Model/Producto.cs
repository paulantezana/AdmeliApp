using AdmeliApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.Model
{
    public class Producto : BaseModel
    {
        //Compra,Venta,Stock cambiado el tipo de string a Decimal
        public int idProducto { get; set; }   // porque haci  manda desde el servicio modificado 
        public string idPresentacion { get; set; }
        public bool cantidadFraccion { get; set; }
        public string codigoBarras { get; set; }
        public string codigoProducto { get; set; }
        public string controlSinStock { get; set; }
        public string descripcionCorta { get; set; }
        public string descripcionLarga { get; set; }
        public bool enCategoriaEstrella { get; set; }
        public bool enPortada { get; set; }
        public bool enUso { get; set; }
        public bool estado { get; set; }
        public int idMarca { get; set; }
        public int idUnidadMedida { get; set; }
        public string keywords { get; set; }
        public string limiteMaximo { get; set; }
        public string limiteMinimo { get; set; }
        public bool mostrarPrecioWeb { get; set; }
        public bool mostrarVideo { get; set; }
        public bool mostrarWeb { get; set; }
        public string nombreMarca { get; set; }
        public string nombreProducto { get; set; }
        public string nombreUnidad { get; set; }
        public Decimal precioCompra { get; set; }
        public string urlVideo { get; set; }
        public bool ventaVarianteSinStock { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        private string estadoString;

        public string EstadoString
        {
            get
            {
                if (estado == true) { return "Activo"; }
                else { return "Anulado"; }
            }
            set
            {
                estadoString = value;
            }
        }
        public Decimal precioVenta { get; set; }
        public Decimal stock { get; set; }
        public string stockFinanciero { get; set; }
        public int idPresentacionAfectada { get; set; }
        public int idAlmacen { get; set; }
        public string nombreAlmacen { get; set; }

        private int idPresentacionint;
        public int IdPresentacionint
        {
            get
            {
                if (idPresentacion == null)
                {
                    return 0;
                }
                else
                {
                    return Int32.Parse(idPresentacion);
                }
            }
            set
            {
                idPresentacion = value.ToString();
            }
        }
    }
}
