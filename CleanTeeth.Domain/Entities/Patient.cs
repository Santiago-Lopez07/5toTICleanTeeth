using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;

public class Patient
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    public Patient(string name, string email)
    {

        Id = Guid.CreateVersion7();

        // Aquí se crea el ValueObject ya que ellos validan las reglas
        Email = new Email(email);
        Name = new Name(name);
        
    }
}
