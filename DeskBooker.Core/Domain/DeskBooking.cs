﻿namespace DeskBooker.Core.Domain
{
    public class DeskBooking : DeskBookingBase
    {
        public int DeskId { get; set; }
        public virtual Desk? Desk { get; set; }
    }
}