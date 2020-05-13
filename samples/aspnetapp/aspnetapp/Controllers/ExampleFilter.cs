using System;  
using Microsoft.AspNetCore.Mvc.Filters;
using Datadog.Trace;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Filters  
{  
    public class ExampleFilterAttribute : ActionFilterAttribute
    {  
         
          
        public override void OnActionExecuting(ActionExecutingContext context)  
        {  
            //To do : before the action executes  
            var scope = Tracer.Instance.ActiveScope;
            
           if (scope?.Span != null)
            {
				scope.Span.SetTag("workingfilterbefore", "yesbefore");
            }
            else 
            {
            	scope.Span.SetTag("workingfiterbefore", "nobefore");
            }
        }  
  
        public override void OnActionExecuted(ActionExecutedContext context)  
        {  
            //To do : after the action executes  
            var scope = Tracer.Instance.ActiveScope;
            
           if (scope?.Span != null)
            {
				scope.Span.SetTag("workingfilterafter", "yesafter");
            }
            else 
            {
            	scope.Span.SetTag("workingfilterafter", "noafter");
            }			
        }  
    }


    public class ExampleFilterAttributeAsync : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {  
            //To do : after the action executes  
            var scope = Tracer.Instance.ActiveScope;
            
           if (scope?.Span != null)
            {
                scope.Span.SetTag("workingfilterasyncbefore", "yesasyncbefore");
            }
            else 
            {
                scope.Span.SetTag("workingfilterasyncbefore", "noasyncbefore");
            }    

            var resultContext = await next();

            var scopetwo = Tracer.Instance.ActiveScope;

           if (scopetwo?.Span != null)
            {
                scopetwo.Span.SetTag("workingfilterasyncafter", "yesasyncafter");
            }
            else 
            {
                scopetwo.Span.SetTag("workingfilterasyncafter", "noasyncafter");
            }    

        }   
    }
} 