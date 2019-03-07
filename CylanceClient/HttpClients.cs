using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using CylanceClient.Models;
using Newtonsoft.Json;

namespace CylanceClient
{
    public class HttpClients
    {
        public void SendPostRequest(GuidClientModel guidModel)
        {
            using (HttpClient client = new HttpClient())
            {
                var serializer = new DataContractJsonSerializer(typeof(GuidClientModel));
                client.BaseAddress = new Uri("https://localhost:44361/"); //should ideally be read from the config file
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpContent body = new StringContent(JsonConvert.SerializeObject(guidModel));
                    body.Headers.ContentType = new MediaTypeHeaderValue("application/json");


                    HttpResponseMessage response = client.PostAsync("/api/guid", body).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                        Console.WriteLine(result.ToString());
                    else
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        public void SendPutRequest(GuidClientModel guidClientModel, string guid)
        {
            using (HttpClient client = new HttpClient())
            {
                var serializer = new DataContractJsonSerializer(typeof(GuidClientModel));
                client.BaseAddress = new Uri("https://localhost:44361/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpContent body = new StringContent(JsonConvert.SerializeObject(guidClientModel));
                    body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = client.PutAsync("api/Guid/" + guid, body).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(result.ToString());
                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        public void SendDeleteRequest(string guid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44361/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = client.DeleteAsync("api/Guid/" + guid).Result;
                    if (response.IsSuccessStatusCode)
                        Console.WriteLine("Record successfully deleted");
                    else
                        Console.Write("Error - " + response.StatusCode);

                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        public void SendGetRequest(string guid)
        {
            using (HttpClient client = new HttpClient()) 
            {
                var serializer = new DataContractJsonSerializer(typeof(GuidClientModel));
                client.BaseAddress = new Uri("https://localhost:44361/"); 
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var streamTask = client.GetStreamAsync("api/Guid/" + guid);
                    var model = serializer.ReadObject(streamTask.Result) as GuidClientModel;
                    if (streamTask.IsCompletedSuccessfully)
                    {
                        Console.WriteLine("Guid: " + model.Guid + " Expire: " + model.Expire + "  User: " + model.User);
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Error occurred - " + streamTask.Exception.Message);
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}
