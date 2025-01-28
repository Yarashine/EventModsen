﻿namespace EventModsen.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfRegistration { get; set; }
    public string Email { get; set; }


}
