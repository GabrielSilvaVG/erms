
using Eventra.Enums;

namespace Eventra.Models
{
    public class Admin : User
    {
        public Admin()
        {
            UserType = UserType.Admin;
        }
    }
}