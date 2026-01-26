using MilkTea.Domain.SharedKernel.Constants;

namespace MilkTea.Application.Models.Errors
{
    public class ValidationError
    {
        public string Code { get; }
        public string[] Fields { get; }

        public ValidationError(string code, params string[] fields)
        {
            Code = code;
            Fields = fields;
        }
        public static ValidationError InvalidData(params string[] fields)
        {
            return new ValidationError(ErrorCode.ValidationFailed, fields);
        }

        public static ValidationError NotExist(params string[] fields)
        {
            return new ValidationError(ErrorCode.NotFound, fields);
        }

        public static ValidationError SendError(string errorCode, params string[] fields)
        {
            return new ValidationError(errorCode, fields);
        }
    }
}
