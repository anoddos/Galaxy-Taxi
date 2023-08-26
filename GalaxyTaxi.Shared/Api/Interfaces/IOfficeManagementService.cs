using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("OfficeManagement")]
public interface IOfficeManagementService
{
    Task EditOfficeDetails(EditOfficeRequest request, CallContext context = default);
    Task<GetOfficesResponse> GetOffices(OfficeManagementFilter? filter = null, CallContext context = default);
}

