using AdmeliApp.Helpers;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace AdmeliApp.Pages.Root
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private WebService webService = new WebService();
        List<Entry> entries = new List<Entry>();

		public HomePage ()
		{
			InitializeComponent ();
            loadDataChart();
        }

        struct ultimasVentas
        {
            public string dia { get; set; }
            public dynamic idVenta { get; set; }
            public string total { get; set; }
        }

        private async void loadDataChart()
        {
            try
            {
                // localhost:8080/admeli/xcore/services.php/ventaspormes
                List<ultimasVentas> ventas = await webService.GET<List<ultimasVentas>>("ventaspormes");
                foreach (ultimasVentas item in ventas)
                {
                    entries.Add(new Entry(int.Parse(item.total))
                    {
                        Color = SKColor.Parse("#36A2EB"),
                        Label = item.dia,
                        ValueLabel = item.total,
                    });
                }
                Chart1.Chart = new LineChart { Entries = entries };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}