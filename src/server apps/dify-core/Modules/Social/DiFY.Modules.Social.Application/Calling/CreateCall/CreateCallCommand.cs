using System;
using DiFY.Modules.Social.Application.Contracts;

namespace DiFY.Modules.Social.Application.Calling.CreateCall;

public class CreateCallCommand : CommandBase<Guid>
{
    public CreateCallCommand(DateTime startDate)
    {
        StartDate = startDate;
    }
    
    public DateTime StartDate { get; }
}