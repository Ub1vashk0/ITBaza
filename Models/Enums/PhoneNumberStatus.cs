using System.Runtime.Serialization;

namespace ITBaza.Models.Enums
{
    public enum PhoneNumberStatus
    {
        [EnumMember(Value = "Активний")]
        Active,

        [EnumMember(Value = "Призупинений")]
        Suspended
    }
}
