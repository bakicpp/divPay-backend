using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class Account : IEntity
    {
        public int id { get; set; }

        public int HesapNo { get; set; }

        public int MusteriNo { get; set; }

        public double Bakiye { get; set; }
    }
}