using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api
{
    public class OfficeManagementService : IOfficeManagementService
    {

        private readonly Db _db;
        private readonly IAddressDetectionService _addressDetectionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OfficeManagementService(Db db, IAddressDetectionService addressDetectionService, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _addressDetectionService = addressDetectionService;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task EditOfficeDetails(EditOfficeRequest request, CallContext context = default)
        {
            throw new NotImplementedException();
        }


        private string GetSessionValue(string key)
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(key) ?? "";
        }

        public async Task<GetOfficesResponse> GetOffices(OfficeManagementFilter? filter = null, CallContext context = default)
        {
            long customerCompanyId = long.Parse(GetSessionValue("CustomerCompanyId"));

            var offices = from office in _db.Offices.Include(o => o.Address)
                            where office.CustomerCompanyId == customerCompanyId
                            select new OfficeInfo
                            {
                                OfficeId = office.Id,
                                WorkingStartTime = office.WorkingStartTime,
                                WorkingEndTime = office.WorkingEndTime,
                                Address = new AddressInfo
                                {
                                    Name = office.Address.Name,
                                    Latitude = office.Address.Latitude,
                                    Longitude = office.Address.Longitude
                                }
                            };

            var response = new GetOfficesResponse { Offices = await offices.ToListAsync() };
            return response;
        }
    }
}
