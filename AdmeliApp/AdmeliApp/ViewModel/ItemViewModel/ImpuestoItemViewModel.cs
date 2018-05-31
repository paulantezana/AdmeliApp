using AdmeliApp.Helpers;
using AdmeliApp.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class ImpuestoItemViewModel : Impuesto
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

        private bool _ToggleOptionsIsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool ToggleOptionsIsVisible
        {
            get { return this._ToggleOptionsIsVisible; }
            set { SetValue(ref this._ToggleOptionsIsVisible, value); }
        }

        private string _IconToggleOptions;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string IconToggleOptions
        {
            get { return this._IconToggleOptions; }
            set { SetValue(ref this._IconToggleOptions, value); }
        }

        #region ================================= COMMANDS =================================
        //private ICommand _GuardarCommand;
        //[JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        //public ICommand GuardarCommand =>
        //    _GuardarCommand ?? (_GuardarCommand = new Command(() => ExecuteGuardarAsync()));

        //private ICommand _EditarCommand;
        //[JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        //public ICommand EditarCommand =>
        //    _EditarCommand ?? (_EditarCommand = new Command(() => ExecuteEditar()));

        //private ICommand _AnularCommand;
        //[JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        //public ICommand AnularCommand =>
        //    _AnularCommand ?? (_AnularCommand = new Command(() => ExecuteAnular()));

        //private ICommand _EliminarCommand;
        //[JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        //public ICommand EliminarCommand =>
        //    _EliminarCommand ?? (_EliminarCommand = new Command(() => ExecuteEliminar()));

        private ICommand _ToggleOptionsCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand ToggleOptionsCommand =>
            _ToggleOptionsCommand ?? (_ToggleOptionsCommand = new Command(() => ExecuteToggleOptions()));
        #endregion

        #region ================================ CONSTRUCTOR ================================
        public ImpuestoItemViewModel()
        {
            // Estados
            this.IsRunning = false;
            this.IsEnabled = true;
            this.IconToggleOptions = "expandToggle_icon.png"; //Icono por defecto para expandir la item de la lista
            this.estado = 1;
        }
        #endregion

        #region =============================== COMMAND EXECUTE ===============================
        private void ExecuteToggleOptions()
        {
            this.ToggleOptionsIsVisible = !this.ToggleOptionsIsVisible;
            IconToggleOptions = (ToggleOptionsIsVisible) ? "collapseToggle_icon.png" : "expandToggle_icon.png"; //Cambiando los iconos en tiempo real
        }
        #endregion
    }
}
