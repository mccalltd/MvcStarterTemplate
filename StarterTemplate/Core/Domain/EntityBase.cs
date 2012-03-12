using System;

namespace StarterTemplate.Core.Domain
{
    public abstract class ImmutableEntityBase
    {
        public long Id { get; set; }
    
        public long? CreatedByMemberId { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public abstract class MutableEntityBase : ImmutableEntityBase
    {
        public long? ModifiedByMemberId { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}