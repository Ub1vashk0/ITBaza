using Microsoft.AspNetCore.Mvc;
using ITBaza.Models;

public class AccountingController : Controller
{
    public IActionResult Index()
    {
        ViewBag.AccountingMenu = AccountingMenuItem.GetMenu();
        return View();
    }
}
