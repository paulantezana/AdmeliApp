using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;
using Xamarin.Forms;

namespace AdmeliApp.Helpers
{
    public class BaseModel : INotifyPropertyChanged
    {
        private bool _IsRunning;

        [Ignore]
        [JsonIgnore]
        public bool IsRunning
        {
            get { return this._IsRunning; }
            set { SetValue(ref this._IsRunning, value); }
        }

        private bool _IsEnabled;

        [Ignore]
        [JsonIgnore]
        public bool IsEnabled
        {
            get { return this._IsEnabled; }
            set { SetValue(ref this._IsEnabled, value); }
        }

        private bool _IsRefreshing;

        [Ignore]
        [JsonIgnore]
        public bool IsRefreshing
        {
            get { return this._IsRefreshing; }
            set { SetValue(ref this._IsRefreshing, value); }
        }

        [Ignore]
        [JsonIgnore]
        public Color BackgroundItem { get; set; }

        [Ignore]
        [JsonIgnore]
        public Color TextColorItem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
