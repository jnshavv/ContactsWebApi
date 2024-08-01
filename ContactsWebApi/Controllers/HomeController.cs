using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;



namespace ContactsWebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }


public class Example
    {
        public async Task<string> GetDataAsync()
        {
            await Task.Delay(1000); // Simulate an async operation
            return "Data";
        }
    }

}
