﻿using System;
using DiFY.Modules.UserAccess.Application.Contracts;

namespace DiFY.Modules.UserAccess.Application.UserRegistrations.ConfirmUserRegistration
{
    public class ConfirmUserRegistrationCommand : CommandBase
    {
        public ConfirmUserRegistrationCommand(Guid userRegistrationId)
        {
            UserRegistrationId = userRegistrationId;
        }
        
        public Guid UserRegistrationId { get; }
    }
}