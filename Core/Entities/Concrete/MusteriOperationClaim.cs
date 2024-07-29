namespace Core.Entities.Concrete
{
    public class MusteriOperationClaim : IEntity
    {
        public int Id { get; set; }
        public int MusteriNo { get; set; }
        public int OperationClaimId { get; set; }

    }
}

