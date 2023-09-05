using System.Web;
using GalaxyTaxi.Api.Api.Models;
using GalaxyTaxi.Api.Database.Models;

namespace GalaxyTaxi.Api.Helpers;

public static class VrpHelper
{
    
    public static async Task<long[,]> GenerateTimeMatrix(Address officeAddress, List<Address> employeeAddresses, string apiKey)
    {
        var addresses = new List<string> { $"{officeAddress.Latitude},{officeAddress.Longitude}" };
        addresses.AddRange(employeeAddresses.Select(a => $"{a.Latitude},{a.Longitude}"));
        var addressQuery =  string.Join("|",addresses);
        
        var builder = new UriBuilder("https://maps.googleapis.com/maps/api/distancematrix/json");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["destinations"] = addressQuery;
        query["origins"] = addressQuery;
        query["units"] = "metric";
        query["key"] = apiKey;
        builder.Query = query.ToString();
        
        var client = new HttpClient();


        using var response = await client.GetAsync(builder.ToString());
        response.EnsureSuccessStatusCode();
        var distanceMatrix = await response.Content.ReadFromJsonAsync<MapsDistanceMatrixResponse>();
        if (distanceMatrix == null)
        {
            return new long[0,0];
        }
        
        var graph = new long[distanceMatrix.rows!.Count, distanceMatrix.rows.Count];
        for (var i = 0; i < distanceMatrix.rows.Count; i++)
        {
            for (var j = 0; j < distanceMatrix.rows.Count; j++)
            {
                graph[i, j] = distanceMatrix.rows[i].elements![j].duration!.value;
            }
        }
        return graph;
    }

    public static async Task<IEnumerable<Journey>> GenerateJourneysForEmployeesOfficeToHome(long companyId, Address officeAddress, List<Employee> companyEmployeesWithoutJourneys, long[,] timeMatrix)
    {
        for (var i = 0; i < timeMatrix.GetLength(0); i++)
        {
            timeMatrix[0, i] = 0;
        }

        var timeWindows = new long[timeMatrix.GetLength(0), timeMatrix.GetLength(1)];

        var data = VrpDataModel(timeMatrix,timeWindows);
        throw new NotImplementedException();
    }

    public static async Task<IEnumerable<Journey>> GenerateJourneysForEmployeesHomeToOffice(long companyId, Address officeAddress, List<Employee> companyEmployeesWithoutJourneys, long[,] timeMatrix)
    {
        for (var i = 0; i < timeMatrix.GetLength(0); i++)
        {
            timeMatrix[i, 0] = 0;
        }
        
        throw new NotImplementedException();
    }
    private static List<Journey> GenerateJourneys()
    {
        throw new NotImplementedException();
    }
}