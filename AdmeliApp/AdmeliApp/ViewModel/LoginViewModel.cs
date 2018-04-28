using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class LoginViewModel : BaseModel
    {
        internal WebService webService = new WebService();
        internal DataService dataService = new DataService();

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetValue(ref _UserName, value); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetValue(ref _Password, value); }
        }

        private bool _IsRemembered;
        public bool IsRemembered
        {
            get { return _IsRemembered; }
            set { SetValue(ref _IsRemembered, value); }
        }

        private int nLoads;

        private ICommand loginCommand;
        public ICommand LoginCommand =>
            loginCommand ?? (loginCommand = new Command(() => ExecuteLoginAsync()));

        public LoginViewModel()
        {
            this.IsRunning = false;
            this.IsEnabled = true;

            /// Recuperando la contraseña si fue guardada
            Personal OldPersonal = dataService.GetPersonal();
            if (OldPersonal != null && OldPersonal.IsRemembered)
            {
                this.Password = OldPersonal.password;
                this.UserName = OldPersonal.usuario;
                this.IsRemembered = OldPersonal.IsRemembered;
                this.ExecuteLoginAsync();
            }
        }

        private async void ExecuteLoginAsync()
        {
            /// validacion de los campos
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
            //Response connection = await this.webService.CheckConnection();
            //if (!connection.IsSuccess)
            //{
            //    this.IsRunning = false;
            //    this.IsEnabled = true;
            //    await Application.Current.MainPage.DisplayAlert("Alerta", connection.Message, "Aceptar");
            //    return;
            //}

            try
            {
                /// Logueando El Usuario
                Personal personal = new Personal();
                personal.usuario = this.UserName;
                personal.password = this.Password;

                // Login del personal
                List<Personal> user = await webService.POST<Personal, List<Personal>>("personal", "buscar", personal);
                if (user.Count == 0) // Contraseña y usuario incorrecta
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "El nombre de usuario o contraseña es incorrecta!!", "Aceptar");
                    return;
                }
                App.personal = user[0];

                // Lamacenando en una base de datos local si el usuario indico recordar contraseña
                if (IsRemembered)
                {
                    App.personal.IsRemembered = this.IsRemembered;
                    App.personal.password = this.Password;
                    dataService.InsertPersonal(App.personal);
                }

                // cargar componentes desde el webservice
                await cargarComponente();

                // esperar a que cargen todo los web service
                await Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(50);
                        if (nLoads >= 10) // IMPORTANTE IMPORTANTE el numero tiene que ser igual al numero de web service que se este llamando
                        {
                            break;
                        }
                    }
                });

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
        }

        private async Task cargarComponente()
        {
            loadDatosGenerales();

            // www.lineatienda.com/services.php/sucursalespersonal/8
            List<Sucursal> sList = await webService.GET<List<Sucursal>>("sucursalespersonal", App.personal.idPersonal.ToString());
            if (sList.Count == 0) throw new Exception("Usted no pertenece a una sucursal no podrá ingresar al sistema.");
            App.sucursal = sList[0];
            this.nLoads++;

            // www.lineatienda.com/services.php/personales/asignacionpersonal/per/8/suc/1
            App.asignacionPersonal = await webService.GET<AsignacionPersonal>("personales", String.Format("asignacionpersonal/per/{0}/suc/{1}", App.personal.idPersonal, App.sucursal.idSucursal));
            this.nLoads++;

            loadConfiGeneral();

            loadMonedas();

            loadTipoCambioMonedas();

            loadTipoDocumento();

            loadAlmacenes();

            loadPuntoDeVenta();

            loadCajaSesion();

           // await configModel.loadCierreIngresoEgreso(1, ConfigModel.cajaSesion.idCajaSesion); // Falta Buscar de donde viene el primer parametro

        }

        private async void loadDatosGenerales()
        {
            // www.lineatienda.com/services.php/generales
            App.datosGeneralesList = await webService.GET<List<DatosGenerales>>("generales");
            this.nLoads++;
            //loadState("datos generales");
        }

        private async void loadConfiGeneral()
        {
            // www.lineatienda.com/services.php/configeneral
            List<ConfiguracionGeneral> cgList = await webService.GET<List<ConfiguracionGeneral>>("configeneral");
            App.configuracionGeneral = cgList[0];
            this.nLoads++;
            //loadState("configuracion general");
        }

        private async void loadCajaSesion()
        {
            // www.lineatienda.com/services.php/cajasesion/idasignarcaja/3
            List<CajaSesion> csList = await webService.GET<List<CajaSesion>>("cajasesion", String.Format("idasignarcaja/{0}", App.asignacionPersonal.idAsignarCaja));
            if (csList.Count > 0)
            {
                App.cajaSesion = csList[0];
                App.cajaIniciada = true;
            }
            else
            {
                App.cajaIniciada = false;
            }
            this.nLoads++;
            //loadState("caja session");
        }

        private async void loadPuntoDeVenta()
        {
            // www.lineatienda.com/services.php/asignarpuntoventas/sucursal/1/personal/8
            App.puntosDeVenta = await webService.GET<List<PuntoVenta>>("asignarpuntoventas", String.Format("sucursal/{0}/personal/{1}", App.sucursal.idSucursal, App.personal.idPersonal));

            this.nLoads++;
            //loadState("puntos de venta");
        }

        private async void loadAlmacenes()
        {
            // www.lineatienda.com/services.php/personalalmacenes/per/8/suc/1
            App.alamacenes = await webService.GET<List<Almacen>>("personalalmacenes", String.Format("per/{0}/suc/{1}", App.personal.idPersonal, App.sucursal.idSucursal));

            this.nLoads++;
            //loadState("almacenes");
        }

        private async void loadTipoDocumento()
        {
            // www.lineatienda.com/services.php/tipodoc21/estado/1
            App.tipoDocumentos = await webService.GET<List<TipoDocumento>>("tipodoc21", "estado/1");

            this.nLoads++;
            //loadState("tipos de documentos");
        }

        private async void loadTipoCambioMonedas()
        {
            // www.lineatienda.com/services.php/tipocambiotodasmonedas/hoy
            App.tipoCambioMonedas = await webService.GET<List<TipoCambioMoneda>>("tipocambiotodasmonedas", "hoy");

            this.nLoads++;
            //loadState("tipos de cambio");
        }

        private async void loadMonedas()
        {
            // www.lineatienda.com/services.php/monedas/estado/1
            App.monedas = await webService.GET<List<Moneda>>("monedas", "estado/1");

            this.nLoads++;
            //loadState("monedas");
        }
    }
}
