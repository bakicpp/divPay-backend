using System;
using System.Collections.Generic;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPaymentRequestService
    {
        IResult SendTransferRequest(int cardId,int senderHesapNo, int receiverHesapNo, double transferAmount, DivideType divideType);
        IDataResult<List<PaymentRequest>> GetMyIncomingTransferRequests();
        IResult ApproveTransferRequest(int odemeIstegiId, double customAmount = 0);
    }
}

