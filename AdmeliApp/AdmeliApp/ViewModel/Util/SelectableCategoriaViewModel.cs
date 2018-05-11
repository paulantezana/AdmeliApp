using AdmeliApp.Helpers;
using AdmeliApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.Util
{
    public class SelectableCategoriaViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        private ObservableCollection<SelectableCategoria> _SelectCategoriaItems;
        public ObservableCollection<SelectableCategoria> SelectCategoriaItems
        {
            get { return this._SelectCategoriaItems; }
            set { SetValue(ref this._SelectCategoriaItems, value); }
        }

        #region ======================== CONSTRUCTOR ========================
        public SelectableCategoriaViewModel()
        {
            this.RootLoad();
            this.SelectCategoriaItems = new ObservableCollection<SelectableCategoria>();
        }
        #endregion

        #region ========================== LOADS ==========================
        private void RootLoad()
        {
            this.LoadCategorias();
        }

        private async void LoadCategorias()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                int estado = 1;

                // www.lineatienda.com/services.php/categoriastodo/estado/1
                List<Categoria> categorias = await webService.GET<List<Categoria>>("categoriastodo", String.Format("estado/{0}", estado));

                foreach (Categoria c in categorias)
                {
                    SelectCategoriaItems.Add(new SelectableCategoria()
                    {
                        nombreCategoria = c.nombreCategoria,
                        idCategoria = c.idCategoria,
                        idPadreCategoria = c.idPadreCategoria,
                        padre = c.padre,
                        IsSelected = false
                    });
                }
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
    }
}
