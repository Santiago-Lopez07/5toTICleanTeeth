using CleanTeeth.Domain.Exceptions;
using CleanTeeth.Domain.ValueObjects;

namespace CleanTeeth.Domain.Entities;
public class Dentist
{
    public Guid Id { get; private set; }
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    public Dentist(string name, string email)
    {

        Id = Guid.CreateVersion7();

        // Aquí se crea el ValueObject,ellos validan las reglas de negocio, por lo que no es necesario
        // validar nada más aquí
        Email = new Email(email);
        Name = new Name(name);
    }

}
