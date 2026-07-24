using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _timeToLiveSeconds;

        public RedisCacheAttribute(int timeToLiveSeconds = 60)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        // work before endpoint and after endpoint run
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // get cahe service 
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // check if cache exist => skip endpoint and return cache
            var caheKey = CreateCacheKey(context.HttpContext.Request);
            var data = await cacheService.GetDataAsync(caheKey);
            if (!string.IsNullOrEmpty(data))
            {
                context.Result = new ContentResult
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            // if not exist run endpoint and storing result in cache [StatusCode.200Ok , data]
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult { Value: not null } ok)
            {
                await cacheService.SetDataAsync(caheKey, ok.Value, TimeSpan.FromSeconds(90));
            }

        }

        private static string CreateCacheKey(HttpRequest request)
        {
            // get cache key from request path and query string
            var Key = new StringBuilder();
            Key.Append(request.Path);
            // speicifaction 
            if (request.Query.Any())
            {
                Key.Append('?');
                foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
                {
                    Key.Append(key).Append('=').Append(value).Append('&');
                }
            }
            return Key.ToString();
        }
    }
    }
