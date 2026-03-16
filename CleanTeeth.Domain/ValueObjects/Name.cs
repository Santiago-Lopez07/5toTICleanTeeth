using CleanTeeth.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace CleanTeeth.Domain.ValueObjects;

// Value Object que representa un nombre válido dentro del dominio
// Se usa 'record' porque los Value Objects comparan por valor y son inmutables
public sealed record Name
{
    // Expresión regular para validar nombres
    // \p{L} = cualquier letra (incluye acentos y caracteres internacionales)
    // \s = espacios
    // \- = guión
    // ^ $ = inicio y fin de la cadena
    // Solo permite letras, espacios y guiones
    private static readonly Regex ValidNameRegex =
        new(@"^[\p{L}\s\-]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Name(string value)
    {
        // Regla: el nombre no puede ser null, vacío o solo espacios
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleException("El nombre no puede estar vacío.");

        // Elimina espacios al inicio y al final
        value = value.Trim();

        // Regla: longitud mínima de 3 caracteres
        if (value.Length < 3)
            throw new BusinessRuleException("El nombre debe tener al menos 3 caracteres.");

        // Regla: longitud máxima de 50 caracteres
        if (value.Length > 50)
            throw new BusinessRuleException("El nombre no puede tener más de 50 caracteres.");

        // Regla: solo se permiten letras, espacios o guiones
        if (!ValidNameRegex.IsMatch(value))
            throw new BusinessRuleException("El nombre solo puede contener letras, espacios o guiones.");

        // Si todas las validaciones pasan, se asigna el valor
        Value = value;
    }

    // Devuelve el nombre como string cuando se imprime el objeto
    public override string ToString() => Value;
}