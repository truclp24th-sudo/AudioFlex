using SpicyKorea.Data;
using SpicyKorea.Models;
using SpicyKorea.Repositories.Interfaces;

namespace SpicyKorea.Repositories.Implementations
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
