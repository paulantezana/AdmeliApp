using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdmeliApp.Pages.Navigation
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MenuGrouping> MenuTiendaItems { get; set; }

        public MenuViewModel()
        {
            MenuTiendaItems = new ObservableCollection<MenuGrouping>()
            {
                new MenuGrouping("Ventas")
                {
                    new MenuTienda() { Id = 100, Icon = "icon.png", Title = "Contizaciones de cliente" },
                    new MenuTienda() { Id = 101, Icon = "icon.png", Title = "Ventas" },
                    new MenuTienda() { Id = 102, Icon = "icon.png", Title = "Clientes" },
                    new MenuTienda() { Id = 103, Icon = "icon.png", Title = "Descuento y oferta" }
                },

                new MenuGrouping("Compras")
                {
                    new MenuTienda() { Id = 201, Icon = "icon.png", Title = "Órden compra proveedor" },
                    new MenuTienda() { Id = 202, Icon = "icon.png", Title = "Compras" },
                    new MenuTienda() { Id = 203, Icon = "icon.png", Title = "Proveedores" }
                },

                new MenuGrouping("Productos")
                {
                    new MenuTienda() { Id = 301, Icon = "icon.png", Title = "Listado de productos" },
                    new MenuTienda() { Id = 302, Icon = "icon.png", Title = "Marcas" },
                    new MenuTienda() { Id = 303, Icon = "icon.png", Title = "Unidades de medida" },
                    new MenuTienda() { Id = 304, Icon = "icon.png", Title = "Categorias" }
                },

                new MenuGrouping("Almacen")
                {
                    new MenuTienda() { Id = 401, Icon = "icon.png", Title = "Nota de entrada" },
                    new MenuTienda() { Id = 402, Icon = "icon.png", Title = "Nota de salida" },
                    new MenuTienda() { Id = 403, Icon = "icon.png", Title = "Guía de remisión" }
                },

                new MenuGrouping("Caja")
                {
                    new MenuTienda() { Id = 501, Icon = "icon.png", Title = "Egresos" },
                    new MenuTienda() { Id = 502, Icon = "icon.png", Title = "Ingresos" },
                    new MenuTienda() { Id = 503, Icon = "icon.png", Title = "Cuentas por cobrar" },
                    new MenuTienda() { Id = 504, Icon = "icon.png", Title = "Cuentas por pagar" },
                    new MenuTienda() { Id = 505, Icon = "icon.png", Title = "Iniciar caja" },
                    new MenuTienda() { Id = 506, Icon = "icon.png", Title = "Cierre de caja" }
                },

                new MenuGrouping("Herramientas")
                {
                    new MenuTienda() { Id = 601, Icon = "icon.png", Title = "Stock precio" },
                    new MenuTienda() { Id = 602, Icon = "icon.png", Title = "Asignar categorías" },
                    new MenuTienda() { Id = 603, Icon = "icon.png", Title = "Asignar impuestos" },
                    new MenuTienda() { Id = 604, Icon = "icon.png", Title = "Generar código de barras" }
                },

                new MenuGrouping("Reportes")
                {
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Existencia producto" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Ingresos ventas" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Impuestos" }
                },

                new MenuGrouping("Configuración")
                {
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Datos Empresa" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Sucursales" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Punto de venta" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Almacenes" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Documentos de identificación" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Personal" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Listado documentos" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Asignar correlativos" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Diseño personalización" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Monedas" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Tipo cambio" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Denominaciones" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Impuestos" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Grupos de cliente" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Cajas inicializadas" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Impuestos documento" },
                    new MenuTienda() { Id = 1, Icon = "icon.png", Title = "Diseño de código de barras" },
                }
            };
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
