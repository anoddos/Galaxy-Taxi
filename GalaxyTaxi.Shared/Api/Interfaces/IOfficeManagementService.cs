using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("OfficeManagement")]
public interface IOfficeManagementService
{
    Task EditOfficeDetails(OfficeInfo request, CallContext context = default);
    Task<GetOfficesResponse> GetOffices(OfficeManagementFilter? filter = null, CallContext context = default);
	Task<OfficeInfo> AddOffice(OfficeInfo request, CallContext context = default);
}

