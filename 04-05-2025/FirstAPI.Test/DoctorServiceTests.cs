using Moq;
using AutoMapper;
using FirstAPI.Models;
using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Services;

namespace FirstAPI.Test
{
    public class DoctorServiceTests
    {
        private Mock<IRepository<int, Doctor>> _doctorRepoMock;
        private Mock<IRepository<int, Speciality>> _specialityRepoMock;
        private Mock<IRepository<int, DoctorSpeciality>> _doctorSpecialityRepoMock;
        private Mock<IRepository<string, User>> _userRepoMock;
        private Mock<IOtherContextFunctionities> _otherContextMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<IMapper> _mapperMock;

        private DoctorService _service;

        [SetUp]
        public void Setup()
        {
            _doctorRepoMock = new Mock<IRepository<int, Doctor>>();
            _specialityRepoMock = new Mock<IRepository<int, Speciality>>();
            _doctorSpecialityRepoMock = new Mock<IRepository<int, DoctorSpeciality>>();
            _userRepoMock = new Mock<IRepository<string, User>>();
            _otherContextMock = new Mock<IOtherContextFunctionities>();
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(
                _doctorRepoMock.Object,
                _specialityRepoMock.Object,
                _doctorSpecialityRepoMock.Object,
                _userRepoMock.Object,
                _otherContextMock.Object,
                _encryptionServiceMock.Object,
                _mapperMock.Object
            );
        }

        [Test]
        public async Task AddDoctor_WithValidInput_AddsDoctorAndReturnsDoctor()
        {
            // Arrange
            var requestDto = new DoctorAddRequestDto
            {
                Name = "Dr. House",
                Password = "pass123",
                YearsOfExperience = 10,
                Specialities = new List<SpecialityAddRequestDto>
                {
                    new SpecialityAddRequestDto { Name = "Diagnostics" }
                }
            };

            var mappedUser = new User { Username = "drhouse@example.com", Role = "Doctor" };
            var encryptedData = new EncryptModel
            {
                EncryptedData = System.Text.Encoding.UTF8.GetBytes("encrypted"),
                HashKey = System.Text.Encoding.UTF8.GetBytes("key")
            };

            var addedUser = new User { Username = "drhouse@example.com", Role = "Doctor" };

            var doctor = new Doctor { Id = 1, Name = "Dr. House" };
            var speciality = new Speciality { Id = 2, Name = "Diagnostics" };
            var doctorSpeciality = new DoctorSpeciality { DoctorId = 1, SpecialityId = 2 };

            _mapperMock.Setup(m => m.Map<DoctorAddRequestDto, User>(requestDto)).Returns(mappedUser);
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>())).ReturnsAsync(encryptedData);
            _userRepoMock.Setup(u => u.Add(It.IsAny<User>())).ReturnsAsync(addedUser);
            _doctorRepoMock.Setup(d => d.Add(It.IsAny<Doctor>())).ReturnsAsync(doctor);
            _specialityRepoMock.Setup(s => s.GetAll()).ReturnsAsync(new List<Speciality>());
            _specialityRepoMock.Setup(s => s.Add(It.IsAny<Speciality>())).ReturnsAsync(speciality);
            _doctorSpecialityRepoMock.Setup(ds => ds.Add(It.IsAny<DoctorSpeciality>())).ReturnsAsync(doctorSpeciality);

            // Act
            var result = await _service.AddDoctor(requestDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("Dr. House"));
        }

        [Test]
        public void AddDoctor_WhenDoctorCreationFails_ThrowsException()
        {
            // Arrange
            var requestDto = new DoctorAddRequestDto
            {
                Name = "Dr. Fails",
                Password = "failpass",
                Specialities = new List<SpecialityAddRequestDto>()
            };

            _mapperMock.Setup(m => m.Map<DoctorAddRequestDto, User>(requestDto)).Returns(new User());
            _encryptionServiceMock.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
                                  .ReturnsAsync(new EncryptModel
                                  {
                                      EncryptedData = System.Text.Encoding.UTF8.GetBytes("enc"),
                                      HashKey = System.Text.Encoding.UTF8.GetBytes("hash")
                                  });

            _userRepoMock.Setup(u => u.Add(It.IsAny<User>())).ReturnsAsync(new User());
            _doctorRepoMock.Setup(d => d.Add(It.IsAny<Doctor>())).ReturnsAsync((Doctor)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _service.AddDoctor(requestDto));
            Assert.That(ex.Message, Is.EqualTo("Could not add doctor"));
        }
    }
}
