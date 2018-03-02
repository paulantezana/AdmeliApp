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
                    throw new Exception("El nombre de usuario o contraseña es incorrecta!!");
                }
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

            /// Traendo todo los campos necesarios
        }
    }
}
