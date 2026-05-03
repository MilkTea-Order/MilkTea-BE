using System;
using System.Collections.Generic;
using MilkTea.Domain.Auth.Exceptions;

namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct Channel : IEquatable<Channel>
{
    public string Value { get; }

    public static readonly Channel Email = new("EMAIL");
    public static readonly Channel Sms = new("SMS");

    private static readonly HashSet<string> ValidValues = new(StringComparer.OrdinalIgnoreCase)
    {
        Email.Value,
        Sms.Value
    };

    private Channel(string value) => Value = value;

    public static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && ValidValues.Contains(value.Trim());

    public static Channel Create(string value)
    {
        if (!IsValid(value))
            throw new InvalidChannelException();

        return new Channel(value.ToUpperInvariant().Trim());
    }

    public override string ToString() => Value;

    public bool Equals(Channel other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is Channel other && Equals(other);

    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static bool operator ==(Channel left, Channel right) => left.Equals(right);
    public static bool operator !=(Channel left, Channel right) => !left.Equals(right);

    public static implicit operator string(Channel channel) => channel.Value;
    public static explicit operator Channel(string value) => Create(value);
}
