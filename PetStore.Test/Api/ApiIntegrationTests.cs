using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using PetStore.Api.Models;
using RestSharp;

namespace PetStore.Test.Api
{
    [TestFixture]
    public class ApiIntegrationTests
    {
        private const string BaseUrl = "https://petstore.swagger.io/v2";

        [Test]
        public void FindPetsByStatus_ValidStatus_ReturnsPets()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet/findByStatus", Method.GET);
            request.AddParameter("status", "available");

            // Act
            IRestResponse<List<Pet>> response = client.Execute<List<Pet>>(request);
            List<Pet> pets = JsonConvert.DeserializeObject<List<Pet>>(response.Content);

            // Assert
            Assert.NotNull(pets);
            Assert.IsNotEmpty(pets);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }

        [Test]
        public void GetPetById_ValidId_ReturnsPet()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet/{id}", Method.GET);
            request.AddUrlSegment("id", "2");

            // Act
            IRestResponse<Pet> response = client.Execute<Pet>(request);
            Pet pet = response.Data;

            // Assert
            Assert.NotNull(pet);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(2, pet.Id);
            Assert.AreEqual("cat", pet.Name);
            Assert.AreEqual("available", pet.Status);
        }

        [Test]
        public void AddNewPet_ValidPet_ReturnsCreatedPet()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet", Method.POST);
            var newPet = new Pet { Id = 9223372036854775807, Name = "NewPet", Status = "available" };
            request.AddJsonBody(newPet);

            // Act
            IRestResponse<Pet> response = client.Execute<Pet>(request);
            Pet createdPet = response.Data;

            // Assert
            Assert.NotNull(createdPet);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.AreEqual(newPet.Id, createdPet.Id);
        }

        [Test]
        public void UpdatePet_ValidPet_ReturnsUpdatedPet()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet", Method.PUT);
            var updatedPet = new Pet { Id = 9223372036854775807, Name = "UpdatedCat", Status = "available" };
            request.AddJsonBody(updatedPet);

            // Act
            IRestResponse<Pet> response = client.Execute<Pet>(request);
            Pet updatedPetResponse = response.Data;

            // Assert
            Assert.NotNull(updatedPetResponse);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(updatedPet.Id, updatedPetResponse.Id);
        }

        [Test]
        public void DeletePet_ValidId_ReturnsSuccess()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet/{id}", Method.DELETE);
            request.AddUrlSegment("id", "1");

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void DeletePet_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("pet/{id}", Method.DELETE);
            request.AddUrlSegment("id", "999");

            // Act
            IRestResponse response = client.Execute(request);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
