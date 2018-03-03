using AdmeliApp.Helpers;
using AdmeliApp.Pages.ProductoPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.Pages.Navigation
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuGrouping> MenuTiendaItems { get; set; }
        private DataService dataService = new DataService();

        private string _UserName;
        public string UserName
        {
            get { return this._UserName; }
            set { SetValue(ref this._UserName, value); }
        }

        private string _UserDocument;
        public string UserDocument
        {
            get { return this._UserDocument; }
            set { SetValue(ref this._UserDocument, value); }
        }

        private ICommand _LogoutCommand;
        public ICommand LogoutCommand =>
            _LogoutCommand ?? (_LogoutCommand = new Command(() => ExecuteLogoutAsync()));

        public MenuViewModel()
        {
            this.UserName = App.personal.usuario;
            this.UserDocument = App.personal.numeroDocumento;

            MenuTiendaItems = new ObservableCollection<MenuGrouping>()
            {
                new MenuGrouping("Ventas")
                {
                    new MenuTienda() { Id = 100, Icon = "guia_icon.png", Title = "Contizaciones de cliente" },
                    new MenuTienda() { Id = 101, Icon = "venta_icon.png", Title = "Ventas" },
                    new MenuTienda() { Id = 102, Icon = "cliente_icon.png", Title = "Clientes" },
                    new MenuTienda() { Id = 103, Icon = "oferta_icon.png", Title = "Descuento y oferta" }
                },

                new MenuGrouping("Compras")
                {
                    new MenuTienda() { Id = 201, Icon = "ordenCompra_icon.png", Title = "Órden compra proveedor" },
                    new MenuTienda() { Id = 202, Icon = "compra_icon.png", Title = "Compras" },
                    new MenuTienda() { Id = 203, Icon = "proveedor_icon.png", Title = "Proveedores" }
                },

                new MenuGrouping("Productos")
                {
                    new MenuTienda() { Id = 301, Icon = "producto_icon.png", Title = "Listado de productos", TargetType = typeof(ProductosPage) },
                    new MenuTienda() { Id = 302, Icon = "marca_icon.png", Title = "Marcas", TargetType = typeof(MarcaPage) },
                    new MenuTienda() { Id = 303, Icon = "unidadMedida_icon.png", Title = "Unidades de medida", TargetType = typeof(UnidadMedidaPage) },
                    new MenuTienda() { Id = 304, Icon = "categoria_icon.png", Title = "Categorias", TargetType = typeof(CategoriaPage) }
                },

                new MenuGrouping("Almacen")
                {
                    new MenuTienda() { Id = 401, Icon = "view_icon.png", Title = "Nota de entrada" },
                    new MenuTienda() { Id = 402, Icon = "notaSalida_icon.png", Title = "Nota de salida" },
                    new MenuTienda() { Id = 403, Icon = "gruiaRemision_icon.png", Title = "Guía de remisión" }
                },

                new MenuGrouping("Caja")
                {
                    new MenuTienda() { Id = 501, Icon = "egreso_icon.png", Title = "Egresos" },
                    new MenuTienda() { Id = 502, Icon = "ingreso_icon.png", Title = "Ingresos" },
                    new MenuTienda() { Id = 503, Icon = "cobrar_icon.png", Title = "Cuentas por cobrar" },
                    new MenuTienda() { Id = 504, Icon = "pagar_icon.png", Title = "Cuentas por pagar" },
                    new MenuTienda() { Id = 505, Icon = "iniciarCaja_icon.png", Title = "Iniciar caja" },
                    new MenuTienda() { Id = 506, Icon = "cierreCaja_icon.png", Title = "Cierre de caja" }
                },

                new MenuGrouping("Herramientas")
                {
                    new MenuTienda() { Id = 601, Icon = "stock_icon.png", Title = "Stock precio" },
                    new MenuTienda() { Id = 602, Icon = "categoria_icon.png", Title = "Asignar categorías" },
                    new MenuTienda() { Id = 603, Icon = "impuesto_icon.png", Title = "Asignar impuestos" },
                    new MenuTienda() { Id = 604, Icon = "codigoBarra_icon.png", Title = "Generar código de barras" }
                },

                new MenuGrouping("Reportes")
                {
                    new MenuTienda() { Id = 1, Icon = "reporteProducto_icon.png", Title = "Existencia producto" },
                    new MenuTienda() { Id = 1, Icon = "reporteIngreso_icon.png", Title = "Ingresos ventas" },
                    new MenuTienda() { Id = 1, Icon = "reporteImpuesto_icon.png", Title = "Impuestos" }
                },

                new MenuGrouping("Configuración")
                {
                    new MenuTienda() { Id = 1, Icon = "empresa_icon.png", Title = "Datos Empresa" },
                    new MenuTienda() { Id = 1, Icon = "sucursal_icon.png", Title = "Sucursales" },
                    new MenuTienda() { Id = 1, Icon = "puntoVenta_icon.png", Title = "Punto de venta" },
                    new MenuTienda() { Id = 1, Icon = "almacen_icon.png", Title = "Almacenes" },
                    new MenuTienda() { Id = 1, Icon = "docIdentificacion_icon.png", Title = "Documentos de identificación" },
                    new MenuTienda() { Id = 1, Icon = "personal_icon.png", Title = "Personal" },
                    new MenuTienda() { Id = 1, Icon = "documento_icon.png", Title = "Listado documentos" },
                    new MenuTienda() { Id = 1, Icon = "correlativo_icon.png", Title = "Asignar correlativos" },
                    new MenuTienda() { Id = 1, Icon = "design_icon.png", Title = "Diseño personalización" },
                    new MenuTienda() { Id = 1, Icon = "moneda_icon.png", Title = "Monedas" },
                    new MenuTienda() { Id = 1, Icon = "cambioMoneda_icon.png", Title = "Tipo cambio" },
                    new MenuTienda() { Id = 1, Icon = "denominacion_icon.png", Title = "Denominaciones" },
                    new MenuTienda() { Id = 1, Icon = "impuesto_icon.png", Title = "Impuestos" },
                    new MenuTienda() { Id = 1, Icon = "grupoCliente_icon.png", Title = "Grupos de cliente" },
                    new MenuTienda() { Id = 1, Icon = "caja_icon.png", Title = "Cajas inicializadas" },
                    new MenuTienda() { Id = 1, Icon = "docImpuesto_icon.png", Title = "Impuestos documento" },
                    new MenuTienda() { Id = 1, Icon = "codigoBarra_icon.png", Title = "Diseño de código de barras" },
                }
            };
        }

        private void ExecuteLogoutAsync()
        {
            dataService.DeletePersonal(App.personal);
            App.Current.MainPage = new AdmeliApp.Pages.Root.LoginPage();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
        }
        #endregion
    }
}
