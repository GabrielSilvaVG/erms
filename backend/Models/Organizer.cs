
using Eventra.Enums;

namespace Eventra.Models
{
    public class Organizer : User
    {
        public Organizer()
        {
            UserType = UserType.Organizer;
        }
    }
}