using System;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<Client> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<Client> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(int musteriNo);
        IDataResult<AccessToken> CreateAccessToken(Client user);
    }
}

