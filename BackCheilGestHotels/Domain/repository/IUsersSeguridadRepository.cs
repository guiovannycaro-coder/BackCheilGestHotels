using BackCheilGestHotels.application.entities;

namespace BackCheilGestHotels.Domain.repository
{
    public interface IUsersSeguridadRepository
    {


        public bool authenticate(LoginDto datos);

        public String rememberUser(LoginDto datos);

        public String getProfile(String email);
    }
}
