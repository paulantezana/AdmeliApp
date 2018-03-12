using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.Helpers
{
    public class BaseViewModelPagination : BaseModel
    {
        internal Paginacion paginacion = new Paginacion(1, App.configuracionGeneral.itemPorPagina); // Configuracion inicial de la paginacion

        private int _CurrentPage;
        public int CurrentPage
        {
            get { return this._CurrentPage; }
            set
            {
                SetValue(ref this._CurrentPage, value);
                this.paginacion.reloadPage((value == 0) ? 1 : value);
                this.LoadRegisters();
            }
        }

        public virtual async void LoadRegisters()
        {
            await App.Current.MainPage.DisplayAlert("Load registers","Load resgisters" ,"Aceptar");
        }

        private string _NRegistros;
        public string NRegistros
        {
            get { return this._NRegistros; }
            set { SetValue(ref this._NRegistros, value); }
        }

        private bool _FirstIsVisible;
        public bool FirstIsVisible
        {
            get { return this._FirstIsVisible; }
            set { SetValue(ref this._FirstIsVisible, value); }
        }

        private bool _PreviousIsVisible;
        public bool PreviousIsVisible
        {
            get { return this._PreviousIsVisible; }
            set { SetValue(ref this._PreviousIsVisible, value); }
        }

        private bool _NextIsVisible;
        public bool NextIsVisible
        {
            get { return this._NextIsVisible; }
            set { SetValue(ref this._NextIsVisible, value); }
        }

        private bool _LastIsVisible;
        public bool LastIsVisible
        {
            get { return this._LastIsVisible; }
            set { SetValue(ref this._LastIsVisible, value); }
        }

        private ICommand _FirstCommand;
        public ICommand FirstCommand =>
            _FirstCommand ?? (_FirstCommand = new Command(() => ExecuteFirstPage()));

        private ICommand _PreviousCommand;
        public ICommand PreviousCommand =>
            _PreviousCommand ?? (_PreviousCommand = new Command(() => ExecutePreviousPage()));

        private ICommand _NextCommand;
        public ICommand NextCommand =>
            _NextCommand ?? (_NextCommand = new Command(() => ExecuteNextPage()));

        private ICommand _LastCommand;
        public ICommand LastCommand =>
            _LastCommand ?? (_LastCommand = new Command(() => ExecutelastPage()));

        private void ExecuteFirstPage()
        {
            if (this.CurrentPage != 1)
            {
                this.paginacion.firstPage();
                this.LoadRegisters();
            }
        }

        private void ExecutePreviousPage()
        {
            if (this.CurrentPage != 1)
            {
                this.paginacion.previousPage();
                this.LoadRegisters();
            }
        }

        private void ExecuteNextPage()
        {
            if (this.paginacion.pageCount != this.CurrentPage)
            {
                this.paginacion.nextPage();
                this.LoadRegisters();
            }
        }

        private void ExecutelastPage()
        {
            if (this.paginacion.pageCount != this.CurrentPage)
            {
                this.paginacion.lastPage();
                this.LoadRegisters();
            }
        }

        protected void reloadPagination()
        {
            // Print data pagination
            this.CurrentPage = paginacion.currentPage;
            this.NRegistros = String.Format("{0} Registros de {1} ", App.configuracionGeneral.itemPorPagina, this.paginacion.itemsCount);

            // Pagination button navigation is visible false or true
            this.NextIsVisible = ((this.paginacion.pageCount - this.paginacion.currentPage) >= 1);
            this.LastIsVisible = ((this.paginacion.pageCount - this.paginacion.currentPage) >= 2);
            this.PreviousIsVisible = (this.paginacion.currentPage >= 2);
            this.FirstIsVisible = (this.paginacion.currentPage >= 3);
        }
    }
}
