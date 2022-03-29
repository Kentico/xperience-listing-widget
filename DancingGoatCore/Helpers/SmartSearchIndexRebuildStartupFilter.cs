using System;

using DancingGoat.Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace DancingGoat.Helpers
{
    public class SmartSearchIndexRebuildStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                // Ensures smart search index rebuild upon installation or deployment
                builder.UseMiddleware<SmartSearchIndexRebuildMiddleware>();

                next(builder);
            };
        }
    }
}
