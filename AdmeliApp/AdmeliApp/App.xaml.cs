using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdmeliApp.Model;
using AdmeliApp.Pages.CompraPages;
using AdmeliApp.Pages.ConfiguracionPages;
using AdmeliApp.Pages.ConfiguracionPages.ConfiguracionItemPages;
using AdmeliApp.Pages.ProductoPages;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using AdmeliApp.Pages.Root;
using AdmeliApp.Pages.VentaPages;
using Xamarin.Forms;

namespace AdmeliApp
{
	public partial class App : Application
	{
        internal static ConfiguracionGeneral configuracionGeneral;
        internal static List<DatosGenerales> datosGeneralesList;
        internal static int currentIdAlmacen;
        internal static int currentIdPuntoVenta;
        internal static AsignacionPersonal asignacionPersonal;
        internal static bool cajaIniciada;
        internal static CajaSesion cajaSesion;
        internal static List<PuntoVenta> puntosDeVenta;
        internal static List<Almacen> alamacenes;
        internal static List<TipoDocumento> tipoDocumentos;
        internal static List<TipoCambioMoneda> tipoCambioMonedas;
        internal static List<Moneda> monedas;
        internal static Personal personal;

        public static MarcaPage MarcaPage { get; internal set; }
        public static MarcaItemPage MarcaItemPage { get; internal set; }
        public static AlmacenPage AlmacenPage { get; internal set; }
        public static AsignarCorrelativoPage AsignarCorrelativoPage { get; internal set; }
        public static DatosEmpresaPage DatosEmpresaPage { get; internal set; }
        public static DenominacionPage DenominacionPage { get; internal set; }
        public static DocIdentificacionPage DocIdentificacionPage { get; internal set; }
        public static GrupoClientePage GrupoClientePage { get; internal set; }
        public static ImpuestoPage ImpuestoPage { get; internal set; }
        public static MonedaPage MonedaPage { get; internal set; }
        public static PersonalPage PersonalPage { get; internal set; }
        public static PuntoVentaPage PuntoVentaPage { get; internal set; }
        public static SucursalPage SucursalPage { get; internal set; }
        public static TipoCambioPage TipoCambioPage { get; internal set; }
        public static ClientePage ClientePage { get; internal set; }
        public static VentaPage VentaPage { get; internal set; }
        public static DescuentoPage DescuentoPage { get; internal set; }
        public static CotizacionPage CotizacionPage { get; internal set; }
        public static ListarDocumentosPage TipoDocumentoPage { get; internal set; }
        public static CajaInicializadaPage CajaInicializadaPage { get; internal set; }
        public static SucursalItemPage SucursalItemPage { get; internal set; }
        public static ProductosPage ProductosPage { get; internal set; }
        public static ProductoItemPage ProductoItemPage { get; internal set; }
        public static ProveedorPage ProveedorPage { get; internal set; }
        public static CompraPage CompraPage { get; internal set; }
        public static OrdenCompraPage OrdenCompraPage { get; internal set; }
        public static List<Sucursal> sucursales { get; internal set; }
        public static Sucursal sucursal { get; internal set; }
        public static RootPage RootPage { get; internal set; }
        public static UnidadMedidaPage UnidadMedidaPage { get; internal set; }
        public static UnidadMedidaItemPage UnidadMedidaItemPage { get; internal set; }
        public static CategoriaPage CategoriaPage { get; internal set; }
        public static CategoriaItemPage CategoriaItemPage { get; internal set; }

        public App ()
		{
			InitializeComponent();

            MainPage = new AdmeliApp.Pages.Root.LoginPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
