using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.ValueObjects;

// Value Object que representa un Email válido dentro del dominio
public sealed record Email
{
    // Valor interno del email
    public string Value { get; }

    // Longitud mínima permitida para un email
    private const int MinEmailLength = 6;

    // Longitud máxima total permitida según estándar RFC
    private const int MaxEmailLength = 254;

    // Longitud máxima de la parte antes del @ (usuario)
    private const int MaxLocalPartLength = 64;

    // Longitud máxima del dominio (parte después del @)
    private const int MaxDomainLength = 190;

    // Constructor que valida todas las reglas del email
    public Email(string email)
    {
        // Verifica que el email no sea null, vacío o solo espacios
        if (string.IsNullOrWhiteSpace(email))
            throw new BusinessRuleException("El email es obligatorio.");

        // Elimina espacios al inicio y final
        email = email.Trim();

        // Valida reglas generales del email
        ValidateEmail(email);

        // Obtiene la posición del símbolo @
        int atIndex = email.IndexOf('@');

        // Obtiene la parte antes del @ (usuario)
        var localPart = email[..atIndex];

        // Obtiene la parte después del @ (dominio)
        var domainPart = email[(atIndex + 1)..];

        // Valida la parte local del email
        ValidateLocalPart(localPart);

        // Valida el dominio del email
        ValidateDomainPart(domainPart);

        // Guarda el email en minúsculas para mantener consistencia
        Value = email.ToLowerInvariant();
    }

    // Validaciones generales del email completo
    private static void ValidateEmail(string email)
    {
        // Verifica longitud mínima
        if (email.Length < MinEmailLength)
            throw new BusinessRuleException($"El email debe tener al menos {MinEmailLength} caracteres.");

        // Verifica longitud máxima
        if (email.Length > MaxEmailLength)
            throw new BusinessRuleException($"El email no puede exceder {MaxEmailLength} caracteres.");

        // Busca la posición del símbolo @
        int atIndex = email.IndexOf('@');

        // No puede iniciar con @
        if (atIndex <= 0)
            throw new BusinessRuleException("El email no puede iniciar con '@'.");

        // No puede terminar con @
        if (atIndex >= email.Length - 1)
            throw new BusinessRuleException("El email no puede terminar con '@'.");

        // Verifica que solo exista un @
        if (atIndex != email.LastIndexOf('@'))
            throw new BusinessRuleException("El email debe contener un solo '@'.");
    }

    // Validaciones de la parte local (usuario antes del @)
    private static void ValidateLocalPart(string localPart)
    {
        // Verifica que no supere la longitud máxima
        if (localPart.Length > MaxLocalPartLength)
            throw new BusinessRuleException($"La parte antes del '@' no puede exceder {MaxLocalPartLength} caracteres.");

        // No puede iniciar con punto
        if (localPart.StartsWith('.') || localPart.EndsWith('.'))
            throw new BusinessRuleException("La parte antes del '@' no puede iniciar ni terminar con punto.");

        // No puede contener puntos consecutivos
        if (localPart.Contains(".."))
            throw new BusinessRuleException("La parte antes del '@' no puede contener puntos consecutivos.");

        // Valida caracteres permitidos
        if (!localPart.All(c =>
            char.IsLetterOrDigit(c) || // letras o números
            c == '.' ||                // punto
            c == '_' ||                // guión bajo
            c == '-' ||                // guión
            c == '+'))                 // símbolo +
        {
            throw new BusinessRuleException("La parte antes del '@' contiene caracteres inválidos.");
        }
    }

    // Validaciones del dominio (parte después del @)
    private static void ValidateDomainPart(string domainPart)
    {
        // Verifica que el dominio exista
        if (string.IsNullOrWhiteSpace(domainPart))
            throw new BusinessRuleException("El dominio del email es obligatorio.");

        // Verifica longitud máxima del dominio
        if (domainPart.Length > MaxDomainLength)
            throw new BusinessRuleException($"El dominio no puede exceder {MaxDomainLength} caracteres.");

        // Debe contener al menos un punto (ej: gmail.com)
        if (!domainPart.Contains('.'))
            throw new BusinessRuleException("El dominio debe contener al menos un punto.");

        // No puede iniciar o terminar con punto
        if (domainPart.StartsWith('.') || domainPart.EndsWith('.'))
            throw new BusinessRuleException("El dominio no puede iniciar ni terminar con punto.");

        // No puede tener puntos consecutivos
        if (domainPart.Contains(".."))
            throw new BusinessRuleException("El dominio no puede contener puntos consecutivos.");

        // Solo caracteres válidos para dominio
        if (!domainPart.All(c =>
            char.IsLetterOrDigit(c) || // letras o números
            c == '.' ||                // punto
            c == '-'))                 // guión
        {
            throw new BusinessRuleException("El dominio contiene caracteres inválidos.");
        }

        // Divide el dominio para obtener la extensión final
        var parts = domainPart.Split('.');

        // Obtiene el TLD (Top Level Domain) ej: com, org
        var tld = parts[^1];

        // La extensión debe tener al menos 2 caracteres
        if (tld.Length < 2)
            throw new BusinessRuleException("El dominio debe tener una extensión válida (ej: .com, .org).");
    }

    // Permite convertir el objeto Email a string automáticamente
    public override string ToString() => Value;

    // Conversión implícita para usar Email como string
    public static implicit operator string(Email email) => email.Value;
}