using System;
using System.Collections.Generic;
using MilkTea.Domain.Auth.Exceptions;

namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct SessionFunction : IEquatable<SessionFunction>
{
    public string Value { get; }
    public static readonly SessionFunction ValidateCustomer = new("VALIDATE_CUSTOMER");
    public static readonly SessionFunction Register = new("REGISTER");
    public static readonly SessionFunction ValidateEmail = new("VALIDATE_EMAIL");
    public static readonly SessionFunction ValidatePhone = new("VALIDATE_PHONE");
    public static readonly SessionFunction ResetPassword = new("RESET_PASSWORD");
    public static readonly SessionFunction Login = new("LOGIN");

    private static readonly HashSet<string> ValidValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ValidateCustomer.Value,
        Register.Value,
        ValidateEmail.Value,
        ValidatePhone.Value,
        ResetPassword.Value,
        Login.Value
    };

    private SessionFunction(string value) => Value = value;

    public static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && ValidValues.Contains(value.Trim());

    public static SessionFunction Create(string value)
    {
        if (!IsValid(value))
            throw new InvalidFunctionSessionException();

        return new SessionFunction(value.ToUpperInvariant().Trim());
    }

    public override string ToString() => Value;

    public bool Equals(SessionFunction other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is SessionFunction other && Equals(other);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static bool operator ==(SessionFunction left, SessionFunction right) => left.Equals(right);
    public static bool operator !=(SessionFunction left, SessionFunction right) => !left.Equals(right);

    public static implicit operator string(SessionFunction function) => function.Value;
    public static explicit operator SessionFunction(string value) => Create(value);
}
