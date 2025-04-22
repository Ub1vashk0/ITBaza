using Microsoft.AspNetCore.Mvc;
using ITBaza.Models;

public class AccountingController : Controller
{
    public IActionResult Index()
    { 
        return View();
    }
}
