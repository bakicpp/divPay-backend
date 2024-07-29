using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private ITokenHelper _tokenHelper;
        private IClientService _clientService;

        public AuthManager(ITokenHelper tokenHelper, IClientService clientService)
        {
            _tokenHelper = tokenHelper;
            _clientService = clientService;
            
        }

        public IDataResult<Client> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new Client
            {
                //Email = userForRegisterDto.Email,
                MusteriNo = userForRegisterDto.MusteriNo,
                MusteriAdi = userForRegisterDto.MusteriAdi,
                MusteriSoyadi = userForRegisterDto.MusteriSoyadi,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                //Status = true
            };
            _clientService.Add(user);
            return new SuccessDataResult<Client>(user, "Kayıt oldu");
        }

        public IDataResult<Client> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _clientService.GetByClientNo(userForLoginDto.MusteriNo);
            if (userToCheck == null)
            {
                return new ErrorDataResult<Client>("Kullanıcı bulunamadı");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Sifre, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
            
                return new ErrorDataResult<Client>("Parola hatası");
            }

            return new SuccessDataResult<Client>(userToCheck, "Başarılı giriş");
        }

        public IResult UserExists(int musteriNo)
        {
            if (_clientService.GetByClientNo(musteriNo) != null)
            {
                return new ErrorResult("Kullanıcı mevcut");
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(Client user)
        {
            var claims = _clientService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, "Token oluşturuldu");
        }

      
    }
}