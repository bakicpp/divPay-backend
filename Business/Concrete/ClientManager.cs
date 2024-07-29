using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class ClientManager : IClientService
    {
        IClientDal _clientDal;
        IHttpContextAccessor _httpContextAccessor;
        public ClientManager(IClientDal clientDal, IHttpContextAccessor httpContextAccessor)
        {
            _clientDal = clientDal;
            _httpContextAccessor = httpContextAccessor;

        }

        public void Add(Client client)
        {
            _clientDal.Add(client);
        }

        public IDataResult<List<Client>> GetAllClients()
        {
            return new SuccessDataResult<List<Client>>(_clientDal.GetAll(filter: null), Messages.ClientsListed);

        }

        public IDataResult<Client> GetByClientId(int musteriNo)
        {
            return new SuccessDataResult<Client>((Client)_clientDal.Get((Client p) => p.MusteriNo == musteriNo));

        }

        public Client GetByClientNo(int musteriNo)
        {
            return _clientDal.Get(c=> c.MusteriNo == musteriNo);

        }

        public List<OperationClaim> GetClaims(Client client)
        {
            return _clientDal.GetClaims(client);
        }

        public int GetCurrentMusteriNo()
        {
          return  _clientDal.GetCurrentMusteriNo();
        }

        public IResult RemoveClientFromCard(Card card, int musteriNo)
        {
            throw new NotImplementedException();
        }
    }
}

