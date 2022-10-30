using System.Collections.Generic;

namespace DiFY.Modules.Social.Application.Calling.GetAllCalls;

public class CallsResultDto
{
    public IEnumerable<CallDto> Calls { get; set; }

    public int TotalCount { get; set; }
}
