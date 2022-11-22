using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Module_9_ResponsivePages.Models;

namespace Module_9_ResponsivePages.Pages
{
    public class AjaxPartialModel : PageModel
    {
        private ICarService _carService;
        public AjaxPartialModel(ICarService carService)
        {
            _carService = carService;
        }
        public List<Car> Cars { get; set; }
        public void OnGet()
        {
        }
        //burada g�stermek istedi�imiz partial pagemizi d�nd�r�yoruz.bu pageye servicemizden ald���m�z araba listesinide ekliyoruz
        public PartialViewResult OnGetCarPartial()
        {
            Cars = _carService.GetAll();
            return Partial("CarPartial", Cars);
        }
    }
}
