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

        public TableStorageClient(CloudStorageAccount storageAccount, string tableName)
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

        public IEnumerable<string> GetMessages()
        {
            var table = GetTable();

            // Define the query, and select only the Email property.
            TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Select(new string[] { "Message" });

            // Define an entity resolver to work with the entity after retrieval.
            EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("Message") ? props["Message"].StringValue : null;

            return table.ExecuteQuery(projectionQuery, resolver, null, null);

        }

        public IEnumerable<T> GetMessagesContaining(string messagePart)
        {
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(
    TableQuery.CombineFilters(
        TableQuery.GenerateFilterCondition("Message", QueryComparisons., "Smith"),
        TableOperators.And,
        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "E")));

            return Enumerable.Empty<T>();
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

            foreach (var partition in items.GroupBy(i => i.PartitionKey))
            {
                var count = 0;

                while (count < partition.Count())
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    foreach (var item in partition.Skip(count).Take(100))
                    {
                        batchOperation.Insert(item);
                        count += 100;
                    }
                    try
                    {
                        table.ExecuteBatch(batchOperation);
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                }



            }


            // Execute the batch operation.
        }

        public void InsertOrUpdate(params T[] items)
        {
            // Create the CloudTable object that represents the "people" table.
            CloudTable table = GetTable();

            // Create the batch operation.

            foreach (var partition in items.GroupBy(i => i.PartitionKey))
            {
                var count = 0;

                while (count < partition.Count())
                {
                    TableBatchOperation batchOperation = new TableBatchOperation();
                    foreach (var item in partition.Skip(count).Take(100))
                    {
                        batchOperation.InsertOrMerge(item);
                        count += 100;
                    }
                    try
                    {
                        table.ExecuteBatch(batchOperation);
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                }
            }
        }

        public void Insert(T item)
        {
            var table = GetTable();
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(item);

            // Execute the insert operation.
            try
            {
                table.Execute(insertOperation);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }

    public class StorageFactory
    {
        private readonly string connectionString;

        public StorageFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public TableStorageClient<T> CreateTableStorageClient<T>(string tableName) where T : ITableEntity, new()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            return new TableStorageClient<T>(storageAccount, tableName);

        }

    }
}