using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AirportModel;

namespace ClientUI
{
    class Client
    {
        string baseUrl = @"https://localhost:44384/trips";
        HttpClient client;

        public Client()
        {
          
            client=new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }


        public async Task<Trip> GetTrip(int id)
        {
            Trip trip = null;
            string path = baseUrl + "/" + id;
            HttpResponseMessage response = await client.GetAsync((path));
            if (response.IsSuccessStatusCode)
            {
                trip = await response.Content.ReadAsAsync<Trip>();
            }

            return trip;

        }

        public async Task<List<Trip>> GetAllAsync()
        {
            List<Trip> all=null;
            HttpResponseMessage response = await client.GetAsync((baseUrl));
            if (response.IsSuccessStatusCode)
            {
                all = await response.Content.ReadAsAsync<List<Trip>>();
            }
            return all;
        }

        public async Task<Uri> CreateTripAsync(Trip trip)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(baseUrl, trip);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;

        }

        public async Task<Uri> ModifyTripAsync(Trip trip)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(baseUrl, trip);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;

        }

        public async Task<HttpStatusCode> DeleteTripAsync(int id)
        {
            var url = baseUrl + "/" + id;
            HttpResponseMessage response = await client.DeleteAsync(url);
            return response.StatusCode;

        }

    }
}
