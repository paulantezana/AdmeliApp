using AdmeliApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdmeliApp.Pages.ProductoPages.ProductoItemPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductoItemPage : TabbedPage
    {
        public ProductoItemPage ()
        {
            InitializeComponent();
            ProductoViewModel productoViewModel = ProductoViewModel.GetInstance();
            BindingContext = productoViewModel.CurrentProducto;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.ProductoItemPage = this;
        }
    }
}