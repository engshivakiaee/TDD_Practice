using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.DataAccess.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private readonly DeskBookerContext _context;

        public DeskRepository(DeskBookerContext context)
        {
            _context = context;
        }

        public IEnumerable<Desk> GetAll()
        {
           return  _context.Desk.OrderBy(x => x.Id);
        }

        public IEnumerable<Desk> GetAvailableDesks(DateTime dateTime)
        {
            return _context.Desk.Where(d => !d.DeskBookings.Any(db => db.Date.Date == dateTime.Date)).ToList();
        }
    }
}
