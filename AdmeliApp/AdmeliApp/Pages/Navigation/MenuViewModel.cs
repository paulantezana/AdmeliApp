using AdmeliApp.Helpers;
using AdmeliApp.Pages.AlmacenPages;
using AdmeliApp.Pages.CajaPages;
using AdmeliApp.Pages.CompraPages;
using AdmeliApp.Pages.ConfiguracionPages;
using AdmeliApp.Pages.ProductoPages;
using AdmeliApp.Pages.ReportePages;
using AdmeliApp.Pages.Root;
using AdmeliApp.Pages.VentaPages;
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

        private ICommand _HomePageCommand;
        public ICommand HomePageCommand =>
            _HomePageCommand ?? (_HomePageCommand = new Command(() => ExecuteHomePage()));

        private ICommand _LogoutCommand;
        public ICommand LogoutCommand =>
            _LogoutCommand ?? (_LogoutCommand = new Command(() => ExecuteLogoutAsync()));

        public MenuViewModel()
        {
            this.UserName = App.personal.usuario;
            this.UserDocument = App.personal.numeroDocumento;

            MenuTiendaItems = new ObservableCollection<MenuGrouping>();

            MenuGrouping menuVentas = new MenuGrouping("Ventas")
            {
                new MenuTienda() { Id = 100, Icon = "guia_icon.png", Title = "Contizaciones de cliente", TargetType = typeof(VentaPages.CotizacionPage)  },
                new MenuTienda() { Id = 101, Icon = "venta_icon.png", Title = "Ventas", TargetType = typeof(VentaPages.VentaPage)  },
                new MenuTienda() { Id = 102, Icon = "cliente_icon.png", Title = "Clientes", TargetType = typeof(VentaPages.ClientePage)  },
                new MenuTienda() { Id = 103, Icon = "oferta_icon.png", Title = "Descuento y oferta", TargetType = typeof(VentaPages.DescuentoPage)  }
            };

            MenuGrouping menuCompras = new MenuGrouping("Compras")
            {
                new MenuTienda() { Id = 201, Icon = "ordenCompra_icon.png", Title = "Órden compra proveedor", TargetType = typeof(CompraPages.OrdenCompraPage)  },
                new MenuTienda() { Id = 202, Icon = "compra_icon.png", Title = "Compras", TargetType = typeof(CompraPages.CompraPage)  },
                new MenuTienda() { Id = 203, Icon = "proveedor_icon.png", Title = "Proveedores", TargetType = typeof(CompraPages.ProveedorPage)  }
            };

            MenuGrouping menuProductos = new MenuGrouping("Productos")
            {
                new MenuTienda() { Id = 301, Icon = "producto_icon.png", Title = "Listado de productos", TargetType = typeof(ProductoPages.ProductosPage) },
                new MenuTienda() { Id = 302, Icon = "marca_icon.png", Title = "Marcas", TargetType = typeof(ProductoPages.MarcaPage) },
                new MenuTienda() { Id = 303, Icon = "unidadMedida_icon.png", Title = "Unidades de medida", TargetType = typeof(ProductoPages.UnidadMedidaPage) },
                new MenuTienda() { Id = 304, Icon = "categoria_icon.png", Title = "Categorias", TargetType = typeof(ProductoPages.CategoriaPage) }
            };

            MenuGrouping menuAlmacen = new MenuGrouping("Almacen")
            {
                new MenuTienda() { Id = 401, Icon = "view_icon.png", Title = "Nota de entrada", TargetType = typeof(AlmacenPages.NotaEntradaPage) },
                new MenuTienda() { Id = 402, Icon = "notaSalida_icon.png", Title = "Nota de salida", TargetType = typeof(AlmacenPages.NotaSalidaPage)  },
                new MenuTienda() { Id = 403, Icon = "gruiaRemision_icon.png", Title = "Guía de remisión", TargetType = typeof(AlmacenPages.GuiaRemisionPage)  }
            };

            MenuGrouping menuCaja = new MenuGrouping("Caja")
            {
                new MenuTienda() { Id = 501, Icon = "egreso_icon.png", Title = "Egresos",  TargetType = typeof(CajaPages.EgresoPage)},
                new MenuTienda() { Id = 502, Icon = "ingreso_icon.png", Title = "Ingresos",  TargetType = typeof(CajaPages.IngresoPage) },
                //new MenuTienda() { Id = 503, Icon = "cobrar_icon.png", Title = "Cuentas por cobrar"},
                //new MenuTienda() { Id = 504, Icon = "pagar_icon.png", Title = "Cuentas por pagar"},
                //new MenuTienda() { Id = 505, Icon = "iniciarCaja_icon.png", Title = "Iniciar caja"},
                //new MenuTienda() { Id = 506, Icon = "cierreCaja_icon.png", Title = "Cierre de caja"}
            };

            MenuGrouping menuReportes = new MenuGrouping("Reportes")
            {
                new MenuTienda() { Id = 1, Icon = "reporteProducto_icon.png", Title = "Existencia producto",  TargetType = typeof(ReportePages.ExistenciaProductoPage) },
                new MenuTienda() { Id = 1, Icon = "reporteIngreso_icon.png", Title = "Ingresos ventas",  TargetType = typeof(ReportePages.IngresoPage) },
                new MenuTienda() { Id = 1, Icon = "reporteImpuesto_icon.png", Title = "Impuestos",  TargetType = typeof(ReportePages.ImpuestoPage) }
            };

            MenuGrouping menuAdmin = new MenuGrouping("Configuración")
            {
                new MenuTienda() { Id = 1, Icon = "empresa_icon.png", Title = "Datos Empresa",  TargetType = typeof(ConfiguracionPages.DatosEmpresaPage)  },
                new MenuTienda() { Id = 1, Icon = "sucursal_icon.png", Title = "Sucursales",  TargetType = typeof(ConfiguracionPages.SucursalPage)  },
                new MenuTienda() { Id = 1, Icon = "puntoVenta_icon.png", Title = "Punto de venta",  TargetType = typeof(ConfiguracionPages.PuntoVentaPage)  },
                new MenuTienda() { Id = 1, Icon = "almacen_icon.png", Title = "Almacenes",  TargetType = typeof(ConfiguracionPages.AlmacenPage)  },
                new MenuTienda() { Id = 1, Icon = "docIdentificacion_icon.png", Title = "Documentos de identificación",  TargetType = typeof(ConfiguracionPages.DenominacionPage)  },
                new MenuTienda() { Id = 1, Icon = "personal_icon.png", Title = "Personal",  TargetType = typeof(ConfiguracionPages.PersonalPage)  },
                new MenuTienda() { Id = 1, Icon = "documento_icon.png", Title = "Listado documentos",  TargetType = typeof(ConfiguracionPages.ListarDocumentosPage)  },
                new MenuTienda() { Id = 1, Icon = "correlativo_icon.png", Title = "Asignar correlativos",  TargetType = typeof(ConfiguracionPages.AsignarCorrelativoPage)  },
                //new MenuTienda() { Id = 1, Icon = "design_icon.png", Title = "Diseño personalización"},
                new MenuTienda() { Id = 1, Icon = "moneda_icon.png", Title = "Monedas",  TargetType = typeof(ConfiguracionPages.MonedaPage)  },
                new MenuTienda() { Id = 1, Icon = "cambioMoneda_icon.png", Title = "Tipo cambio",  TargetType = typeof(ConfiguracionPages.TipoCambioPage)  },
                new MenuTienda() { Id = 1, Icon = "denominacion_icon.png", Title = "Denominaciones",  TargetType = typeof(ConfiguracionPages.DenominacionPage)  },
                new MenuTienda() { Id = 1, Icon = "impuesto_icon.png", Title = "Impuestos",  TargetType = typeof(ConfiguracionPages.ImpuestoPage)  },
                new MenuTienda() { Id = 1, Icon = "grupoCliente_icon.png", Title = "Grupos de cliente",  TargetType = typeof(ConfiguracionPages.GrupoClientePage)  },
                new MenuTienda() { Id = 1, Icon = "caja_icon.png", Title = "Cajas inicializadas",  TargetType = typeof(ConfiguracionPages.CajaInicializadaPage)  },
                //new MenuTienda() { Id = 1, Icon = "docImpuesto_icon.png", Title = "Impuestos documento"},
                //new MenuTienda() { Id = 1, Icon = "codigoBarra_icon.png", Title = "Diseño de código de barras"},
            };

            //// Agregando los menus a la lista del menu tienda segun los permisos de login ususario
            MenuTiendaItems.Add(menuVentas);
            MenuTiendaItems.Add(menuCompras);
            MenuTiendaItems.Add(menuProductos);
            MenuTiendaItems.Add(menuAlmacen);
            MenuTiendaItems.Add(menuCaja);
            MenuTiendaItems.Add(menuReportes);
            if (App.asignacionPersonal.idAsignarPuntoAdministracion > 0)
            {
                MenuTiendaItems.Add(menuAdmin);
            }
        }

        private void ExecuteLogoutAsync()
        {
            dataService.DeletePersonal(App.personal);
            App.Current.MainPage = new AdmeliApp.Pages.Root.LoginPage();
        }

        private void ExecuteHomePage()
        {
            Page page = new HomePage(); // Creando una instancia
            page.Title = "Home"; // Titulo de la página
            App.RootPage.Detail = new NavigationPage(page); // Detalle de la página
            App.RootPage.IsPresented = false; // Oculta el menu de navegacion
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
