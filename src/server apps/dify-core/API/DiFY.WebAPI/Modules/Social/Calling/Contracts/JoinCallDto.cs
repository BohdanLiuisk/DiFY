using System;

namespace DiFY.WebAPI.Modules.Social.Calling.Contracts;

public class JoinCallDto
{
    public string StreamId { get; set; }
    
    public string PeerId { get; set; }

    public Guid CallId { get; set; }
}
