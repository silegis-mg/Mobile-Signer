using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    public delegate Task RefreshMessagesCallback();

    public class MessageCollection<T> : List<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public bool MoreAvailable { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int NotReadCount { get; set; }

        public MessageCollection(int pageSize)
        {
            PageSize = pageSize;
            CurrentPage = 0;
            MoreAvailable = true;
        }

        public void Update(List<T> requests, int notReadCount, int page, bool clear)
        {            
            if(clear)
                this.Clear();

            this.CurrentPage = page;

            if(requests.Count < PageSize)
            {
                MoreAvailable = false;
            }
            else
            {
                MoreAvailable = true;
            }

            foreach (var request in requests)
            {
                this.Add(request);
            }

            this.NotReadCount = notReadCount;

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
