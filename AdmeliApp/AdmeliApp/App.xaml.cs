using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using Xamarin.Forms;

namespace AdmeliApp
{
	public partial class App : Application
	{
        internal static ConfiguracionGeneral configuracionGeneral;
        internal static List<DatosGenerales> datosGeneralesList;
        internal static Sucursal sucursal;
        internal static AsignacionPersonal asignacionPersonal;
        internal static bool cajaIniciada;
        internal static CajaSesion cajaSesion;
        internal static List<PuntoDeVenta> puntosDeVenta;
        internal static List<Almacen> alamacenes;
        internal static List<TipoDocumento> tipoDocumentos;
        internal static List<TipoCambioMoneda> tipoCambioMonedas;
        internal static List<Moneda> monedas;
        internal static Personal personal;

        public static MarcaPage MarcaPage { get; internal set; }
        public static MarcaItemPage MarcaItemPage { get; internal set; }

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
