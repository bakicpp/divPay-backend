using System;
using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Entities.Concrete
{
	public class PaymentRequest:IEntity
	{
		[Key]
		public int OdemeIstegiId { get; set; }
		public int GondericiHesapNo { get; set; }
        public int AliciHesapNo { get; set; }
		public bool Durum { get; set; }
		public string Aciklama { get; set; }
		public double Miktar { get; set; }
		public string Tip { get; set; }
		public int KartId { get; set; }

	}
}

