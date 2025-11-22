using ERMS.Enums;

namespace ERMS.Models
{
    public class Participant : User
    {
         public Participant()
        {
            UserType = UserType.Participant;
        }
    }
}