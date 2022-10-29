using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.CreateCall;

public class CreateCallCommand : CommandBase<Guid>
{
    public CreateCallCommand(string name, DateTime startDate)
    {
        Name = name;
        StartDate = startDate;
    }
    
    public string Name { get; }
    
    public DateTime StartDate { get; }
}