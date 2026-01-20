using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MilkTea.API.RestfulAPI.Common;
using MilkTea.API.RestfulAPI.DTOs.Responses;
using MilkTea.Shared.Domain.Constants;
using MilkTea.Shared.Domain.Services;

namespace MilkTea.API.RestfulAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Dictionary<string, object> DefaultResponse(bool status, Dictionary<string, object>? data = null)
        {
            data ??= [];
            data["status"] = status;
            return data;
        }

        protected ResponseDto SendError(StringListEntry? error = null)
        {
            if (error == null)
            {
                return ResultsReturned.NotFound();
            }
            var vErrorDtl = error.GetData();
            var vLog = error.GetMeta(MetaKey.LOG_DATA);
            if (vLog != null)
            {
                vErrorDtl.Add(MetaKey.LOG_DATA, vLog);
            }
            return ResultsReturned.ErrorDetail(vErrorDtl);
        }

        protected ResponseDto SendError(string msg)
        {
            return ResultsReturned.ErrorDetail(msg);
        }

        protected ResponseDto SendTokenError(string? msg = null)
        {
            return ResultsReturned.TokenError(msg);
        }

        protected ResponseDto SendError(ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return ResultsReturned.ErrorDetail(errors);
        }

        protected ResponseDto SendSuccess(Dictionary<string, object>? data = null)
        {
            data ??= new Dictionary<string, object>
            {
                ["status"] = true
            };
            return ResultsReturned.Success(data);
        }
        protected ResponseDto SendSuccess(object data)
        {
            return ResultsReturned.Success(data);
        }
    }
}
