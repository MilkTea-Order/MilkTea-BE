using MilkTea.Domain.Auth.Exceptions;

namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct Channel
{
    public string Value { get; }

    public static Channel Email => new("EMAIL");
    public static Channel Sms => new("SMS");

    private Channel(string value) => Value = value;

    public static Channel Create(string value)
        => value.ToUpperInvariant().Trim() switch
        {
            "EMAIL" => Email,
            "SMS" => Sms,
            _ => throw new InvalidChannelException()
        };

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Channel other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Channel a, Channel b) => a.Value == b.Value;
    public static bool operator !=(Channel a, Channel b) => a.Value != b.Value;

    public static implicit operator string(Channel channel) => channel.Value;
    public static explicit operator Channel(string value) => Create(value);
}
