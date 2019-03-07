using CylanceClient.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace CylanceClient
{
    class Program
    {
        public static GuidClientModel _guidClientModel = new GuidClientModel();
        public static string _guid = string.Empty;
        static void Main(string[] args)
        {
            string crudOperation = GetCrudOperation();

            PopulateGuidModel(crudOperation);
            
            switch (crudOperation.ToUpper())
            {
                case "GET":
                    PerformGetOperation();
                    break;

                case "PUT":
                    PerformPutOperation();
                    break;

                case "POST":
                    PerformPostOperation();
                    break;

                case "DELETE":
                    PerformDeleteOperation();
                    break;
            }
        }

        private static string GetCrudOperation()
        {
            Console.WriteLine("Enter CRUD operation (Get, Put, Post or Delete)");
            string crudOperation = Console.ReadLine();
            string[] validCrudOperations = { "GET", "PUT", "POST", "DELETE" };
            if (!validCrudOperations.Contains(crudOperation.ToUpper()))
                throw new Exception("Invalid operation. Press any key to exit application.");

            return crudOperation;
        }

        private static void PopulateGuidModel(string crudOperation)
        {
            Console.WriteLine("Enter the Guid: ");
            _guid = Console.ReadLine();
            if (!IsGuidValid(_guid, crudOperation))
                throw new Exception("Invalid Guid value entered. Press any key to exit application.");

            if (crudOperation.ToUpper().Equals("GET") || crudOperation.ToUpper().Equals("DELETE"))
                return;

            if (!string.IsNullOrEmpty(_guid) && !crudOperation.ToUpper().Equals("PUT"))
                _guidClientModel.Guid = GetGuid(_guid);
            
            Console.WriteLine("Enter Unix time (Press enter to skip this field/Send default value): ");
            string unixTime = Console.ReadLine();

            if (!string.IsNullOrEmpty(unixTime))
            {
                if (int.TryParse(unixTime, out int outTime))
                    _guidClientModel.Expire = outTime;
                else
                    throw new Exception("Invalid time entered. Press any key to exit.");
            }

            Console.WriteLine("Enter user information (Press enter to skip this field): ");
            _guidClientModel.User = Console.ReadLine();
        }

        private static void PerformDeleteOperation()
        {
            HttpClients httpClient = new HttpClients();
            httpClient.SendDeleteRequest(_guid);
        }

        private static void PerformPostOperation()
        {
            HttpClients httpClient = new HttpClients();
            httpClient.SendPostRequest(_guidClientModel);
        }

        private static void PerformPutOperation()
        {
            HttpClients httpClients = new HttpClients();
            httpClients.SendPutRequest(_guidClientModel, _guid);
        }

        public static bool IsGuidValid(string guid, string crudOpertion)
        {
            if (crudOpertion.ToUpper().Equals("POST") && string.IsNullOrEmpty(guid))
                return true;
            return Guid.TryParse(guid, out Guid guidOutput);
        }

        public static Guid GetGuid(string guid)
        {
            Guid.TryParse(guid.ToUpper(), out Guid guidOutput);
            return guidOutput;
        }
        private static void PerformGetOperation()
        {
            HttpClients httpClients = new HttpClients();
            httpClients.SendGetRequest(_guid);
        }
    }
}
