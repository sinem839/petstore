using System.Net;
using Moq;
using PetStore.Api;
using PetStore.Api.Models;
using RestSharp;

namespace YourUnitTestProjectNamespace
{

    [TestFixture]
    public class ApiWrapperTests
    {
        private Mock<IRestClient> _mockRestClient;
        private ApiWrapper _apiWrapper;

        [SetUp]
        public void Setup()
        {
            _mockRestClient = new Mock<IRestClient>();
            _apiWrapper = new ApiWrapper(_mockRestClient.Object);
        }

        [Test]
        public void FindPetsByStatus_ValidStatus_ReturnsPets()
        {
            // Arrange
            var expectedPets = new List<Pet> { new Pet { Id = 1, Name = "Cat", Status = "available" } };
            _mockRestClient.Setup(c => c.Execute<List<Pet>>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<List<Pet>> { StatusCode = HttpStatusCode.OK, Data = expectedPets });

            // Act
            List<Pet> actualPets = _apiWrapper.FindPetsByStatus("available");

            // Assert
            Assert.NotNull(actualPets);
            Assert.AreEqual(expectedPets, actualPets);
        }

        [Test]
        public void GetPetById_ValidId_ReturnsPet()
        {
            // Arrange
            var expectedPet = new Pet { Id = 1, Name = "Cat", Status = "available" };
            _mockRestClient.Setup(c => c.Execute<Pet>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<Pet> { StatusCode = HttpStatusCode.OK, Data = expectedPet });

            // Act
            Pet actualPet = _apiWrapper.GetPetById(1);

            // Assert
            Assert.NotNull(actualPet);
            Assert.AreEqual(expectedPet, actualPet);
        }

        [Test]
        public void GetPetById_InvalidId_ReturnsNull()
        {
            // Arrange
            _mockRestClient.Setup(c => c.Execute<Pet>(It.IsAny<IRestRequest>()))
                .Returns(new RestResponse<Pet> { StatusCode = HttpStatusCode.NotFound });

            // Act
            Pet actualPet = _apiWrapper.GetPetById(999);

            // Assert
            Assert.Null(actualPet);
        }

    }
}
