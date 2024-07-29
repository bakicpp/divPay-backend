using System;
using System.Collections.Generic;
using Core.DataAccess;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IPaymentRequestDal : IEntityRepository<PaymentRequest>
    {
        void SendTransferRequest(int cardId, int senderHesapNo, int receiverHesapNo, double transferAmount, DivideType divideType);

        List<PaymentRequest> GetMyIncomingTransferRequests(int hesapNo);

        void ApproveTransferRequest(int odemeIstegiId, CardDetailDTO cardDetail, double customAmount = 0);
    }
}

