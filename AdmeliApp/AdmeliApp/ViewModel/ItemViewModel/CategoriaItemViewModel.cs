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
            this.RootLoad();
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
        // Listar Mostrar en ---------------------------------------------------------------------
        // =======================================================================================
        private MostrarEn _MostrarEnSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public MostrarEn MostrarEnSelectedItem
        {
            get { return this._MostrarEnSelectedItem; }
            set
            {
                SetValue(ref this._MostrarEnSelectedItem, value);
            }
        }

        private List<MostrarEn> _MostrarEnItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<MostrarEn> MostrarEnItems
        {
            get { return this._MostrarEnItems; }
            set { SetValue(ref this._MostrarEnItems, value); }
        }

        // =======================================================================================
        // Listar Orden Visual ----------------------------------------------------------------
        // =======================================================================================
        private OrdenVisual _OrdenVisualSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public OrdenVisual OrdenVisualSelectedItem
        {
            get { return this._OrdenVisualSelectedItem; }
            set
            {
                SetValue(ref this._OrdenVisualSelectedItem, value);
            }
        }

        private List<OrdenVisual> _OrdenVisualPadreItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<OrdenVisual> OrdenVisualPadreItems
        {
            get { return this._OrdenVisualPadreItems; }
            set { SetValue(ref this._OrdenVisualPadreItems, value); }
        }

        #region =============================== LOADS ===============================
        public void RootLoad()
        {
            this.LoadCategorias();
            this.LoadMostrarEn();
            this.LoadOrdenVisual();
        }

        private void LoadMostrarEn()
        {
            MostrarEnItems = new List<MostrarEn>()
            {
                new MostrarEn()
                {
                    idMostrarEn = 1,
                    nombre = "En Cuadriculas"
                },
                new MostrarEn()
                {
                    idMostrarEn = 2,
                    nombre = "En Listas"
                },
            };
        }

        private void LoadOrdenVisual()
        {
            OrdenVisualPadreItems = new List<OrdenVisual>()
            {
                new OrdenVisual()
                {
                    idOrdenVisual = 1,
                    nombre = "Precio: Menos a Mas"
                },
                new OrdenVisual()
                {
                    idOrdenVisual = 2,
                    nombre = "Precio: Mas a Menos"
                },
                new OrdenVisual()
                {
                    idOrdenVisual = 3,
                    nombre = "Segun Nombre"
                },
                new OrdenVisual()
                {
                    idOrdenVisual = 4,
                    nombre = "Fecha: Modificacion"
                },
                new OrdenVisual()
                {
                    idOrdenVisual = 5,
                    nombre = "Promedio de Puntuacion"
                },
                new OrdenVisual()
                {
                    idOrdenVisual = 6,
                    nombre = "Numero de Comentarios"
                },
            };
        }

        private async void LoadCategorias()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/categorias21/-1
                int CategoriaID = (idCategoria > 0) ? idCategoria : -1;
                List<Categoria> datos = await webService.GET<List<Categoria>>("categorias21", String.Format("{0}", CategoriaID));
                CategoriaPadreItems = datos;

                CategoriaPadreSelectedItem = datos.Find(c => c.idCategoria == this.idPadreCategoria); // Selecciona categoria padre por defecto
                OrdenVisualSelectedItem = OrdenVisualPadreItems.Find(x => x.idOrdenVisual == 1);
                MostrarEnSelectedItem = MostrarEnItems.Find(x => x.idMostrarEn == 1);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            finally
            {
                this.IsRefreshing = false;
                this.IsEnabled = true;
            }
        }
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteEditar()
        {
            CategoriaViewModel categoriaViewModel = CategoriaViewModel.GetInstance();
            categoriaViewModel.SetCurrentCategoria(this);

            App.CategoriaPage.Navigation.PushAsync(new CategoriaItemPage()); // Navegacion

            this.Nuevo = false; /// Importante indicaque se modificara el registro actual
            this.DeleteIsEnabled = true;

            // Establecer valores al modificar
            CategoriaPadreSelectedItem = CategoriaPadreItems.Find(c => c.idCategoria == this.idPadreCategoria); // selecciona la categoria por defecto o la categoria seleccionada
            MostrarEnSelectedItem = MostrarEnItems.Find(x => x.idMostrarEn == this.mostrarProductosEn);
            OrdenVisualSelectedItem = OrdenVisualPadreItems.Find(x => x.idOrdenVisual == this.ordenVisualizacionProductos);
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
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Nombre de la categoria \n Campo obligatoria", "Aceptar");
                    return;
                }

                if (CategoriaPadreSelectedItem == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Seleccione un categoria padre \n Campo obligatoria", "Aceptar");
                    return;
                }

                // Estados
                this.IsRunning = true;
                this.IsEnabled = false;

                // Preparando el objeto para enviar
                this.idPadreCategoria = CategoriaPadreSelectedItem.idCategoria; // CATEGORIA PADRE
                this.padre = CategoriaPadreSelectedItem.nombreCategoria; // CATEGORIA PADRE

                this.ordenVisualizacionProductos = OrdenVisualSelectedItem.idOrdenVisual;
                this.mostrarProductosEn = MostrarEnSelectedItem.idMostrarEn;

                this.numeroColumnas = (this.numeroColumnas == 0) ? 1 : this.numeroColumnas; // Numero de datos si es cero valor por defecto 1
                this.orden = (this.orden == 0) ? 1 : this.orden; // Numero de orden si es 0 es 0

                if (this.Nuevo)
                {
                    this.afecta = true;
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

    public class OrdenVisual
    {
        public int idOrdenVisual { get; set; }
        public string nombre { get; set; }
    }

    public class MostrarEn 
    {
        public int idMostrarEn { get; set; }
        public string nombre { get; set; }
    }
}
