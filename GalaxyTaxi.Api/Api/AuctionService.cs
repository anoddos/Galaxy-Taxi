using System.Text.Encodings.Web;
using System.Web;
using GalaxyTaxi.Api.Api.Models;
using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Api.Helpers;
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
using Newtonsoft.Json.Linq;
using ProtoBuf.Grpc;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace GalaxyTaxi.Api.Api;

public class AuctionService : IAuctionService
{
    private readonly Db _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;
    public AuctionService(Db db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
        _config = configuration;
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
        var accountId = GetAccountId();

        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);

        var auctions = await _db.Auctions.Include(x => x.Bids)
            .Where(x => ((loggedInAs == AccountType.CustomerCompany && x.CustomerCompany.AccountId == accountId)
                         || (loggedInAs == AccountType.VendorCompany && AccountIsVerified(accountId) && (x.CurrentWinnerId == null || x.Bids.Any(xx => xx.AccountId == accountId)))
                         || loggedInAs == AccountType.Admin)
                        && (filter.Status == ActionStatus.All || (filter.Status == ActionStatus.Active && x.CurrentWinnerId == null) || (filter.Status == ActionStatus.Finished && x.CurrentWinnerId != null))
                        && (filter.WonByMe == false || x.CurrentWinnerId == accountId))
            .Skip(filter.PageIndex * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => new AuctionInfo
            {
                Id = x.Id,
                Amount = x.Amount,
                EndTime = x.EndTime,
                Bids = x.Bids.Select(xx => new BidInfo
                {
                    Id = xx.Id
                }),
                CurrentWinner = x.CurrentWinnerId == null
                    ? null
                    : new VendorCompanyInfo
                    {
                        Id = (long)x.CurrentWinnerId,
                        Name = x.CurrentWinner!.Name.Substring(0,10)
                    },
                JourneyInfo = new JourneyInfo
                {
                    Id = x.Journey.Id,
                    IsOfficeDest = x.Journey.IsOfficeDest,
                    Office = new OfficeInfo
                    {
                        Address = new AddressInfo
                        {
                            Id = x.Journey.Office.Address.Id,
                            Name = x.Journey.Office.Address.Name,
                            Latitude = x.Journey.Office.Address.Latitude,
                            Longitude = x.Journey.Office.Address.Longitude
                        }
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
                        }
                    })
                }
            }).ToListAsync();

        return new GetAuctionsResponse { Auctions = auctions };
    }

    private bool AccountIsVerified(long accountId)
    {
        return  _db.Accounts.Any(x => x.Id == accountId && x.Status == AccountStatus.Verified);
    }

    public async Task<GetAuctionsCountResponse> GetAuctionCount(AuctionsFilter filter)
    {
        var accountId = GetAccountId();

        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);

        var count = _db.Auctions.Count(x => ((loggedInAs == AccountType.CustomerCompany && x.CustomerCompany.AccountId == accountId)
                                             || (loggedInAs == AccountType.VendorCompany &&  AccountIsVerified(accountId) && (x.CurrentWinnerId == null || x.Bids.Any(xx => xx.AccountId == accountId)))
                                             || loggedInAs == AccountType.Admin)
                                            && (filter.Status == ActionStatus.All || (filter.Status == ActionStatus.Active && x.CurrentWinnerId == null) || (filter.Status == ActionStatus.Finished && x.CurrentWinnerId != null))
                                            && (filter.WonByMe == false || x.CurrentWinnerId == accountId));

        return await Task.FromResult(new GetAuctionsCountResponse { TotalCount = count });
    }

    public async Task<GetSingleAuctionsResponse> GetSingleAuction(IdFilter filter)
    {
        var accountId = GetAccountId();

        var auction = await _db.Auctions.Include(x => x.Bids)
            .Where(x => x.Id == filter.Id && x.CustomerCompany.AccountId == accountId || x.Bids.Any(xx => xx.AccountId == accountId))
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
                    IsOfficeDest = x.Journey.IsOfficeDest,
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
                        },
                        StopOrder = xx.StopOrder
                    })
                }
            }).SingleAsync();

        return new GetSingleAuctionsResponse { Auction = auction };
    }

    public async Task<GenerateAuctionsResponse> GenerateAuctionsForCompany(CallContext context = default)
    {
        var companyId = GetCompanyId();
        var totalCost = 0.0;
        
        var journeys = await GenerateJourneysForCompany(companyId);
        
        var maxAmountPerEmployee = (await _db.CustomerCompanies.SingleAsync(x => x.Id == companyId)).MaxAmountPerEmployee;
        var dayCountPerAuction = (await _db.Subscriptions.SingleAsync(x => x.SubscriptionStatus == SubscriptionStatus.Active)).SubscriptionPlanTypeId == SubscriptionPlanType.Annual ? 7 : 5;
        var nextRelevantMonday = GetNextRelevantMonday();
        
        foreach (var journey in journeys)
        {
            var newAuction = new Auction
            {
                Amount = maxAmountPerEmployee * dayCountPerAuction * journey.Stops.Count,
                StartTime = DateTime.UtcNow,
                CustomerCompanyId = companyId,
                JourneyId = journey.Id,
                EndTime = nextRelevantMonday.ToUniversalTime(),
                FromDate = nextRelevantMonday,
                ToDate = nextRelevantMonday.AddDays(6)
            };
            
            totalCost += newAuction.Amount;
            
            await _db.Auctions.AddAsync(newAuction);
        }

        return new GenerateAuctionsResponse
        {
            GeneratedAuctionCount = journeys.Count,
            GeneratedAuctionTotalCost = totalCost
        };
    }

    public async Task SaveEvaluation(SaveEvaluationRequest evaluation, CallContext context = default)
    {
        var auction = await _db.Auctions.SingleAsync(x => x.Id == evaluation.Id);

        if (auction.ToDate.AddDays(2) < DateTime.Today)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Too Late For Evaluation"));
        }
        
        auction.Comment = evaluation.Comment;
        //auction.FeedbackId = evaluation.Evaluation;

        _db.Auctions.Update(auction);
        await _db.SaveChangesAsync();
    }

    private async Task<List<Journey>> GenerateJourneysForCompany(long companyId)
    {
        var offices = await _db.Offices.Include(x => x.Address).Where(x => x.CustomerCompanyId == companyId)
            .ToListAsync();

        var journeys = new List<Journey>();

        foreach (var office in offices)
        {
            var officeEmployeesWithoutJourneys = await _db.Employees
                .Include(x => x.Addresses)
                .ThenInclude(x => x.Address)
                .Where(x => x.CustomerCompanyId == companyId && !x.HasActiveJourney && x.Addresses.Any(xx => xx.IsActive && xx.Address.IsDetected)).ToListAsync();

            var newJourneys = await GenerateJourneysForEmployees(companyId, office, officeEmployeesWithoutJourneys);
    
            foreach (var newJourney in newJourneys)
            {
                await _db.Journeys.AddAsync(newJourney);
            }
            
            journeys.AddRange(newJourneys);
        }

        await _db.SaveChangesAsync();
        
        return journeys;
    }
    
    private static DateTime GetNextRelevantMonday()
    {
        var today = DateTime.Today;
        return today.DayOfWeek switch
        {
            DayOfWeek.Saturday => today.AddDays(9),
            DayOfWeek.Sunday => today.AddDays(8),
            _ => today.AddDays(8 - (int)today.DayOfWeek),
        };
    }

    private async Task<List<Journey>> GenerateJourneysForEmployees(long companyId, Office office, List<Employee> companyEmployeesWithoutJourneys)
    {
        var result = new List<Journey>();

        var supportTwoWayJourneys = _db.CustomerCompanies.Single(x => x.Id == companyId).SupportTwoWayJourneys;
        var timeMatrix = await VrpHelper.GenerateTimeMatrix(office.Address,
            companyEmployeesWithoutJourneys.Select(e => e.Addresses.First().Address).ToList(),
            _config.GetValue<string>("GoogleMapsKey"));
        
        result.AddRange(await VrpHelper.GenerateJourneysForEmployeesHomeToOffice(companyId, office, companyEmployeesWithoutJourneys, timeMatrix));
        
        if(supportTwoWayJourneys)
            result.AddRange(await VrpHelper.GenerateJourneysForEmployeesOfficeToHome(companyId, office, companyEmployeesWithoutJourneys, timeMatrix));
        
        return result;
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

    private long GetAccountId()
    {
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        return long.Parse(accountId);
    }
    
    private long GetCompanyId()
    {
        var companyId = GetSessionValue(AuthenticationKey.CompanyId);

        if (string.IsNullOrWhiteSpace(companyId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        return long.Parse(companyId);
    }
}