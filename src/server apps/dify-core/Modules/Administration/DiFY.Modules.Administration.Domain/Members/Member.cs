using System;
using DiFY.BuildingBlocks.Domain;
using DiFY.Modules.Administration.Domain.Members.Events;

namespace DiFY.Modules.Administration.Domain.Members
{
    public class Member : Entity, IAggregateRoot
    {
        public MemberId Id { get; }
             
        private string _login;
        
        private string _email;
        
        private string _firstName;
        
        private string _lastName;
        
        private string _name;
        
        private DateTime _createDate;
        
        private Member() { }
        
        private Member(Guid id, string login, string email, string firstName, string lastName, string name)
        {
            Id = new MemberId(id);
            _login = login;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = name;
            _createDate = DateTime.UtcNow;

            AddDomainEvent(new MemberCreatedDomainEvent(Id));
        }
        
        public static Member Create(Guid id, string login, string email, string firstName, string lastName, string name)
        {
            return new (id, login, email, firstName, lastName, name);
        }
    }
}