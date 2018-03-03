using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        internal WebService webService = new WebService();
        internal DataService dataService = new DataService();

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { SetValue(ref userName, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetValue(ref password, value); }
        }

        private ICommand loginCommand;
        public ICommand LoginCommand =>
            loginCommand ?? (loginCommand = new Command(async () => await ExecuteLoginAsync()));

        public LoginViewModel()
        {
            this.IsRunning = false;
            this.IsEnabled = true;
            Password = "admin";
            UserName = "admin";
        }

        async Task ExecuteLoginAsync()
        {
            /// Verificar los campos
            if (string.IsNullOrEmpty(this.UserName))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Ingrese el nombre de usuario :(", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Ingrese la contraseña :(", "Aceptar");
                Password = "";
                return;
            }

            /// Actualizando los estados
            this.IsRunning = true;
            this.IsEnabled = false;

            /// Verificar conecion a internet
            Response connection = await this.webService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert("Alerta", connection.Message, "Aceptar");
                return;
            }

            /// Reciviendo el token de seguridad desde el web service
            ///


            /// Logueando El Usuario
            Personal personal = new Personal();
            personal.usuario = this.UserName;
            personal.password = this.Password;
            try
            {
                List<Personal> user = await webService.POST<Personal, List<Personal>>("personal", "buscar", personal);
                if (user.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El nombre de usuario o contraseña es incorrecta!!", "Aceptar");
                    return;
                }

                // cargar componentes desde el webservice
                await cargarComponente();

                /// Cargando todo los datos necesarios para el funcionamiento del sistema



                App.Current.MainPage = new AdmeliApp.Pages.Root.RootPage();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                this.IsRunning = false;
                this.IsEnabled = true;
            }

            /// Almacenando el usuario logueado en una base de datos local
            dataService.InsertPersonal(personal);


            /// Traendo todo los campos necesarios
        }

        private async Task cargarComponente()
        {
            // www.lineatienda.com/services.php/generales
            List<DatosGenerales> list = await webService.GET<List<DatosGenerales>>("generales");

            // www.lineatienda.com/services.php/configeneral
            List<ConfiguracionGeneral> list = await webService.GET<List<ConfiguracionGeneral>>("configeneral");
            configuracionGeneral = list[0];

            // www.lineatienda.com/services.php/sucursalespersonal/8
            List<Sucursal> list = await webService.GET<List<Sucursal>>("sucursalespersonal", idPersonal.ToString());
            if (list.Count == 0) throw new Exception("Usted no pertenece a una sucursal no podrá ingresar al sistema.");
            sucursal = list[0];

            // www.lineatienda.com/services.php/personales/asignacionpersonal/per/8/suc/1
            AsignacionPersonal datos = await webService.GET<AsignacionPersonal>("personales", String.Format("asignacionpersonal/per/{0}/suc/{1}", idPersonal, idSucursal));
            asignacionPersonal = datos;

            // www.lineatienda.com/services.php/cajasesion/idasignarcaja/3
            List<CajaSesion> list = await webService.GET<List<CajaSesion>>("cajasesion", String.Format("idasignarcaja/{0}", idAsignarCaja));
            if (list.Count > 0)
            {
                cajaSesion = list[0];
                ConfigModel.cajaIniciada = true;
            }
            else
            {
                ConfigModel.cajaIniciada = false;
            }

            // www.lineatienda.com/services.php/asignarpuntoventas/sucursal/1/personal/8
            List<PuntoDeVenta> list = await webService.GET<List<PuntoDeVenta>>("asignarpuntoventas", String.Format("sucursal/{0}/personal/{1}", idSucursal, idPersonal));
            puntosDeVenta = list;

            // www.lineatienda.com/services.php/personalalmacenes/per/8/suc/1
            List<Almacen> list = await webService.GET<List<Almacen>>("personalalmacenes", String.Format("per/{0}/suc/{1}", idPersonal, idSucursal));
            alamacenes = list;

            // www.lineatienda.com/services.php/tipodoc21/estado/1
            List<TipoDocumento> list = await webService.GET<List<TipoDocumento>>("tipodoc21", "estado/1");
            tipoDocumento = list;

            // www.lineatienda.com/services.php/tipocambiotodasmonedas/hoy
            List<TipoCambioMoneda> list = await webService.GET<List<TipoCambioMoneda>>("tipocambiotodasmonedas", "hoy");
            tipoCambioMonedas = list;

            // www.lineatienda.com/services.php/monedas/estado/1
            List<Moneda> list = await webService.GET<List<Moneda>>("monedas", "estado/1");
            monedas = list;
        }
    }
}
