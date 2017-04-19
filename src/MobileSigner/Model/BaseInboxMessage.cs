using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model.Inbox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{

    public class BaseInboxMessage: INotifyPropertyChanged
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<Document> Documents { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<InboxMessageAction> Actions { get; set; }
        public bool Archived { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private bool read = false;
        public bool Read
        {
            get
            {
                return read;
            }
            set
            {
                read = value;
                OnPropertyChanged("Read");
            }
        }

        public int AttachmentCount
        {
            get
            {
                return this.Documents.Sum(d => d.AttachmentCount).Value;
            }
        }

        public string LastUpdateStr
        {
            get
            {
                return DateHelper.DateStr(Date);
            }
        }

        public string DateStr
        {
            get
            {
                var dateStr = DateHelper.DateStr(Date);
                return dateStr;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseInboxMessage()
        {
            this.Documents = new List<Document>();
        }
    }

    public class InboxMessageAction
    {
        public string Method { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public InboxMessageAction(InboxAction action)
        {
            this.Method = action.Method;
            this.Name = action.Name;
            this.Url = action.Url;
        }
    }
}
