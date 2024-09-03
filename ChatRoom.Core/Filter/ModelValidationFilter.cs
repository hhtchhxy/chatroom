using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Filter
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var _unvali = (from v in context.ModelState.Values where v.ValidationState == ModelValidationState.Invalid select v).FirstOrDefault();
                context.Result = new JsonResult(ApiResult.Error(_unvali?.Errors[0].ErrorMessage));
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
