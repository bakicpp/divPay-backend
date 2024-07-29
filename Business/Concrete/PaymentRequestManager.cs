using System;
using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class PaymentRequestManager : IPaymentRequestService
    {
        IPaymentRequestDal _paymentRequestDal;
        IClientService _clientService;
        IAccountService _accountService;
        ICardService _cardService;
        public PaymentRequestManager(IPaymentRequestDal paymentRequestDal, IClientService clientService, IAccountService accountService, ICardService cardService)
        {
            _paymentRequestDal = paymentRequestDal;
            _clientService = clientService;
            _accountService = accountService;
            _cardService = cardService;
        }

        public IResult ApproveTransferRequest(int odemeIstegiId, double customAmount = 0)
        {
            var paymentRequest = _paymentRequestDal.Get(o => o.OdemeIstegiId == odemeIstegiId);
            Console.WriteLine(paymentRequest.KartId);

            var cardDetail = _cardService.GetCardDetails(paymentRequest.KartId).Data;

            _paymentRequestDal.ApproveTransferRequest(odemeIstegiId, cardDetail, customAmount);
            return new SuccessResult("Ödeme isteği kabul edildi ve transfer işlemi başarıyla gerçekleşti.");
        }

        public IDataResult<List<PaymentRequest>> GetMyIncomingTransferRequests()
        {
            var musteriNo = _clientService.GetCurrentMusteriNo();
            var account = _accountService.GetAccountByMusteriNo(musteriNo);

            var res = _paymentRequestDal.GetMyIncomingTransferRequests(account.Data.HesapNo);
            return new SuccessDataResult<List<PaymentRequest>>(res, "Token sahibi kullanıcının gelen transfer istekleri listelendi.");

        }

        public IResult SendTransferRequest(int cardId,int senderHesapNo, int receiverHesapNo, double amount, DivideType divideType)
        {
            _paymentRequestDal.SendTransferRequest(cardId,senderHesapNo, receiverHesapNo, amount, divideType);
            return new SuccessResult("Ödeme isteği başarıyla gönderildi!");
        }
    }
}

