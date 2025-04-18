using Microsoft.AspNetCore.Mvc;

namespace ITBaza.Controllers;

public class DictonaryController : Controller
{
    public IActionResult Index()
    {
        return View(GetDictionaryMenu());
    }
    public static List<(string Title, string Controller)> GetDictionaryMenu()
    {
        return new List<(string, string)>
        {
            ("Країни", "Countries"),
            ("Розміщення", "Placements"),
            ("Департаменти", "Departments"),
            ("Відділи", "Divisions"),
            ("Вакансії", "Vacations"),
            ("Оператори", "MobileOperators"),
            ("Типи ресурсів", "ResourceTypes"),
            ("Ролі ресурсів", "ResourceRoles"),
            ("Ресурси", "Resources"),
        };
    }
}