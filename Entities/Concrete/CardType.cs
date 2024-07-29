using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class CardType : IEntity
    {
        public int id { get; set; }

        public int KartId { get; set; }

        public string KartTipi { get; set; }

    }
}

