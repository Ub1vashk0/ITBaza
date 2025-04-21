using ITBaza.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITBaza.Controllers;

public class DictonaryController : Controller
{
    public IActionResult Index()
    {
        var menu = DictionaryMenuItem.GetDictionaryMenu();
        return View(menu);
    }
    

}

