using BackCheilGestHotels.application.entities;
using BackCheilGestHotels.application.iservices;
using BackCheilGestHotels.Domain.repository;
using BackCheilGestHotels.infra.Exceptions;

namespace BackCheilGestHotels.application.services
{
    public class UsersSeguridadServices : IUsersSeguridadService
    {

        private readonly IUsersSeguridadRepository _usersSRepository;

        public UsersSeguridadServices(IUsersSeguridadRepository usersSRepository)
        {
            _usersSRepository = usersSRepository;
        }


        public bool Authenticate(LoginDto datos)
        {
            try
            {
                return _usersSRepository.authenticate(datos);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while authenticating the user.",
                    ex);
            }
        }

        public string getRole(string email)
        {
            try
            {
                return _usersSRepository.getProfile(email);
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException(
                    500,
                    "An error occurred while retrieving the user's role.",
                    ex);
            }
        }

        public string RememberUser(LoginDto datos)
        {
            try
            {
                string result = _usersSRepository.rememberUser(datos);
                return result;
            }
            catch (BackCheilGestHotelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BackCheilGestHotelException( 500, "An error occurred while resetting the user's password.", ex);
            }
        }
    }
}
