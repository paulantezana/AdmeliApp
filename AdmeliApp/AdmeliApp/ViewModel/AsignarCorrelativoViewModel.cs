using AdmeliApp.Helpers;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.ViewModel
{
    public class AsignarCorrelativoViewModel
    {
        internal WebService webService = new WebService();

        public AsignarCorrelativoItemViewModel CurrentAsignarCorrelativo { get; set; }



        #region =============================== SINGLETON ===============================
        private static AsignarCorrelativoViewModel instance;

        public static AsignarCorrelativoViewModel GetInstance()
        {
            if (instance == null)
            {
                return new AsignarCorrelativoViewModel();
            }
            return instance;
        }
        #endregion
    }
}
