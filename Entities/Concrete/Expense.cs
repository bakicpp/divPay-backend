using System;
using Core.Entities;

namespace Entities.Concrete
{
	public class Expense : IEntity
	{
		public int id { get; set; }
		public int HarcamaId { get; set; }
		public double Miktar { get; set; }
		public int HesapNo { get; set; }
	}
}

