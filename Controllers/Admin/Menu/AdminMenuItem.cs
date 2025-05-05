
namespace ITBaza.Controllers.Admin.Menu
{
    public class AdminMenuItem
    {
        public string Title { get; set; } = "";
        public string Controller { get; set; } = "";

        public static List<AdminMenuItem> GetAdminMenu()
        {
            return new List<AdminMenuItem>
            {
                new() { Title = "Керування користувачами", Controller = "SystemUsers" },
                new() { Title = "Ролі користувачів", Controller = "Roles" },
            };
        }
    }
}
