using Moq;
using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Services;
using FirstAPI.Models.DTOs.Patients;
using FirstAPI.Misc;

namespace FirstAPI.Test
{
    public class PatientServiceTests
    {
        private Mock<IRepository<int, Patient>> _patientRepoMock;
        private Mock<IRepository<string, User>> _userRepoMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<IMapper> _mapperMock;

        private PatientService _service;

        [SetUp]
        public void Setup()
        {
            _patientRepoMock = new Mock<IRepository<int, Patient>>();
            _userRepoMock = new Mock<IRepository<string, User>>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _patientRepoMock.Object,
                _userRepoMock.Object,
                _encryptionServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task AddPatient_WithValidInput_AddsPatientAndReturnsPatient()
        {
            // Arrange
            var requestDto = new PatientAddRequestDto
            {
                Name = "John Doe",
                Age = 30,
                Password = "securePassword123",
                Email = "johndoe@example.com",
                Address = "123 Street"
            };

            var mappedUser = new User { Username = requestDto.Email };
            var encryptedData = new EncryptModel
            {
                EncryptedData = Encoding.UTF8.GetBytes("encrypted"),
                HashKey = Encoding.UTF8.GetBytes("hashkey")
            };

            var addedUser = new User { Username = requestDto.Email, Role = "Patient" };
            var newPatient = new Patient { Id = 1, Name = "John Doe", UserId = requestDto.Email };

            _mapperMock.Setup(m => m.Map<PatientAddRequestDto, User>(requestDto)).Returns(mappedUser);
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encryptedData);
            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(addedUser);
            _mapperMock.Setup(m => m.Map<PatientAddRequestDto, Patient>(requestDto)).Returns(newPatient);
            _patientRepoMock.Setup(r => r.Add(It.IsAny<Patient>())).ReturnsAsync(newPatient);

            // Act
            var result = await _service.AddPatient(requestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("John Doe"));
            Assert.That(result.UserId, Is.EqualTo(requestDto.Email));
        }

        [Test]
        public void AddPatient_WhenPatientCreationFails_ThrowsException()
        {
            // Arrange
            var requestDto = new PatientAddRequestDto
            {
                Name = "Jane Doe",
                Age = 25,
                Password = "failPassword",
                Email = "janedoe@example.com",
                Address = "456 Lane"
            };

            var mappedUser = new User { Username = requestDto.Email };

            _mapperMock.Setup(m => m.Map<PatientAddRequestDto, User>(requestDto)).Returns(mappedUser);
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                                  .ReturnsAsync(new EncryptModel
                                  {
                                      EncryptedData = Encoding.UTF8.GetBytes("enc"),
                                      HashKey = Encoding.UTF8.GetBytes("hash")
                                  });
            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(mappedUser);
            _mapperMock.Setup(m => m.Map<PatientAddRequestDto, Patient>(requestDto)).Returns(new Patient());
            _patientRepoMock.Setup(r => r.Add(It.IsAny<Patient>())).ReturnsAsync((Patient)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.AddPatient(requestDto));
            Assert.That(ex.Message, Is.EqualTo("Could not add patient"));
        }

        [Test]
        public async Task GetAllPatients_WhenCalled_ReturnsListOfPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, Name = "Alice" },
                new Patient { Id = 2, Name = "Bob" }
            };

            _patientRepoMock.Setup(r => r.GetAll()).ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllPatients();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(p => p.Name == "Alice"));
            Assert.That(result.Any(p => p.Name == "Bob"));
        }

        [Test]
        public void GetAllPatients_WhenRepoThrowsException_ThrowsException()
        {
            // Arrange
            _patientRepoMock.Setup(r => r.GetAll()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.GetAllPatients());
            Assert.That(ex.Message, Is.EqualTo("Error retrieving patients: Database error"));
        }
    }
}
