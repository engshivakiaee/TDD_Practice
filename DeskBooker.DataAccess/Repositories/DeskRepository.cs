﻿using DeskBooker.Core.DataInterface;
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
            return _context.Desk.OrderBy(x => x.Id);
        }

        public IEnumerable<Desk> GetAvailableDesks(DateTime date)
        {
            var bookedDeskIds = _context.DeskBooking.
                 Where(x => x.Date == date)
                 .Select(b => b.DeskId)
                 .ToList();

            return _context.Desk
              .Where(x => !bookedDeskIds.Contains(x.Id))
              .ToList();
        }
    }
}
