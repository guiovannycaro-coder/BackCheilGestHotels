using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.application.iservices
{
    public interface IUsersSeguridadService
    {
        bool Authenticate(LoginDto datos);

        string RememberUser(LoginDto datos);
        string getRole(String email);
    }
}
