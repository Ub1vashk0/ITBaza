namespace ITBaza.Models
{
    public class AccountingMenuItem
    {
        public string Title { get; set; } = "";
        public string Controller { get; set; } = "";

        public static List<AccountingMenuItem> GetMenu()
        {
            return new List<AccountingMenuItem>
            {
                new AccountingMenuItem { Title = "Люди", Controller = "People" },
                new AccountingMenuItem { Title = "Доступи", Controller = "Accesses" },
                new AccountingMenuItem { Title = "Операції доступів", Controller = "AccessOperations" },
                new AccountingMenuItem { Title = "Номери", Controller = "PhoneNumbers" },
                new AccountingMenuItem { Title = "Операції номерів", Controller = "PhoneNumberOperations" }
            };
        }
    }
}
