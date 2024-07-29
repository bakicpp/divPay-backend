using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IClientService
    {
        IDataResult<List<Client>> GetAllClients();
        IDataResult<Client> GetByClientId(int musteriNo);
        IResult RemoveClientFromCard(Card card, int musteriNo);
        void Add(Client client);
        Client GetByClientNo(int musteriNo);
        List<OperationClaim> GetClaims(Client client);
        int GetCurrentMusteriNo();
    }
}

