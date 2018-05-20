using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Pages.ProductoPages.ProductoItemPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class CategoriaItemViewModel : Categoria
    {
        internal WebService webService = new WebService();
        public bool Nuevo;

        private bool _DeleteIsEnabled;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool DeleteIsEnabled
        {
            get { return _DeleteIsEnabled; }
            set { SetValue(ref _DeleteIsEnabled, value); }
        }

        #region ================================= COMMANDS =================================
        private ICommand _GuardarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand GuardarCommand =>
            _GuardarCommand ?? (_GuardarCommand = new Command(() => ExecuteGuardarAsync()));

        private ICommand _EditarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand EditarCommand =>
            _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        private ICommand _AnularCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand AnularCommand =>
            _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));

        private ICommand _EliminarCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand EliminarCommand =>
            _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminar()));
        #endregion

        #region ================================ CONSTRUCTOR ================================
        public CategoriaItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
            this.estado = 1;
        }
        #endregion

        // =======================================================================================
        // Listar categoria padre ----------------------------------------------------------------
        // =======================================================================================
        private Categoria _CategoriaPadreSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Categoria CategoriaPadreSelectedItem
        {
            get { return this._CategoriaPadreSelectedItem; }
            set
            {
                SetValue(ref this._CategoriaPadreSelectedItem, value);
            }
        }

        private List<Categoria> _CategoriaPadreItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Categoria> CategoriaPadreItems
        {
            get { return this._CategoriaPadreItems; }
            set { SetValue(ref this._CategoriaPadreItems, value); }
        }

        // =======================================================================================
        // Listar Mostrar en ----------------------------------------------------------------
        // =======================================================================================
        private Categoria _MostrarEnSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Categoria MostrarEnSelectedItem
        {
            get { return this._MostrarEnSelectedItem; }
            set
            {
                SetValue(ref this._MostrarEnSelectedItem, value);
            }
        }

        private List<Categoria> _MostrarEnPadreItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Categoria> MostrarEnPadreItems
        {
            get { return this._MostrarEnPadreItems; }
            set { SetValue(ref this._MostrarEnPadreItems, value); }
        }

        // =======================================================================================
        // Listar Orden Visual ----------------------------------------------------------------
        // =======================================================================================
        private Categoria _OrdenVisualSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Categoria OrdenVisualSelectedItem
        {
            get { return this._OrdenVisualSelectedItem; }
            set
            {
                SetValue(ref this._OrdenVisualSelectedItem, value);
            }
        }

        private List<Categoria> _OrdenVisualPadreItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Categoria> OrdenVisualPadreItems
        {
            get { return this._OrdenVisualPadreItems; }
            set { SetValue(ref this._OrdenVisualPadreItems, value); }
        }


        #region =============================== LOADS ===============================

        #endregion


        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            CategoriaViewModel categoriaViewModel = CategoriaViewModel.GetInstance();
            categoriaViewModel.SetCurrentCategoria(this);
            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            this.DeleteIsEnabled = true;
            App.CategoriaPage.Navigation.PushAsync(new CategoriaItemPage()); // Navegacion
        }

        private async void ExecuteAnular()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                /// Verificacion si el registro esta anulado
                if (this.estado == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Anular", "Este registro ya esta anulado \n" + this.nombreCategoria, "Aceptar");
                    return;
                }

                /// pregunta al usuario (Confirmacion)
                if (await App.Current.MainPage.DisplayAlert("Anular", "¿esta seguro de anular este registro? \n" + this.nombreCategoria, "Aceptar", "Cancelar") == false) return;

                /// Preparando el objeto para enviar
                Categoria categoria = new Categoria();
                categoria.idCategoria = this.idCategoria;

                /// Ejecutando el webservice
                // localhost:8080/admeli/xcore2/xcore/services.php/categoria/desactivar
                Response response = await webService.POST<Categoria, Response>("categoria", "desactivar", (Categoria)this);

                // Message response
                await App.Current.MainPage.DisplayAlert("Anular", response.Message, "Aceptar");

                // Refrescar la lista
                MarcaViewModel.GetInstance().ExecuteRefresh();
            }
            catch (Exception ex)
            {
                // Error message
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                // Estados
                this.IsRunning = false;
                this.IsEnabled = true;
            }
        }

        private async void ExecuteGuardarAsync()
        {
            try
            {
                /// validacion de los campos
                if (string.IsNullOrEmpty(this.nombreCategoria))
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Campo obligatoria", "Aceptar");
                    return;
                }

                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // Preparando el objeto para enviar
                if (this.Nuevo)
                {
                    //this.CaptionImagen = "";
                    //this.UbicacionLogo = "";
                    //this.TieneRegistros = "";
                }

                if (this.Nuevo)
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/categorias/guardar
                    Response response = await webService.POST<Categoria, Response>("categorias", "guardar", (Categoria)this);
                    await App.Current.MainPage.DisplayAlert("Guardar", response.Message, "Aceptar");
                }
                else
                {
                    // localhost:8080/admeli/xcore2/xcore/services.php/categorias/modificar
                    Response response = await webService.POST<Categoria, Response>("categorias", "modificar", (Categoria)this);
                    await App.Current.MainPage.DisplayAlert("Modificar", response.Message, "Aceptar");
                }

                // Refrescar y regresar a la pagina anterior
                CategoriaViewModel.GetInstance().ExecuteRefresh();
                await App.CategoriaItemPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Error message
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                // Estados
                this.IsRunning = false;
                this.IsEnabled = true;
            }
        }

        private async void ExecuteEliminar()
        {
            try
            {
                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                /// pregunta al usuario (Confirmacion)
                if (await App.Current.MainPage.DisplayAlert("Eliminar", "¿esta seguro de eliminar este registro? \n" + this.nombreCategoria, "Aceptar", "Cancelar") == false) return;
                
                // localhost:8080/admeli/xcore2/xcore/services.php/categorias/eliminar
                Response response = await webService.POST<Categoria, Response>("categorias", "eliminar", (Categoria)this);
                await App.Current.MainPage.DisplayAlert("Eliminar", response.Message, "Aceptar");

                // Refrescar la lista
                MarcaViewModel.GetInstance().ExecuteRefresh();
            }
            catch (Exception ex)
            {
                // Error message
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                // Estados
                this.IsRunning = false;
                this.IsEnabled = true;
            }
        }
        #endregion
    }
}
