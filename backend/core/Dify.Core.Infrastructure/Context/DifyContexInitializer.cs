using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dify.Core.Infrastructure.Context;

public class DifyContextInitializer
{
    private readonly ILogger<DifyContextInitializer> _logger;

    private readonly DifyContext _context;

    public DifyContextInitializer(ILogger<DifyContextInitializer> logger, DifyContext context)
    {
        _context = context;
        _logger = logger;
    }

    public void Initialise()
    {
        try
        {
            _context.Database.Migrate();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}
