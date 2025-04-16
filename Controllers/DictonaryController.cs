using Microsoft.AspNetCore.Mvc;

namespace ITBaza.Controllers;

public class DictonaryController : Controller
{
    public IActionResult Index()
    {
        // Тут можна оновити список, або зробити автоматичне сканування
        var directoryItems = new List<(string Title, string Controller)>
        {
            ("Країни", "Countries"),
            ("Розміщення", "Placements"),
            ("Департаменти", "Departments"),
            ("Відділи", "Divisions"),
            ("Вакансії", "Vacations"),
            ("Оператори мобільного", "MobileOperators"),
            ("Типи ресурсів", "ResourceTypes"),
            ("Ролі ресурсів", "ResourceRoles"),
            ("Ресурси", "Resources")
        };

        return View(directoryItems);
    }
}