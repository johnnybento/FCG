using FGC.Core.Exceptions;
using System.Drawing;
using System.Text.RegularExpressions;

namespace FGC.Core.Users.ValueObjects;

public sealed class SenhaVo
{
    public string Value { get; private set; }

    private SenhaVo(string valor) => Value = valor;

    public static SenhaVo Create(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new DomainException("Senha não pode ser vazia.");

        if (senha.Length < 8)
            throw new DomainException("Senha deve ter ao menos 8 caracteres.");

        if (!Regex.IsMatch(senha, @"[A-Za-z]"))
            throw new DomainException("Senha deve conter ao menos uma letra.");

        if (!Regex.IsMatch(senha, @"\d"))
            throw new DomainException("Senha deve conter ao menos um número.");

        if (!Regex.IsMatch(senha, @"[!@#$%^&*(),.?""':{}|<>_\-\\/\[\];]"))
            throw new DomainException("Senha deve conter ao menos um caractere especial.");

        return new SenhaVo(senha);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not SenhaVo other) return false;
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}
