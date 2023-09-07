using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Common;

    public static class AccountStatusMapping
    {
    public static readonly Dictionary<AccountStatus, string> StatusMapping = new()
    {
        { AccountStatus.Pending, "Pending" },
        { AccountStatus.Declined, "Declined" },
        { AccountStatus.Registered, "Registered"},
        { AccountStatus.Verified, "Verified" },

    };
}
