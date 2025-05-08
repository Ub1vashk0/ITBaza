using ITBaza.Controllers.ReportingControlers;

namespace ITBaza.Controllers.ReportingControlers
{
    public class ReportingItemMenu
    {
        public string Title { get; set; } = "";
        public string Controller { get; set; } = "";

        public static List<ReportingItemMenu> GetReportingMenu()
        {
            return new List<ReportingItemMenu>
            {
                new() { Title = "Звіт по працівнику", Controller = "ReportPerson" },
                new() { Title = "Звіт по доступу", Controller = "ReportAccess" },
                new() { Title = "Звіт по номеру", Controller = "ReportPhoneNumber" },
            };
        }
    }
}
