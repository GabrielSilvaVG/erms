using Eventra.Enums;

namespace Eventra.Models
{
    public class Participant : User
    {
         public Participant()
        {
            UserType = UserType.Participant;
        }
    }
}