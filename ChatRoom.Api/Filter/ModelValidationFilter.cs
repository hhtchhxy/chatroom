using ChatRoom.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace ChatRoom.Api.Filter
{
    public class ModelValidationFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var _unvali = (from v in context.ModelState.Values where v.ValidationState == ModelValidationState.Invalid select v).FirstOrDefault(); 
                context.Result =new JsonResult(ApiResult.Error(_unvali?.Errors[0].ErrorMessage));
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
