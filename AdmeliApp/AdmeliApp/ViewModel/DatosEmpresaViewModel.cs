﻿using AdmeliApp.Helpers;
using AdmeliApp.Model;
using AdmeliApp.Model.Location;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdmeliApp.ViewModel
{
    public class DatosEmpresaViewModel : BaseModel
    {
        internal WebService webService = new WebService();

        private DatosGenerales _DatosGeneralesData;
        public DatosGenerales DatosGeneralesData
        {
            get { return this._DatosGeneralesData; }
            set { SetValue(ref this._DatosGeneralesData, value); }
        }

        private ConfiguracionGeneral _ConfiguracionGeneralData;
        public ConfiguracionGeneral ConfiguracionGeneralData
        {
            get { return this._ConfiguracionGeneralData; }
            set { SetValue(ref this._ConfiguracionGeneralData, value); }
        }

        #region ============================== COMANDS ==============================
        private ICommand _GuardarConfiguracionGeneralCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand GuardarConfiguracionGeneralCommand =>
            _GuardarConfiguracionGeneralCommand ?? (_GuardarConfiguracionGeneralCommand = new Command(() => ExecuteGuardarConfiguracionGeneralesAsync()));

        private ICommand _GuardarDatosGeneralesCommand;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public ICommand GuardarDatosGeneralesCommand =>
            _GuardarDatosGeneralesCommand ?? (_GuardarDatosGeneralesCommand = new Command(() => ExecuteGuardarDatosGeneralAsync()));
        #endregion

        #region ============================ CONSTRUCTOR ============================
        public DatosEmpresaViewModel()
        {
            this.RootLoad();
        }
        #endregion

        #region ============================== LOAD DATA ==================================
        public void RootLoad()
        {
            this.LoadDatosGenerales();
            this.cargarPaises();
        }

        private async void LoadDatosGenerales()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/generales
                List<DatosGenerales> listData = await webService.GET<List<DatosGenerales>>("generales");
                DatosGeneralesData = listData[0];

                // www.lineatienda.com/services.php/configeneral
                List<ConfiguracionGeneral> list = await webService.GET<List<ConfiguracionGeneral>>("configeneral");
                ConfiguracionGeneralData = list[0];
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

        #region ==================================== EXECUTE ====================================
        private async void ExecuteGuardarDatosGeneralAsync()
        {
            try
            {
                gUbication.idPais = PaisSelectedItem.idPais;
                gUbication.idNivel1 = Nivel1SelectedItem.idNivel1;
                gUbication.idNivel2 = Nivel2SelectedItem.idNivel2;
                gUbication.idNivel3 = Nivel3SelectedItem.idNivel3;

                // www.admeli.com/demo2/services.php/ubigeo
                Response response = await webService.POST<UbicacionGeografica, Response>("ubigeo", gUbication);
                DatosGeneralesData.idUbicacionGeografica = response.Id;

                // localhost:8080/admeli/xcore2/xcore/services.php/datosgenerales/modificar
                Response responseDG = await webService.POST<DatosGenerales, Response>("datosgenerales", "modificar", DatosGeneralesData);
                await Application.Current.MainPage.DisplayAlert("Guardar", responseDG.Message, "Aceptar");
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

        private async void ExecuteGuardarConfiguracionGeneralesAsync()
        {
            try
            {
                // localhost:8080/admeli/xcore2/xcore/services.php/configuraciongeneral/modificar
                Response responseCG = await webService.POST<ConfiguracionGeneral, Response>("configuraciongeneral", "modificar", ConfiguracionGeneralData);
                
                await Application.Current.MainPage.DisplayAlert("Guardar", responseCG.Message, "Aceptar");
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

        #region ========================== LOCALIZACION ==========================

        // Variables
        private UbicacionGeografica gUbication;

        // Structura de los niveles de cada pais
        private Pais _PaisSelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Pais PaisSelectedItem
        {
            get { return this._PaisSelectedItem; }
            set
            {
                SetValue(ref this._PaisSelectedItem, value);
                if (_PaisSelectedItem != null) crearNivelesPais();
            }
        }

        private List<Pais> _PaisItems;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Pais> PaisItems
        {
            get { return this._PaisItems; }
            set { SetValue(ref this._PaisItems, value); }
        }

        // ----------------------------------------------------------------------------------------------------
        // Nivel 1 ============================================================================================
        // ----------------------------------------------------------------------------------------------------
        private Nivel1 _Nivel1SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel1 Nivel1SelectedItem
        {
            get { return this._Nivel1SelectedItem; }
            set
            {
                SetValue(ref this._Nivel1SelectedItem, value);
                if (_Nivel1SelectedItem != null) cargarNivel2();
            }
        }

        private List<Nivel1> _Nivel1Items;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel1> Nivel1Items
        {
            get { return this._Nivel1Items; }
            set { SetValue(ref this._Nivel1Items, value); }
        }

        private string _Nivel1Title;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string Nivel1Title
        {
            get { return this._Nivel1Title; }
            set { SetValue(ref this._Nivel1Title, value); }
        }

        private bool _Nivel1IsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool Nivel1IsVisible
        {
            get { return this._Nivel1IsVisible; }
            set { SetValue(ref this._Nivel1IsVisible, value); }
        }

        // ----------------------------------------------------------------------------------------------------
        // Nivel 2 ============================================================================================
        // ----------------------------------------------------------------------------------------------------
        private Nivel2 _Nivel2SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel2 Nivel2SelectedItem
        {
            get { return this._Nivel2SelectedItem; }
            set
            {
                SetValue(ref this._Nivel2SelectedItem, value);
                if (_Nivel2SelectedItem != null) cargarNivel3();
            }
        }

        private List<Nivel2> _Nivel2Items;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel2> Nivel2Items
        {
            get { return this._Nivel2Items; }
            set { SetValue(ref this._Nivel2Items, value); }
        }

        private string _Nivel2Title;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string Nivel2Title
        {
            get { return this._Nivel2Title; }
            set { SetValue(ref this._Nivel2Title, value); }
        }

        private bool _Nivel2IsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool Nivel2IsVisible
        {
            get { return this._Nivel2IsVisible; }
            set { SetValue(ref this._Nivel2IsVisible, value); }
        }

        // ----------------------------------------------------------------------------------------------------
        // Nivel 3 ============================================================================================
        // ----------------------------------------------------------------------------------------------------
        private Nivel3 _Nivel3SelectedItem;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public Nivel3 Nivel3SelectedItem
        {
            get { return this._Nivel3SelectedItem; }
            set { SetValue(ref this._Nivel3SelectedItem, value); }
        }

        private List<Nivel3> _Nivel3Items;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public List<Nivel3> Nivel3Items
        {
            get { return this._Nivel3Items; }
            set { SetValue(ref this._Nivel3Items, value); }
        }

        private string _Nivel3Title;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public string Nivel3Title
        {
            get { return this._Nivel3Title; }
            set { SetValue(ref this._Nivel3Title, value); }
        }

        private bool _Nivel3IsVisible;
        [JsonIgnore] /// Con esta linea se ignora en la serializacion con el web service
        public bool Nivel3IsVisible
        {
            get { return this._Nivel3IsVisible; }
            set { SetValue(ref this._Nivel3IsVisible, value); }
        }


        private async void cargarPaises()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // CARGANDO LOS PAISES 
                // www.lineatienda.com/services.php/pais
                PaisItems = await webService.GET<List<Pais>>("pais");

                // CARGANDO LA UBICACION GEOGRAFICA si es nuevo cargara la ubicacion geografica
                // www.lineatienda.com/services.php/ubigeoAtrib/2
                List<UbicacionGeografica> data = await webService.GET<List<UbicacionGeografica>>("ubigeoAtrib", String.Format("{0}", App.datosGeneralesList[0].idUbicacionGeografica));
                gUbication = data[0];

                // Seleccionar el pais por defecto segun la ubicacion geografica
                PaisSelectedItem = PaisItems.Find(p => p.idPais == gUbication.idPais);
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

        private async void crearNivelesPais()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // Cargando los labels de cada pais
                // www.lineatienda.com/services.php/lubicacion/pais/26
                List<LabelUbicacion> labelUbicaciones = await webService.GET<List<LabelUbicacion>>("lubicacion", String.Format("pais/{0}", PaisSelectedItem.idPais));

                if (labelUbicaciones.Count >= 1)
                {
                    Nivel1Title = labelUbicaciones[0].denominacion;
                    Nivel1IsVisible = true;
                }

                if (labelUbicaciones.Count >= 2)
                {
                    Nivel2Title = labelUbicaciones[1].denominacion;
                    Nivel2IsVisible = true;
                }

                if (labelUbicaciones.Count >= 3)
                {
                    Nivel3Title = labelUbicaciones[2].denominacion;
                    Nivel3IsVisible = true;
                }

                // Cragar datos de 
                // Nivel 1
                cargarNivel1();
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

        private async void cargarNivel1()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/pais/21/nivel1
                Nivel1Items = await webService.GET<List<Nivel1>>("pais", String.Format("{0}/nivel1", PaisSelectedItem.idPais));
                //if (gUbication.idNivel1 > 0)
                Nivel1SelectedItem = Nivel1Items.Find(n => n.idNivel1 == gUbication.idNivel1);
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

        private async void cargarNivel2()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/nivel1/482/nivel2
                Nivel2Items = await webService.GET<List<Nivel2>>("nivel1", String.Format("{0}/nivel2", Nivel1SelectedItem.idNivel1));
                if (gUbication.idNivel2 > 0) Nivel2SelectedItem = Nivel2Items.Find(n => n.idNivel2 == gUbication.idNivel2);
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

        private async void cargarNivel3()
        {
            try
            {
                this.IsRefreshing = true;
                this.IsEnabled = false;

                // www.lineatienda.com/services.php/nivel2/66153/nivel3
                Nivel3Items = await webService.GET<List<Nivel3>>("nivel2", String.Format("{0}/nivel3", Nivel2SelectedItem.idNivel2));
                if (gUbication.idNivel3 > 0) Nivel3SelectedItem = Nivel3Items.Find(n => n.idNivel3 == gUbication.idNivel3);
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
