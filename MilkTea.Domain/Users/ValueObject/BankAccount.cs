using MilkTea.Domain.SharedKernel.Constants;
using System.Text.RegularExpressions;

namespace MilkTea.Domain.Users.ValueObject;

public sealed record BankAccount(
    string AccountNumber,
    string AccountName,
    string BankName,
    byte[]? QrCode)
{
    private static readonly Regex AccountNumberRegex =
        new(@"^[0-9]{6,20}$", RegexOptions.Compiled);

    public static BankAccount Empty()
        => new(string.Empty, string.Empty, string.Empty, null);

    public bool IsEmpty =>
        string.IsNullOrEmpty(AccountNumber) &&
        string.IsNullOrEmpty(AccountName) &&
        string.IsNullOrEmpty(BankName) &&
        QrCode is null;

    public static BankAccount Of(string accountNumber, string accountName, string bankName, byte[]? qrCode)
    {
        bool isHavEmpty = false;
        string fields = "";
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            fields += "bankAccountNumber";
            isHavEmpty = true;
        }

        if (string.IsNullOrWhiteSpace(accountName))
        {
            if (isHavEmpty)
                fields += ", ";
            fields += "bankAccountName";
            isHavEmpty = true;
        }

        if (string.IsNullOrWhiteSpace(bankName))
        {
            if (isHavEmpty)
                fields += ", ";
            fields += "bankName";
            isHavEmpty = true;
        }
        if (isHavEmpty) throw new ArgumentException(ErrorCode.E0001, fields);

        accountNumber = accountNumber.Trim();

        if (!AccountNumberRegex.IsMatch(accountNumber))
            throw new ArgumentException(ErrorCode.E0036, "bankAccountNumber");

        return new BankAccount(
            accountNumber,
            accountName.Trim(),
            bankName.Trim(),
            qrCode);
    }

    /// <summary>
    /// Updates partial fields. If a parameter is null or empty, keeps the existing value from current.
    /// </summary>
    public BankAccount UpdatePartial(
        string? accountNumber = null,
        string? accountName = null,
        string? bankName = null,
        byte[]? qrCode = null)
    {
        var newAccountNumber = !string.IsNullOrWhiteSpace(accountNumber) ? accountNumber.Trim() : AccountNumber;
        var newAccountName = !string.IsNullOrWhiteSpace(accountName) ? accountName.Trim() : AccountName;
        var newBankName = !string.IsNullOrWhiteSpace(bankName) ? bankName.Trim() : BankName;
        var newQrCode = qrCode ?? QrCode;

        // Nếu tất cả đều empty thì trả về Empty
        if (string.IsNullOrWhiteSpace(newAccountNumber) &&
            string.IsNullOrWhiteSpace(newAccountName) &&
            string.IsNullOrWhiteSpace(newBankName) &&
            newQrCode is null)
        {
            return Empty();
        }

        // Validate và tạo BankAccount mới
        return Of(newAccountNumber, newAccountName, newBankName, newQrCode);
    }
}

