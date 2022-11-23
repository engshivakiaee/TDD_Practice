using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.DataAccess.Repositories
{
    public class DeskBookingRepository : IDeskBookingRepository
    {
        private readonly DeskBookerContext _context;

        public DeskBookingRepository(DeskBookerContext context)
        {
            _context = context;
        }

        public DeskBookingResult Save(DeskBooking deskBooking)
        {
            _context.Add(deskBooking);
            _context.SaveChanges();
            return new DeskBookingResult()
            {
                Date = deskBooking.Date,
                DeskBookingId = deskBooking.Id,
                FirstName = deskBooking.FirstName,
                LastName = deskBooking.LastName,
                Email = deskBooking.Email,
                Code = DeskBookingResultCode.Success,
            };
        }
    }
}
