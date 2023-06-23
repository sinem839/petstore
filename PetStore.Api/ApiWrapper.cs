using PetStore.Api.Models;
using RestSharp;
using System.Net;

namespace PetStore.Api
{
    public class ApiWrapper
    {
        private readonly IRestClient _client;

        public ApiWrapper(IRestClient client)
        {
            _client = client;
        }

        public List<Pet> FindPetsByStatus(string status)
        {
            var request = new RestRequest("pet/findByStatus", Method.GET);
            request.AddParameter("status", status);
            IRestResponse<List<Pet>> response = _client.Execute<List<Pet>>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            return null;
        }

        public Pet GetPetById(int id)
        {
            var request = new RestRequest("pet/{id}", Method.GET);
            request.AddUrlSegment("id", id.ToString());
            IRestResponse<Pet> response = _client.Execute<Pet>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            return null;
        }

    }
}