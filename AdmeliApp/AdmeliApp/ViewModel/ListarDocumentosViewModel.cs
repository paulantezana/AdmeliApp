using AdmeliApp.Helpers;
using AdmeliApp.ViewModel.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.ViewModel
{
    public class ListarDocumentosViewModel
    {
        internal WebService webService = new WebService();

        public ListarDocumentosItemViewModel CurrentListarDocumentos { get; set; }


        #region =============================== SINGLETON ===============================
        private static ListarDocumentosViewModel instance;

        public static ListarDocumentosViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ListarDocumentosViewModel();
            }
            return instance;
        }
        #endregion
    }
}
