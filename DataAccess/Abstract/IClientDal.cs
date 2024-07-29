using System;
using System.Collections.Generic;
using Core.DataAccess;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace DataAccess.Abstract
{
	public interface IClientDal : IEntityRepository<Client>
	{
        List<OperationClaim> GetClaims(Client client);

        int GetCurrentMusteriNo();

    }
}

