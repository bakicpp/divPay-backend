using System;
using System.Collections.Generic;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace Core.Utilities.Security.JWT
{
	public interface ITokenHelper
	{
		AccessToken CreateToken(Client user, List<OperationClaim> operationClaims);
	}
}

