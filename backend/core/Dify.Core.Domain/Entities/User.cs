using Dify.Core.Domain.Common;

namespace Dify.Core.Domain.Entities;

public class User : BaseAuditableEntity<int>
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Name { get; set; }
    
    public string Login { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}
