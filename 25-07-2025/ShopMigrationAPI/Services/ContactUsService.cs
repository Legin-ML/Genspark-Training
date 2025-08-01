using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Models;

namespace ShopMigrationAPI.Services
{
    public class ContactUsService
    {
        private readonly IRepository<Contactu> _contactusRepository;

        public ContactUsService(IRepository<Contactu> contactusRepository)
        {
            _contactusRepository = contactusRepository;
        }

        public IEnumerable<Contactu> GetAllContacts()
        {
            return _contactusRepository.GetAll();
        }

        public Contactu GetContactById(int id)
        {
            var contact = _contactusRepository.GetById(id);
            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found.");
            }
            return contact;
        }

        public void CreateContact(Contactu contact)
        {
            var existingContact = _contactusRepository.GetAll().FirstOrDefault(c => c.Email == contact.Email || c.Phone == contact.Phone);
            if (existingContact != null)
            {
                throw new InvalidOperationException("A contact with the same email or phone number already exists.");
            }

            _contactusRepository.Add(contact);
            _contactusRepository.Save();
        }

        public void UpdateContact(Contactu contact)
        {
            var existingContact = _contactusRepository.GetById(contact.Id);
            if (existingContact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {contact.Id} not found.");
            }

            _contactusRepository.Update(contact);
            _contactusRepository.Save();
        }

        public void DeleteContact(int id)
        {
            var contact = _contactusRepository.GetById(id);
            if (contact == null)
            {
                throw new KeyNotFoundException($"Contact with ID {id} not found.");
            }

            _contactusRepository.Delete(id);
            _contactusRepository.Save();
        }
    }
}
