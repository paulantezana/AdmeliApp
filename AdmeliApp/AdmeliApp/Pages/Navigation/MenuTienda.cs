using AdmeliApp.Pages.Root;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Pages.Navigation
{
    public class MenuTienda
    {
        public MenuTienda()
        {
            TargetType = typeof(HomePage);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}
