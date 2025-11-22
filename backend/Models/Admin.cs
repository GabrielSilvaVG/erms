
using ERMS.Enums;

namespace ERMS.Models
{
    public class Admin : User
    {
        public Admin()
        {
            UserType = UserType.Admin;
        }
    }
}