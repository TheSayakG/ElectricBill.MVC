using CustomerElectricBill.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Data.Repository
{
    public class CustomerRepository
    {
        private readonly IConfiguration _config;
        public CustomerRepository(IConfiguration configuration)
        {
            _config = configuration;
        }
        internal IDbConnection dbConnection
        {
            get
            {
                return new SqlConnection(_config.GetSection("ConnectionStrings:DefaultConnection").Value);
            }
        }
        public IEnumerable<T> DbQuery<T>(string sql, object parameters = null)
        {
            try
            {
                using (IDbConnection dbCon = dbConnection)
                {
                    dbCon.Open();
                    if (parameters == null)
                        return dbCon.Query<T>(sql);
                    return dbCon.Query<T>(sql, parameters);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<dynamic> GetAll()
        {
            var query = "SELECT * FROM customers";
            var result = DbQuery<dynamic>(query);
            return result.Select(x => (IDictionary<string, object>)x).ToList(); ;
        }

        public static async Task<string> UploadToBlob(IFormFile file)
        {
            try
            {
                CloudStorageAccount storageacc = CloudStorageAccount.Parse("**");
                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference("test");
                await blobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                var task = blob.UploadFromStreamAsync(file.OpenReadStream(), file.Length);
                return blob.Uri.ToString();
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        public static async Task DeleteBlob(IFormFile file)
        {
            try
            {
                CloudStorageAccount storageacc = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=wheelerbattery;AccountKey=udujsBjC4dy82H93IbEOc8IkzpZ6kibV6zoTMqkVFJzsQ0bNar5QgTvvKAdYdzDUAsVIH11eJZsgecY4tojk2Q==;EndpointSuffix=core.windows.net");
                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                CloudBlobContainer blobContainer = blobClient.GetContainerReference("test");
                await blobContainer.CreateIfNotExistsAsync();
                var task = blobContainer.GetBlockBlobReference(file.FileName).DeleteIfExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

        }
    }
}
