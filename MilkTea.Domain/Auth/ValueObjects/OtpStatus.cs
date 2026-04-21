namespace MilkTea.Domain.Auth.ValueObjects;

public readonly struct OtpStatus
{
    public string Value { get; }

    public static OtpStatus Default  => new("SUCCESS");
    public static OtpStatus Success  => Default;
    public static OtpStatus Error   => new("ERROR");

    private OtpStatus(string value) => Value = value;

    public static OtpStatus Create(string value)
        => value.ToUpperInvariant().Trim() switch
        {
            "SUCCESS"   => Success,
            "ERROR"     => Error,
            _           => throw new ArgumentException($"Invalid OtpStatus: '{value}'. Expected one of: SUCCESS, ERROR.")
        };

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is OtpStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(OtpStatus a, OtpStatus b) => a.Value == b.Value;
    public static bool operator !=(OtpStatus a, OtpStatus b) => a.Value != b.Value;

    public static implicit operator string(OtpStatus status) => status.Value;
    public static explicit operator OtpStatus(string value) => Create(value);
}
