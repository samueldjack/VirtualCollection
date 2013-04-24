using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VirtualCollection.Framework.MVVM;
using VirtualCollection.Northwind;
using VirtualCollection.VirtualCollection;
using VirtualCollection.Framework.Extensions;
using System.Reactive.Linq;

namespace VirtualCollection.Demo
{
    public class MainViewModel : ViewModel
    {
        private string _search;
        private NorthwindProductsSource _source;
        private string _displayStyle;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                RaisePropertyChanged(() => Search);
            }
        }

        public string DisplayStyle
        {
            get { return _displayStyle; }
            set
            {
                _displayStyle = value;
                RaisePropertyChanged(() => DisplayStyle);
            }
        }

        public IList<string> DisplayStyles { get { return new[] {"Card", "Details"}; } }
 
        public VirtualCollection<Product> Items { get; private set; }

        public MainViewModel()
        {
            _source = new NorthwindProductsSource();
            Items = new VirtualCollection<Product>(_source, pageSize: 20, cachedPages: 5);

            this.ObservePropertyChanged(() => Search)
                .Throttle(TimeSpan.FromSeconds(0.25))
                .ObserveOnDispatcher()
                .Subscribe(_ => _source.Search = Search);

            DisplayStyle = "Details";
        }

        protected override void OnViewLoaded()
        {
            Items.Refresh();
        }
    }
}
