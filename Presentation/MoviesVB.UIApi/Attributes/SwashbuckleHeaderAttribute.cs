using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace MoviesVB.UIApi.Attributes
{
    public class SwashbuckleHeaderAttribute
    {
        public class AddRequiredHeaderParamter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                
                var requiredScopes = context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<IAllowAnonymous>()
                    .Distinct();

                if(!requiredScopes.Any())
                {
                    if (operation.Parameters == null)
                        operation.Parameters = new List<OpenApiParameter>();

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "ApiKey",
                        In = ParameterLocation.Header,
                        Required = true,
                        Description = "Endpoint secured with Api key"
                    });
                }
                
            }
        }
    }
}
