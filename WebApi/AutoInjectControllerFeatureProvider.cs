using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace WebApi
{
    public class AutoInjectControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var currentAssembly = typeof(Program).Assembly;
            var candidates = currentAssembly.GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

            foreach (var candidate in candidates)
            {
                feature.Controllers.Add(candidate.GetTypeInfo());
            }
        }
    }
}
