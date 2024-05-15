using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;


namespace CarFactoryAPI_TestsDay2
{
    public class OwnerRepositoryTests
    {
        Mock<FactoryContext> contextMock;
        OwnerRepository ownerRepository;
        public OwnerRepositoryTests()
        {
            // Create Mock of Dependencies
            contextMock = new();
            ownerRepository = new(contextMock.Object);
        }

        [Fact]
        [Trait("Author", "Amal")]
        public void AddOwner_AskForAllOwners_EmptyOwnerList()
        {
            // Arrange
            // Build the mock data
            List<Owner> owners = new List<Owner>() {};

            // setup called Dbsets
            contextMock.Setup(o => o.Owners).ReturnsDbSet(owners);

            // Act
            List<Owner> owners1 = ownerRepository.GetAllOwners();

            // Assert
            Assert.Empty(owners1);
        }

        [Fact]
        [Trait("Author", "Amal")]
        public void AddOwner_AskForOwnerObject_NewOwner()
        {
            // Arrange
            //Build the mock data

            Owner owner1 = new Owner() { Id = 1, Name = "Amal", Car = new Car { Id = 10 } };
            List<Owner> owners = new List<Owner>();

            // setup called DBSets
            contextMock.Setup(o => o.Owners).ReturnsDbSet(owners);

            // Act
            bool result = ownerRepository.AddOwner(owner1);

            // Assert
            Assert.True(result);
        }
    }
}
