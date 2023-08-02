using Dify.Core.Domain.Entities;

namespace Dify.Core.Domain.Common;

public interface IBaseAuditableEntity
{
    DateTime CreatedOn { get; set; }

    DateTime ModifiedOn { get; set; }

    int? CreatedById { get; set; }

    User CreatedBy { get; set; }

    int? ModifiedById { get; set; }

    User ModifiedBy { get; set; }
}
