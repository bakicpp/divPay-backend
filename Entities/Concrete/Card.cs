using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class Card : IEntity
    {
        public int id { get; set; }

        public int KartId { get; set; }

        public int MusteriNo { get; set; }

        public int HarcamaId { get; set; }

        //public string KartAdi { get; set; }

        //public string KartYoneticisi { get; set; }

        //public int KartRenk { get; set; }

    }
}

