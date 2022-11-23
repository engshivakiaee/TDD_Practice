using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DeskBooker.Web.Pages
{
    public class DeskBookingModel : PageModel
    {
        private IDeskBookingRequestProcessor _deskBookingRequestProcessor;

        public DeskBookingModel(IDeskBookingRequestProcessor  deskBookingRequestProcessor)
        {
            _deskBookingRequestProcessor = deskBookingRequestProcessor;
        }

        [BindProperty]
        public DeskBookerRequest? DeskBookerRequest { get; set; }

        public DeskBooking? DeskBooking { get; set; }
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime Date { get; set; }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                var result = _deskBookingRequestProcessor.BookDesk(DeskBookerRequest);

                if (result.Code == DeskBookingResultCode.NoDeskAvailable)
                    ModelState.AddModelError("DeskBookerRequest.Date", "No Desk Available in this date");
            }
        }
    }
}
