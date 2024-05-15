using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using Moq;
using Xunit.Abstractions;

namespace CarFactoryAPI_TestsDay2
{
    public class OwnersServiceTests : IDisposable
    {
        private readonly ITestOutputHelper testOutputHelper;
        // Create Mock Of Dependencies
        Mock<ICarsRepository> carRepoMock;
        Mock<IOwnersRepository> OwnerRepoMock;
        Mock<ICashService> cashMock;

        // use fake object as a dependency
        OwnersService ownersService;

        public OwnersServiceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            // test setup
            testOutputHelper.WriteLine("Test setup");
            // Create Mock Of Dependencies
            carRepoMock = new();
            OwnerRepoMock = new();
            cashMock = new();

            // use fake object as a dependency
            ownersService = new OwnersService(
                carRepoMock.Object, OwnerRepoMock.Object, cashMock.Object);
        }
        public void Dispose()
        {
            testOutputHelper.WriteLine("test clean up");
        }

        [Fact]
        [Trait("Author", "Amal")]
        public void BuyCar_OwnerAlreadyHaveCar_HaveCar()
        {
            testOutputHelper.WriteLine("test 1");

            // Arrange
            var car = new Car() { Id = 10, Price = 1000, OwnerId = 1 };
            var owner = new Owner() { Car = new Car() { Id = 10 } };

            // Setup methods 
            carRepoMock.Setup(repo => repo.GetCarById(10)).Returns(car);
            OwnerRepoMock.Setup(repo => repo.GetOwnerById(1)).Returns(owner);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 10, OwnerId = 1, Amount = 1500 };

            // Act
            var result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Already have car", result);
        }
        [Fact]
        [Trait("Author", "Amal")]
        public void BuyCar_CarPriceMoreThanAmount_InsufficientFunds()
        {
            testOutputHelper.WriteLine("test 2");

            // Arrange
            var car = new Car() { Id = 1, Price = 2000, OwnerId = 2 };
            var owner = new Owner() { Id = 2 };

            // Setup methods 
            carRepoMock.Setup(repo => repo.GetCarById(1)).Returns(car);
            OwnerRepoMock.Setup(repo => repo.GetOwnerById(2)).Returns(owner);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 1, OwnerId = 2, Amount = 1000 };

            // Act
            var result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Insufficient funds", result);
        }
        [Fact]
        [Trait("Author", "Amal")]
        public void BuyCar_IsNotSuccess_SomethingWentWrong()
        {
            testOutputHelper.WriteLine("test 3");

            // Arrange
            var car = new Car() { Id = 1, Price = 2000, OwnerId = 2 };
            var owner = new Owner() { Id = 2 };
            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 1, OwnerId = 2, Amount = 2000 };

            // Setup methods 
            carRepoMock.Setup(repo => repo.GetCarById(1)).Returns(car);
            OwnerRepoMock.Setup(repo => repo.GetOwnerById(2)).Returns(owner);
            cashMock.Setup(cash => cash.Pay(2000)).Returns("Amount: 2000");
            carRepoMock.Setup(repo => repo.AssignToOwner(1, 2)).Returns(false);


            // Act
            var result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Something", result);
        }

        [Fact]
        [Trait("Author", "Amal")]
        public void BuyCar_IsSucess_Successfull()
        {
            testOutputHelper.WriteLine("test 4");

            // Arrange
            var car = new Car() { Id = 1, Price = 2000, OwnerId = 2 };
            var owner = new Owner() { Id = 2 };
            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 1, OwnerId = 2, Amount = 2000 };
            
            // Setup methods 
            carRepoMock.Setup(repo => repo.GetCarById(1)).Returns(car);
            OwnerRepoMock.Setup(repo => repo.GetOwnerById(2)).Returns(owner);
            cashMock.Setup(cash => cash.Pay(2000)).Returns("Amount: 2000");
            carRepoMock.Setup(repo => repo.AssignToOwner(1, 2)).Returns(true);


            // Act
            var result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Successfull", result);
        }


    }
}