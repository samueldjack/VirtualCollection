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
using VirtualCollection.Northwind;
using VirtualCollection.VirtualCollection;

namespace VirtualCollection.Demo
{
    public class NorthwindProductsSource : VirtualCollectionSource<Product>
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

        protected override Task<IList<Product>> GetPageAsyncOverride(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            return GetQueryResults(start, pageSize, sortDescriptions)
                .ContinueWith(t =>
                    {
                        SetCount((int)t.Result.TotalCount);
                        return (IList<Product>)((IEnumerable<Product>)t.Result).ToList();

                    }, TaskContinuationOptions.ExecuteSynchronously);
        }

        private Task<QueryOperationResponse<Product>> GetQueryResults(int start, int pageSize, IList<SortDescription> sortDescriptions)
        {
            var context = new NorthwindEntities(new Uri("http://services.odata.org/Northwind/Northwind.svc/"));

            var orderByString = CreateOrderByString(sortDescriptions);
            var query = context.Products
                .AddQueryOption("$skip", start)
                .AddQueryOption("$top", pageSize)
                .IncludeTotalCount();

            if (!string.IsNullOrEmpty(Search))
            {
                query = query.AddQueryOption("$filter", "(substringof('" + Search + "',ProductName) eq true)");
            }

            if (orderByString.Length > 0)
            {
                query = query.AddQueryOption("$orderby", orderByString);
            }

            return Task.Factory.FromAsync<IEnumerable<Product>>(query.BeginExecute, query.EndExecute, null)
                .ContinueWith(t => (QueryOperationResponse<Product>)t.Result, TaskContinuationOptions.ExecuteSynchronously);
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
