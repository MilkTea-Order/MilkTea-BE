namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct SessionStatus
{
    public string Value { get; }

    public static SessionStatus Default   => new("PENDING");
    public static SessionStatus Pending   => Default;
    public static SessionStatus Verified  => new("VERIFIED");
    public static SessionStatus Expired   => new("EXPIRED");
    public static SessionStatus Cancelled => new("CANCELLED");

    private SessionStatus(string value) => Value = value;

    public static SessionStatus Create(string value)
        => value.ToUpperInvariant().Trim() switch
        {
            "PENDING"   => Pending,
            "VERIFIED"  => Verified,
            "EXPIRED"   => Expired,
            "CANCELLED" => Cancelled,
            _ => throw new ArgumentException($"Invalid SessionStatus: '{value}'. Expected one of: PENDING, VERIFIED, EXPIRED, CANCELLED.")
        };

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is SessionStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(SessionStatus a, SessionStatus b) => a.Value == b.Value;
    public static bool operator !=(SessionStatus a, SessionStatus b) => a.Value != b.Value;

    public static implicit operator string(SessionStatus status) => status.Value;
    public static explicit operator SessionStatus(string value) => Create(value);
}
