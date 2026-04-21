using MilkTea.Domain.Auth.Exceptions;

namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct SessionFunction
{
    public string Value { get; }

    public static SessionFunction ValidateCustomer => new("VALIDATE_CUSTOMER");
    public static SessionFunction Register => new("REGISTER");
    public static SessionFunction ValidateEmail => new("VALIDATE_EMAIL");
    public static SessionFunction ValidatePhone => new("VALIDATE_PHONE");
    public static SessionFunction ResetPassword => new("RESET_PASSWORD");
    public static SessionFunction Login => new("LOGIN");

    private SessionFunction(string value) => Value = value;

    public static SessionFunction Create(string value)
        => value.ToUpperInvariant().Trim() switch
        {
            "VALIDATE_CUSTOMER" => ValidateCustomer,
            "REGISTER" => Register,
            "VALIDATE_EMAIL" => ValidateEmail,
            "VALIDATE_PHONE" => ValidatePhone,
            "RESET_PASSWORD" => ResetPassword,
            "LOGIN" => Login,
            _ => throw new InvalidFunctionSessionException()
        };

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is SessionFunction other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(SessionFunction a, SessionFunction b) => a.Value == b.Value;
    public static bool operator !=(SessionFunction a, SessionFunction b) => a.Value != b.Value;

    public static implicit operator string(SessionFunction function) => function.Value;
    public static explicit operator SessionFunction(string value) => Create(value);
}
