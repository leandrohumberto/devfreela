using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
namespace DevFreela.API.Filters
{
    // Esse filtro não é executado nos controllers que utilizam o
    // atributo Microsoft.AspNetCore.Mvc.ApiControllerAttribute, que dispara
    // uma resposta automática com a descrição dos erros de validação do objeto de modelo
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value != null)
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
            }
        }
    }
}
