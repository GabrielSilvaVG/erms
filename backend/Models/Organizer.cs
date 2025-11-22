
using ERMS.Enums;

namespace ERMS.Models
{
    public class Organizer : User
    {
        public Organizer()
        {
            UserType = UserType.Organizer;
        }
    }
}