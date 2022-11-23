using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeskBooker.Web.Pages
{
    public class DeskModel : PageModel
    {
        private readonly IDeskRepository _deskRepository;

        public DeskModel(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
        }

        public int Id { get; set; }
        public string? Description { get; set; }

        public List<Desk> AllDesks { get; set; }

        public void OnGet()
        {
            AllDesks = _deskRepository.GetAll().Where(x=>x.Id==1).ToList();
        }
    }
}
