using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfClientDal : EfEntityRepositoryBase<Client, DivPayDBContext>, IClientDal
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EfClientDal(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<OperationClaim> GetClaims(Client client)
        {
            using var context = new DivPayDBContext();
            var result = from operationClaim in context.OperationClaims
                         join userOperationClaim in context.MusteriOperationClaims
                           on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.MusteriNo == client.id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
            return result.ToList();
        }

        public int GetCurrentMusteriNo()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new Exception("Authorization header is missing.");
            }

            var token = authorizationHeader.ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Token is missing or invalid.");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new Exception("Invalid token.");
            }
            var musteriNoClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // musteriNo was defined as NameIdentifier
            if (string.IsNullOrEmpty(musteriNoClaim))
            {
                throw new Exception("Müşteri numarası claim'i bulunamadı.");
            }

            if (int.TryParse(musteriNoClaim, out int musteriNo))
            {
                return musteriNo;
            }

            throw new Exception("Müşteri numarası geçersiz.");
        }
    }
}
