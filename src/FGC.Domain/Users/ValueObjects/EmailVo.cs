using FGC.Core.Exceptions;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FGC.Core.Users.ValueObjects;

public sealed class EmailVo
{
    private static readonly Regex _emailRegex = new(
          @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
          RegexOptions.Compiled | RegexOptions.IgnoreCase
      );

    public string Value { get; private set; }
    public EmailVo() { }
    private EmailVo(string valor) => Value = valor;

    public static EmailVo Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("E-mail não pode ser vazio.");

        email = email.Trim().ToLowerInvariant();
        if (!_emailRegex.IsMatch(email))
            throw new DomainException("Formato de e-mail inválido.");

        return new EmailVo(email.Trim().ToLowerInvariant());
    }

    public override bool Equals(object? obj)
    {
        if (obj is not EmailVo other) return false;
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString()
    {
        return Value.ToString();
    }
}
