using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class OrderHistory : TableEntity
    {
        public OrderHistory (string CustomerId,string OrderId)
        {
            this.PartitionKey = CustomerId;
            this.RowKey = OrderId;
        }

        public OrderHistory ()
        {

        }

        public DateTime OrderDate { get; set; }
        
    }

}
