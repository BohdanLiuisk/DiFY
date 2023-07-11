using Dify.Core.Domain.Common;

namespace Dify.Core.Domain.Entities;

public class User : BaseAuditableEntity<int>
{
    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }
}
