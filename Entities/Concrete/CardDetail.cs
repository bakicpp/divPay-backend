using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class CardDetail : IEntity
    {
        public int id { get; set; }

        public int KartId { get; set; }

        public string KartAdi { get; set; }

        public int KartYoneticisi { get; set; }

        public int KartRenk { get; set; }

        public double ToplamHarcama { get; set; }
    }
}

