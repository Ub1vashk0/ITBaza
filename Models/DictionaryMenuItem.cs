namespace ITBaza.Models
{
    public class DictionaryMenuItem
    {
        public string Title { get; set; } = "";
        public string Controller { get; set; } = "";

        public static List<DictionaryMenuItem> GetDictionaryMenu()
        {
            return new List<DictionaryMenuItem>
    {
        new() { Title = "Країни", Controller = "Countries" },
        new() { Title = "Розміщення", Controller = "Placements" },
        new() { Title = "Департаменти", Controller = "Departments" },
        new() { Title = "Відділи", Controller = "Divisions"},
        new() { Title = "Вакансії", Controller = "Vacations" },
        new() { Title = "Оператори", Controller = "MobileOperators" },
        new() { Title = "Типи ресурсів", Controller = "ResourceTypes"},
        new() { Title = "Ресурс", Controller = "Resources"},
        new() { Title = "Ролі користувача ресурсу", Controller = "ResourceRoles"}
    };
        }
    }
}
