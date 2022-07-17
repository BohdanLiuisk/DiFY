using System;
using DiFY.BuildingBlocks.Domain;

namespace DiFY.Modules.Social.Domain.Membership;

public class Member : Entity, IAggregateRoot
{
    public MemberId Id { get; private set; }
    
    private string _login;

    private string _email;

    private string _firstName;

    private string _lastName;

    private string _name;

    private DateTime _createdOn;
    
    private Member() { }
    
    private Member(Guid id, string login, string email, string firstName, 
        string lastName, string name, DateTime createdOn)
    {
        Id = new MemberId(id);
        _login = login;
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
        _name = name;
        _createdOn = createdOn;
    }

    public static Member Create(Guid id, string login, string email, string firstName,
        string lastName, string name, DateTime createdOn)
    {
        return new Member(id, login, email, firstName, lastName, name, createdOn);
    }
}