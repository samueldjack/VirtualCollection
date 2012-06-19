using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Services.Client;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using VirtualCollection.Netflix;
using VirtualCollection.VirtualCollection;

namespace VirtualCollection.Demo
{
    public class NetflixTitleSource : VirtualCollectionSource<Title>
    {
        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                Refresh(RefreshMode.ClearStaleData);
            }
        }

        protected override Task<int> GetCount()
        {
            return GetQueryResults(0, 1, null)
                .ContinueWith(t => (int)t.Result.TotalCount, TaskContinuationOptions.ExecuteSynchronously);
        }

        protected override Task<IList<Title>> GetPageAsyncOverride(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            return GetQueryResults(start, pageSize, sortDescriptions)
                .ContinueWith(t => (IList<Title>)((IEnumerable<Title>)t.Result).ToList(), TaskContinuationOptions.ExecuteSynchronously);
        }

        private Task<QueryOperationResponse<Title>> GetQueryResults(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            var context = new NetflixCatalog(new Uri("http://odata.netflix.com/Catalog"));

            var orderByString = CreateOrderByString(sortDescriptions);
            var query = context.Titles
                .AddQueryOption("$skip", start)
                .AddQueryOption("$top", pageSize)
                .IncludeTotalCount();

            if (!string.IsNullOrEmpty(Search))
            {
                query = query.AddQueryOption("$filter", "(substringof('" + Search + "',Name) eq true) and (BoxArt/SmallUrl ne null)");
            }
            else
            {
                query = query.AddQueryOption("$filter", "(BoxArt/SmallUrl ne null)");
            }

            if (orderByString.Length > 0)
            {
                query = query.AddQueryOption("$orderby", orderByString);
            }

            return Task.Factory.FromAsync<IEnumerable<Title>>(query.BeginExecute, query.EndExecute, null)
                .ContinueWith(t => (QueryOperationResponse<Title>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
        }

        private string CreateOrderByString(IList<SortDescription> sortDescriptions)
        {
            var sb = new StringBuilder();

            if (sortDescriptions != null)
            {
                foreach (var sortDescription in sortDescriptions)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(",");
                    }

                    sb.Append(sortDescription.PropertyName + " " +
                              (sortDescription.Direction == ListSortDirection.Ascending ? "asc" : "desc"));
                }
            }

            return sb.ToString();
        }
    }
}
