using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using WebRole1.DAL;
using WebRole1.Models;
using Newtonsoft.Json;

namespace WorkerRole1
{
    
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private CloudQueue orderqueue;
        private CloudTable ordertable;
        public BKCCloudContext db = new BKCCloudContext();

        public override void Run ()
        {
            Trace.TraceInformation("WorkerRole1 is running properly");
            
            while(true)
            {
                try
                {
                    CloudQueueMessage msg = null;
                     msg = this.orderqueue.GetMessage();

                    if(msg != null)
                    {
                        //Order order = db.Orders.Find(Int32.Parse(msg.AsString));

                        Order order = JsonConvert.DeserializeObject<Order>(msg.AsString);

                        OrderHistory orderHistory = new OrderHistory(order.CustomerId.ToString(),order.Id.ToString());
                        orderHistory.OrderDate = order.OrderDate;
                        TableOperation insertOperation = TableOperation.Insert(orderHistory);

                        ordertable.Execute(insertOperation);


                        TableQuery<OrderHistory> query = new TableQuery<OrderHistory>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"));
                        foreach (OrderHistory orderHist in ordertable.ExecuteQuery(query))
                        {
                            Console.WriteLine("{0}, {1}\t{2}", orderHist.PartitionKey, orderHist.RowKey,
                                orderHist.OrderDate);
                        }

                        orderqueue.Delete();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch(StorageException ex)
                {
                    Trace.TraceInformation("Storage exception occured " + ex.Message);
                }
            }
            


            //try
            //{
            //    this.RunAsync(this.cancellationTokenSource.Token).Wait();
            //}
            //finally
            //{
            //    this.runCompleteEvent.Set();
            //}
        }

        public override bool OnStart ()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

            //Queue client 
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            //Get Queue reference
            orderqueue = queueClient.GetQueueReference("ordersq");

            orderqueue.DeleteIfExists();

            orderqueue.CreateIfNotExists();

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable ordertable = tableClient.GetTableReference("orderhistory");

            ordertable.CreateIfNotExists();

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop ()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }

        private async Task RunAsync ( CancellationToken cancellationToken )
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
