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
using VirtualCollection.Netflix;
using VirtualCollection.VirtualCollection;

namespace VirtualCollection.Demo
{
    public class MainViewModel : ViewModel
    {
        private string _search;
        private NetflixTitleSource _source;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                _source.Search = value;
                RaisePropertyChanged(() => Search);
            }
        }

        public int count;

        public VirtualCollection<Title> Items { get; private set; }


        public MainViewModel()
        {
            _source = new NetflixTitleSource();
            Items = new VirtualCollection<Title>(_source, 25, 25);
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            Items.Refresh();
        }
    }
}
