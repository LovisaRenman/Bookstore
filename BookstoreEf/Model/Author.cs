using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreEf;

public partial class Author
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateofBirth { get; set; }

    public DateOnly? DateofDeath { get; set; }

    public virtual ICollection<Book> BookIsbns { get; set; } = new List<Book>();

    [NotMapped]
    public string? Name { get; set; }


    public Author(string firstName, string lastName, DateOnly dateofBirth, DateOnly dateofDate)
    {
        FirstName = firstName;
        LastName = lastName;
        DateofBirth = dateofBirth;
        DateofDeath = DateofDeath;
    }

}
