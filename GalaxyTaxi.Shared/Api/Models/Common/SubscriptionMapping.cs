using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Common
{
    public static class SubscriptionMapping
    {
        public static readonly Dictionary<SubscriptionPlanType, int> Mapping = new()
        {
            { SubscriptionPlanType.Weekly, 10 },
            { SubscriptionPlanType.Monthly, 10 },
            { SubscriptionPlanType.Annual, Int32.MaxValue}
        };
    }

}
