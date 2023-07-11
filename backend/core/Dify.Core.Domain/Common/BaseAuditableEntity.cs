using Dify.Core.Domain.Entities;

namespace Dify.Core.Domain.Common;

public abstract class BaseAuditableEntity<T> : BaseEntity<T> where T: struct
{
    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public int? CreatedById { get; set; }

    public User CreatedBy { get; set; }

    public int? ModifiedById { get; set; }

    public User ModifiedBy { get; set; }
}
