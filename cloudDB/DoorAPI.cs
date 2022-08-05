using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace cloudDB
{
    public class DoorAPI
    {
        /// <summary>
        /// HTTP access constant to toggle 
        /// between local and cloud server.
        /// </summary>
        public static bool useCloudServer = false;

        /// <summary>
        /// Base url for local testing and cloud url for production
        /// </summary>
        const string baseUrlLocal = "http://localhost:8080/";
        const string baseUrlCloud = "https://mongorevit.herokuapp.com/";

        /// <summary>
        /// Base url for local testing and cloud url for production
        /// </summary>
        public static string RestAPIBaseUrl
        {
            get { return useCloudServer ? baseUrlCloud : baseUrlLocal; }
        }

        /// <summary>
        /// GET JSON data from 
        /// the specified mongoDB collection.
        /// </summary>
        public static List<Door> Get(string collectionName)
        {
            RestClient client = new RestClient(RestAPIBaseUrl);
            RestRequest request = new RestRequest("/api" + "/" + collectionName, Method.GET);

            IRestResponse<List<Door>> response = client.Execute<List<Door>>(request);

            return response.Data;
        }

        /// <summary>
        /// Batch POST JSON document data into 
        /// the specified mongoDB collection.
        /// </summary>
        public static HttpStatusCode PostBatch(out string content, out string errorMessage,
            string collectionName, List<Door> doorData)
        {
            RestClient client = new RestClient(RestAPIBaseUrl);
            RestRequest request = new RestRequest("/api" + "/" + collectionName + "/" + "batch", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(doorData);

            IRestResponse response = client.Execute(request);
            content = response.Content;
            errorMessage = response.ErrorMessage;

            return response.StatusCode;
        }

    }
}
