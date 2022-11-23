
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterface
{
    public interface IDeskBookingRepository
    {
        public DeskBookingResult Save(DeskBooking deskBooking);
    }
}
