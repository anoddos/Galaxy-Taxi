using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Auction;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.CustomerCompany;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using GalaxyTaxi.Shared.Api.Models.Register;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class AuctionService : IAuctionService
{
	private readonly Db _db;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public AuctionService(Db db, IHttpContextAccessor httpContextAccessor)
	{
		_db = db;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task Bid(BidRequest request, CallContext context = default)
	{
		await ValidateBidRequestAsync(request);

		var bid = new Bid
		{
			Amount = request.Amount,
			AccountId = GetAccountId(),
			AuctionId = request.AuctionId,
			TimeStamp = DateTime.UtcNow
		};

		try
		{
			await _db.Bids.AddAsync(bid);

			await _db.SaveChangesAsync();
		}
		catch (Exception)
		{
			throw new RpcException(new Status(StatusCode.Internal, "Could not insert into db"));
		}
	}

    public async Task<GetAuctionsResponse> GetAuction(AuctionsFilter filter, CallContext context = default)
    {
        var accountId = GetAccountId(false);

        var auctions = await _db.Auctions.Include(x => x.Bids)
            .Where(x => (filter.IsFinished == false || x.CurrentWinner != null)
                        && (filter.WonByMe == false || x.CurrentWinnerId == accountId)
                        && (filter.IncludesMe == false || x.CustomerCompany.AccountId == accountId ||
                            x.Bids.Any(xx => xx.AccountId == accountId)))
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new AuctionInfo
            {
                Id = x.Id,
                Amount = x.Amount,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Bids = x.Bids.Select(xx => new BidInfo
                {
                    Id = xx.Id,
                    TimeStamp = xx.TimeStamp,
                    Amount = xx.Amount,
                    Account = new AccountInfo
                    {
                        CompanyName = xx.Account.CompanyName,
                        Id = xx.Account.Id
                    }
                }),
                CurrentWinner = x.CurrentWinnerId == null
                    ? null
                    : new VendorCompanyInfo
                    {
                        Id = (long)x.CurrentWinnerId,
                        Name = x.CurrentWinner!.Name
                    },
                JourneyInfo = new JourneyInfo
                {
                    Id = x.Journey.Id,
                    Office = new OfficeInfo
                    {
                        OfficeId = x.Journey.Office.Id,
                        Address = new AddressInfo
                        {
                            Id = x.Journey.Office.Address.Id,
                            Name = x.Journey.Office.Address.Name,
                            Latitude = x.Journey.Office.Address.Latitude,
                            Longitude = x.Journey.Office.Address.Longitude
                        },
                        WorkingEndTime = x.Journey.Office.WorkingEndTime,
                        WorkingStartTime = x.Journey.Office.WorkingStartTime
                    },
                    CustomerCompany = new CustomerCompanyInfo
                    {
                        Id = x.CustomerCompany.Id,
                        Name = x.CustomerCompany.Name
                    },
                    Stops = x.Journey.Stops.Select(xx => new StopInfo
                    {
                        Id = xx.Id,
                        Address = new AddressInfo
                        {
                            Id = xx.EmployeeAddressId,
                            Name = xx.EmployeeAddress.Address.Name,
                            Latitude = xx.EmployeeAddress.Address.Latitude,
                            Longitude = xx.EmployeeAddress.Address.Longitude
                        },
                        EmployeeDetails = new SingleEmployeeInfo
                        {
                            FirstName = xx.EmployeeAddress.Employee.FirstName,
                            LastName = xx.EmployeeAddress.Employee.LastName,
                            Mobile = xx.EmployeeAddress.Employee.Mobile
                        }
                    })
                }
            }).ToListAsync();

        return new GetAuctionsResponse { Auctions = auctions };
    }

    public async Task<GetAuctionsCountResponse> GetAuctionCount(AuctionsFilter filter)
    {
        var accountId = GetAccountId(false);

        var count = _db.Auctions.Count(x => (filter.IsFinished == false || x.CurrentWinner != null)
                                            && (filter.WonByMe == false || x.CurrentWinnerId == accountId)
                                            && (filter.IncludesMe == false ||
                                                x.CustomerCompany.AccountId == accountId ||
                                                x.Bids.Any(xx => xx.AccountId == accountId)));

        return await Task.FromResult(new GetAuctionsCountResponse { TotalCount = count });
    }

    public async Task<GetSingleAuctionsResponse> GetSingleAuction(IdFilter filter)
    {
	     var auction = await _db.Auctions.Include(x => x.Bids)
            .Where(x => x.Id == filter.Id)
            .Select(x => new AuctionInfo
            {
                Id = x.Id,
                Amount = x.Amount,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Bids = x.Bids.Select(xx => new BidInfo
                {
                    Id = xx.Id,
                    TimeStamp = xx.TimeStamp,
                    Amount = xx.Amount,
                    Account = new AccountInfo
                    {
                        CompanyName = xx.Account.CompanyName,
                        Id = xx.Account.Id
                    }
                }),
                CurrentWinner = x.CurrentWinnerId == null
                    ? null
                    : new VendorCompanyInfo
                    {
                        Id = (long)x.CurrentWinnerId,
                        Name = x.CurrentWinner!.Name
                    },
                JourneyInfo = new JourneyInfo
                {
                    Id = x.Journey.Id,
                    Office = new OfficeInfo
                    {
                        OfficeId = x.Journey.Office.Id,
                        Address = new AddressInfo
                        {
                            Id = x.Journey.Office.Address.Id,
                            Name = x.Journey.Office.Address.Name,
                            Latitude = x.Journey.Office.Address.Latitude,
                            Longitude = x.Journey.Office.Address.Longitude
                        },
                        WorkingEndTime = x.Journey.Office.WorkingEndTime,
                        WorkingStartTime = x.Journey.Office.WorkingStartTime
                    },
                    CustomerCompany = new CustomerCompanyInfo
                    {
                        Id = x.CustomerCompany.Id,
                        Name = x.CustomerCompany.Name
                    },
                    Stops = x.Journey.Stops.Select(xx => new StopInfo
                    {
                        Id = xx.Id,
                        Address = new AddressInfo
                        {
                            Id = xx.EmployeeAddressId,
                            Name = xx.EmployeeAddress.Address.Name,
                            Latitude = xx.EmployeeAddress.Address.Latitude,
                            Longitude = xx.EmployeeAddress.Address.Longitude
                        },
                        EmployeeDetails = new SingleEmployeeInfo
                        {
                            FirstName = xx.EmployeeAddress.Employee.FirstName,
                            LastName = xx.EmployeeAddress.Employee.LastName,
                            Mobile = xx.EmployeeAddress.Employee.Mobile
                        }
                    })
                }
            }).SingleAsync();

        return new GetSingleAuctionsResponse { Auction = auction };
    }

    private async Task ValidateBidRequestAsync(BidRequest request)
	{
		var lastBid = (await _db.Bids.LastAsync(x => x.AuctionId == request.AuctionId)).Amount;
		if (lastBid <= request.Amount)
		{
			throw new RpcException(new Status(StatusCode.AlreadyExists, "New Bid Should Have Smaller Value"));
		}
	}

	private string GetSessionValue(string key)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

		return res == null ? "" : res.Value;
	}

    private long GetAccountId(bool throwException = true)
    {
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            if (throwException)
                throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));

            return -1;
        }

		return long.Parse(accountId);
	}
}