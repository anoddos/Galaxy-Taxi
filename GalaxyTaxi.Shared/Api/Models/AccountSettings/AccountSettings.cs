using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;
[ProtoContract]
[Serializable]
public class AccountSettings
{
    [ProtoMember(1)]
    public string CompanyName { get; set; }
    [ProtoMember(2)]
    public string Email { get; set; }
    [ProtoMember(3)]
    public double MaxAmountPerEmployee { get; set; }
    [ProtoMember(4)]
    public bool IsCustomerCompany { get; set; }
}
