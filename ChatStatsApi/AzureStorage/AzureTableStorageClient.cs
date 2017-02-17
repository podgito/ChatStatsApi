using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pojito.Azure.Storage.Table
{
    public class TableStorageClient<T> where T : ITableEntity, new()
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly string tableName;

        internal TableStorageClient(CloudStorageAccount storageAccount, string tableName)
        {
            this.storageAccount = storageAccount;
            this.tableName = tableName;
        }

        private CloudTable GetTable()
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }

        public IEnumerable<T> Get(string partitionKey)
        {
            var table = GetTable();
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Print the fields for each customer.

            return table.ExecuteQuery<T>(query);
        }

        public T Get(string partitionKey, string rowKey)
        {
            var table = GetTable();
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            return (T)retrievedResult.Result;
        }

        public void Insert(params T[] items)
        {
            // Create the CloudTable object that represents the "people" table.
            CloudTable table = GetTable();

            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            var count = 0;
            foreach (var item in items.Skip(count).Take(100))
            {
                batchOperation.Insert(item);

                count += 100;
            }

            // Execute the batch operation.
            table.ExecuteBatch(batchOperation);
        }

        public void Insert(T item)
        {
            var table = GetTable();
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(item);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }
    }
}