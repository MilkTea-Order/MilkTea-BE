using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Utils;

namespace MilkTea.API.RestfulAPI.Common
{
    public class ResultsReturned
    {
        public static ResponseDto NotFound(string Message = Parameters.TEXT_NOT_FOUND)
        {
            ResponseDto resultToReturn = new()
            {
                Description = Parameters.CODE_SUCCESS + " - " + Parameters.TEXT_SUCCESS + ", " + Parameters.CODE_ERROR + " - Error, " + Parameters.CODE_NOT_FOUND + " - " + Parameters.TEXT_NOT_FOUND,
                Data = null,
                Code = Parameters.CODE_NOT_FOUND,
                Message = Message
            };

            return resultToReturn;
        }



        public static ResponseDto Error(string Message)
        {
            ResponseDto resultToReturn = new()
            {
                Description = Parameters.CODE_SUCCESS + " - " + Parameters.TEXT_SUCCESS + ", " + Parameters.CODE_ERROR + " - Error",
                Data = null,
                Code = Parameters.CODE_ERROR,
                Message = Message
            };

            return resultToReturn;
        }

        public static ResponseDto InternalError(
            Exception? ex = null,
            string? customMessage = null,
            string? callFrom = null)
        {
            // Tự động lấy callFrom nếu null
            callFrom ??= MethodInfoHelper.GetCallFrom(2);

            var logMessage = ex != null
                ? $"{callFrom}:\n{customMessage}\nException: {ex.Message}\n{ex.StackTrace}"
                : $"{callFrom}: {customMessage}";

            LogHelper.Write(logMessage);

            return ResultsReturned.Error(customMessage ?? "INTERNAL_SERVER_ERROR");
        }

        public static ResponseDto TokenError()
        {
            return ResultsReturned.Error("INVALID_TOKEN");
        }


        public static ResponseDto ErrorWithLog(
            string message,
            string? callFrom = null,
            Exception? ex = null)
        {
            callFrom ??= MethodInfoHelper.GetCallFrom(2);

            var logMessage = ex != null
                ? $"{callFrom}:\n{message}\nException: {ex.Message}\n{ex.StackTrace}"
                : $"{callFrom}: {message}";

            LogHelper.Write(logMessage);

            return ResultsReturned.Error(message);
        }

        /// <summary>
        /// @see HTG_CWOS_API.Customer.Utils.ErrorCode
        /// </summary>
        /// <param name="Data">@see StringListEntry</param>
        /// <returns></returns>
        public static ResponseDto ErrorDetail(object Data)
        {
            ResponseDto resultToReturn = new()
            {
                Description = Parameters.CODE_SUCCESS + " - " + Parameters.TEXT_SUCCESS + ", " + Parameters.CODE_ERROR + " - Error",
                Data = Data,
                Code = Parameters.CODE_ERROR,
            };

            return resultToReturn;
        }

        public static ResponseDto Success(object Data, string Message = Parameters.TEXT_SUCCESS)
        {
            ResponseDto resultToReturn = new()
            {
                Description = Parameters.CODE_SUCCESS + " - " + Parameters.TEXT_SUCCESS + ", " + Parameters.CODE_ERROR + " - Error",
                Data = Data,
                Code = Parameters.CODE_SUCCESS,
                Message = Message
            };

            return resultToReturn;
        }

        public static ResponseDto Success(string Message = Parameters.TEXT_SUCCESS)
        {
            ResponseDto resultToReturn = new()
            {
                Description = Parameters.CODE_SUCCESS + " - " + Parameters.TEXT_SUCCESS + ", " + Parameters.CODE_ERROR + " - Error",
                Data = null,
                Code = Parameters.CODE_SUCCESS,
                Message = Message
            };

            return resultToReturn;
        }
    }
}
