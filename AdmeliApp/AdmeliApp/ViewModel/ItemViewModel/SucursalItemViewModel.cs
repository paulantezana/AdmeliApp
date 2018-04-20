using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Model.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.ViewModel.ItemViewModel
{
    public class SucursalItemViewModel : Sucursal
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

        #region ========================== LOCALIZACION ==========================

        // Pais
        private Pais _PaisSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Pais PaisSelectedItem
        {
            get { return this._PaisSelectedItem; }
            set { SetValue(ref this._PaisSelectedItem, value); }
        }

        private List<Pais> _PaisItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Pais> PaisItems
        {
            get { return this._PaisItems; }
            set { SetValue(ref this._PaisItems, value); }
        }

        // Nivel 1
        private Nivel1 _Nivel1SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel1 Nivel1SelectedItem
        {
            get { return this._Nivel1SelectedItem; }
            set { SetValue(ref this._Nivel1SelectedItem, value); }
        }

        private List<Nivel1> _Nivel1Items;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel1> Nivel1Items
        {
            get { return this._Nivel1Items; }
            set { SetValue(ref this._Nivel1Items, value); }
        }

        // Nivel 2
        private Nivel2 _Nivel2SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel2 Nivel2SelectedItem
        {
            get { return this._Nivel2SelectedItem; }
            set { SetValue(ref this._Nivel2SelectedItem, value); }
        }

        private List<Nivel2> _Nivel2Items;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel2> Nivel2Items
        {
            get { return this._Nivel2Items; }
            set { SetValue(ref this._Nivel2Items, value); }
        }

        // Nivel 3
        private Nivel3 _Nivel3SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel3 Nivel3SelectedItem
        {
            get { return this._Nivel3SelectedItem; }
            set { SetValue(ref this._Nivel3SelectedItem, value); }
        }

        private List<Nivel3> _Nivel3tems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel3> Nivel3tems
        {
            get { return this._Nivel3tems; }
            set { SetValue(ref this._Nivel3tems, value); }
        }


        #endregion
    }
}
