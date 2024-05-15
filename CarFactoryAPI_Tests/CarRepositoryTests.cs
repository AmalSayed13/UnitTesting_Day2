using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFactoryAPI_Tests
{
    public class CarRepositoryTests
    {
        // Create Mock of Dependencies
        Mock<FactoryContext> contextMock;

        // use fake object as dependency
        CarRepository carRepository;
        public CarRepositoryTests()
        {
            // Create Mock of Dependencies
            contextMock = new();

            // use fake object as dependency
            carRepository = new(contextMock.Object);
        }
        [Fact]
        public void GetCarById_AskForCarId10_CarObject()
        {
            // Arrange
            // Build the mock data
            List<Car> cars = new List<Car>() { 
                new Car() { Id = 10 },
                new Car() { Id = 20 },
                new Car() { Id = 30 }
            };

            // setup called Dbsets
            contextMock.Setup(o => o.Cars).ReturnsDbSet(cars);

            // Act
            Car result = carRepository.GetCarById(10);

            // Assert
            Assert.NotNull(result);
        }
    }
}
