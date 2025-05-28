using HospitalManagement.Interfaces;
using HospitalManagement.Models;
using HospitalManagement.Models.Dtos;

namespace HospitalManagement.Services
{
    public class DoctorService : IDoctor
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(
            IRepository<int, Doctor> doctorRepository,
            IRepository<int, Speciality> specialityRepository,
            IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

        public async Task<DoctorDto> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            if (doctorDto == null || string.IsNullOrWhiteSpace(doctorDto.Name))
                throw new ArgumentException("Doctor information is invalid.");

            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience
            };

            var addedDoctor = await _doctorRepository.Add(doctor);

            if (doctorDto.Specialities != null && doctorDto.Specialities.Any())
            {
                IEnumerable<Speciality> allSpecialities;
                try
                {
                    allSpecialities = await _specialityRepository.GetAll();
                }
                catch
                {
                    allSpecialities = new List<Speciality>();
                }

                foreach (var specialityDto in doctorDto.Specialities)
                {
                    var existingSpeciality = allSpecialities
                        .FirstOrDefault(s => s.Name.Equals(specialityDto.Name, StringComparison.OrdinalIgnoreCase));

                    Speciality speciality;

                    if (existingSpeciality != null)
                    {
                        speciality = existingSpeciality;
                    }
                    else
                    {
                        speciality = new Speciality
                        {
                            Name = specialityDto.Name,
                            Status = "Active"
                        };

                        try
                        {
                            speciality = await _specialityRepository.Add(speciality);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to add speciality");
                        }
                    }

                    var doctorSpeciality = new DoctorSpeciality
                    {
                        DoctorId = addedDoctor.Id,
                        SpecialityId = speciality.Id
                    };

                    try
                    {
                        await _doctorSpecialityRepository.Add(doctorSpeciality);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to map specialities");
                    }
                }
            }
            
            return await GetDoctorDtoById(addedDoctor.Id);
        }

        public async Task<DoctorDto> GetDoctorByName(string name)
        {
            IEnumerable<Doctor> allDoctors;
            try
            {
                allDoctors = await _doctorRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not retrieve doctors: {ex.Message}");
            }

            var doctor = allDoctors
                .FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (doctor == null)
                throw new Exception($"Doctor with name '{name}' not found.");

            return await JoinSpecialities(doctor);
        }

        public async Task<ICollection<DoctorDto>> GetDoctorsBySpeciality(string specialityName)
        {
            IEnumerable<Speciality> allSpecialities;
            try
            {
                allSpecialities = await _specialityRepository.GetAll();
            }
            catch
            {
                allSpecialities = new List<Speciality>();
            }

            var speciality = allSpecialities
                .FirstOrDefault(s => s.Name.Equals(specialityName, StringComparison.OrdinalIgnoreCase));

            if (speciality == null)
                throw new Exception($"Speciality '{specialityName}' not found.");

            IEnumerable<DoctorSpeciality> allDoctorSpecialities;
            try
            {
                allDoctorSpecialities = await _doctorSpecialityRepository.GetAll();
            }
            catch
            {
                allDoctorSpecialities = new List<DoctorSpeciality>();
            }

            var doctorIds = allDoctorSpecialities
                .Where(ds => ds.SpecialityId == speciality.Id)
                .Select(ds => ds.DoctorId)
                .Distinct()
                .ToList();

            IEnumerable<Doctor> allDoctors;
            try
            {
                allDoctors = await _doctorRepository.GetAll();
            }
            catch
            {
                allDoctors = new List<Doctor>();
            }

            var doctorsBySpeciality = allDoctors
                .Where(d => doctorIds.Contains(d.Id))
                .ToList();
            
            var doctorDtos = new List<DoctorDto>();
            foreach (var doctor in doctorsBySpeciality)
            {
                doctorDtos.Add(await JoinSpecialities(doctor));
            }

            return doctorDtos;
        }
        
        private async Task<DoctorDto> JoinSpecialities(Doctor doctor)
        {
            IEnumerable<DoctorSpeciality> allDoctorSpecialities;
            IEnumerable<Speciality> allSpecialities;

            try
            {
                allDoctorSpecialities = await _doctorSpecialityRepository.GetAll();
                allSpecialities = await _specialityRepository.GetAll();
            }
            catch
            {
                allDoctorSpecialities = new List<DoctorSpeciality>();
                allSpecialities = new List<Speciality>();
            }

            var specialityIds = allDoctorSpecialities
                .Where(ds => ds.DoctorId == doctor.Id)
                .Select(ds => ds.SpecialityId);

            var specialities = allSpecialities
                .Where(s => specialityIds.Contains(s.Id))
                .Select(s => s.Name)
                .ToList();

            return new DoctorDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                YearsOfExperience = doctor.YearsOfExperience,
                Specialities = specialities
            };
        }

        private async Task<DoctorDto> GetDoctorDtoById(int doctorId)
        {
            Doctor doctor;
            try
            {
                var allDoctors = await _doctorRepository.GetAll();
                doctor = allDoctors.FirstOrDefault(d => d.Id == doctorId);
            }
            catch
            {
                doctor = null;
            }

            if (doctor == null)
                throw new Exception($"Doctor with id '{doctorId}' not found.");

            return await JoinSpecialities(doctor);
        }
    }
}
