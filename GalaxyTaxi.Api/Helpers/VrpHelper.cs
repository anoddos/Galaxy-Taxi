using System.Web;
using System.Collections.Generic;
using GalaxyTaxi.Api.Api.Models;
using GalaxyTaxi.Api.Database.Models;
using Google.OrTools.ConstraintSolver;
using Duration = Google.Protobuf.WellKnownTypes.Duration;

namespace GalaxyTaxi.Api.Helpers;

public static class VrpHelper
{
    
    public static async Task<long[,]> GenerateTimeMatrix(Address officeAddress, List<Address> employeeAddresses, string apiKey)
    {
        var addresses = new List<string> { $"{officeAddress.Latitude},{officeAddress.Longitude}" };
        addresses.AddRange(employeeAddresses.Select(a => $"{a.Latitude},{a.Longitude}"));
        var graph = new long[addresses.Count, addresses.Count];
        for (var i = 0; i < addresses.Count; i++)
        {
            for (var j = 0; j < addresses.Count; j++)
            {
                graph[i, j] = long.MaxValue;
            }
        }

        for (int chunkO = 0; chunkO < Math.Ceiling((double)addresses.Count/10); chunkO++)
        {
            var originStart = chunkO * 10;
            var origins = addresses.GetRange(originStart, Math.Min(10, addresses.Count-originStart));
            for (int chunkD = 0; chunkD < Math.Ceiling((double)addresses.Count / 10); chunkD++)
            {
                var destinationStart = chunkD * 10;
                var destinations = addresses.GetRange(destinationStart, Math.Min(10, addresses.Count-destinationStart));
                graph = await GenerateTimeMatrixByChunks(origins, destinations, apiKey, graph, originStart, destinationStart);
            }
        }
        
        return graph;
    }

    private static async Task<long[,]> GenerateTimeMatrixByChunks(IEnumerable<string> origins, IEnumerable<string> destinations, string apiKey, long[,] graph, int offsetOrigin,int offsetDestination)
    {
        var originsQuery =  string.Join("|",origins);
        var destinationsQuery =  string.Join("|",destinations);
        
        var builder = new UriBuilder("https://maps.googleapis.com/maps/api/distancematrix/json");
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["destinations"] = originsQuery;
        query["origins"] = destinationsQuery;
        query["units"] = "metric";
        query["key"] = apiKey;
        builder.Query = query.ToString();
        
        var client = new HttpClient();


        using var response = await client.GetAsync(builder.ToString());
        response.EnsureSuccessStatusCode();
        var distanceMatrix = await response.Content.ReadFromJsonAsync<MapsDistanceMatrixResponse>();
        
        if (distanceMatrix == null)
        {
            return graph;
        }
        
        
        for (var i = 0; i < distanceMatrix.rows.Count; i++)
        {
            for (var j = 0; j < distanceMatrix.rows[i].elements.Count; j++)
            {
                graph[offsetOrigin + j, offsetDestination + i] = distanceMatrix.rows[i].elements![j].duration!.value;
            }
        }

        return graph;
    } 

    public static async Task<IEnumerable<Journey>> GenerateJourneysForEmployeesOfficeToHome(long companyId, Office office, List<Employee> companyEmployeesWithoutJourneys, long[,] timeMatrix)
    {
        for (var i = 0; i < timeMatrix.GetLength(0); i++)
        {
            timeMatrix[i, 0] = 0;
        }

        var timeWindows = new long[timeMatrix.GetLength(0), timeMatrix.GetLength(1)];

        var data =new VrpDataModel(timeMatrix,timeWindows,timeMatrix.GetLength(0),0, 3);
        return GenerateJourneys(data, false, office, companyEmployeesWithoutJourneys, companyId);
    }

    public static async Task<IEnumerable<Journey>> GenerateJourneysForEmployeesHomeToOffice(long companyId, Office office, List<Employee> companyEmployeesWithoutJourneys, long[,] timeMatrix)
    {
        for (var i = 0; i < timeMatrix.GetLength(0); i++)
        {
            timeMatrix[0, i] = 0;
        }
        var timeWindows = new long[timeMatrix.GetLength(0),2];
        for (int i = 0; i < timeWindows.GetLength(0); i++)
        {
            timeWindows[i, 0] = 0;
            timeWindows[i, 1] = 72000;
        }

        var data =new VrpDataModel(timeMatrix,timeWindows,timeMatrix.GetLength(0),0, 3);
        return GenerateJourneys(data, true, office, companyEmployeesWithoutJourneys, companyId);
    }
    private static List<Journey> GenerateJourneys(VrpDataModel data, bool isOfficeDest, Office office, List<Employee> employees, long companyId)
    {
        RoutingIndexManager manager =
            new RoutingIndexManager(data.TimeMatrix.GetLength(0), data.VehicleNumber, data.Depot);

        // Create Routing Model.
        RoutingModel routing = new RoutingModel(manager);

        // Create and register a transit callback.
        int transitCallbackIndex = routing.RegisterTransitCallback((long fromIndex, long toIndex) =>
        {
            // Convert from routing variable Index to time
            // matrix NodeIndex.
            var fromNode = manager.IndexToNode(fromIndex);
            var toNode = manager.IndexToNode(toIndex);
            return data.TimeMatrix[fromNode, toNode];
        });

        // Define cost of each arc.
        routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

        
        // Add Time constraint.
        routing.AddDimension(transitCallbackIndex, // transit callback
            300,                   // allow waiting time
            Math.Max(7200, data.TimeMatrix.Cast<long>().Max()), // vehicle maximum capacities
            false,                // start cumul to zero
            "Time");
        
        RoutingDimension timeDimension = routing.GetMutableDimension("Time");
        // Add time window constraints for each location except depot.
        for (int i = 1; i < data.TimeWindows.GetLength(0); ++i)
        {
            long index = manager.NodeToIndex(i);
            timeDimension.CumulVar(index).SetRange(data.TimeWindows[i, 0], data.TimeWindows[i, 1]);
        }
        // Add time window constraints for each vehicle start node.
        for (int i = 0; i < data.VehicleNumber; ++i)
        {
            long index = routing.Start(i);
            timeDimension.CumulVar(index).SetRange(data.TimeWindows[0, 0], data.TimeWindows[0, 1]);
        }
        for (int i = 0; i < data.VehicleNumber; ++i)
        {
            routing.AddVariableMinimizedByFinalizer(timeDimension.CumulVar(routing.Start(i)));
            routing.AddVariableMinimizedByFinalizer(timeDimension.CumulVar(routing.End(i)));
        }
        
        int demandCallbackIndex = routing.RegisterUnaryTransitCallback((long fromIndex) => manager.IndexToNode(fromIndex) == 0 ? 0:1);
        routing.AddDimensionWithVehicleCapacity(demandCallbackIndex, 0, // null capacity slack
            data.VehicleCapacities, // vehicle maximum capacities
            true,                   // start cumul to zero
            "Capacity");
        RoutingSearchParameters searchParameters =
            operations_research_constraint_solver.DefaultRoutingSearchParameters();
        
        searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
        searchParameters.TimeLimit = new Duration { Seconds = 1 };
        // Solve the problem.
        Assignment solution = routing.SolveWithParameters(searchParameters);

        return GetJourneysFromSolution(solution, routing, data, manager, isOfficeDest, office, employees, companyId);
    }

    private static List<Journey> GetJourneysFromSolution(Assignment solution, RoutingModel routing, VrpDataModel data, RoutingIndexManager manager, bool isOfficeDest, Office office, List<Employee> employees, long companyId)
    {
        var answer = new List<Journey>();
        
        RoutingDimension timeDimension = routing.GetMutableDimension("Time");
        for (int i = 0; i < data.VehicleNumber; i++)
        {
            var routeForVehicle = new List<Tuple<int, long>>();
            var index = routing.Start(i);
            while (routing.IsEnd(index) == false)
            {
                var timeVar = timeDimension.CumulVar(index);
                routeForVehicle.Add(new Tuple<int, long>(manager.IndexToNode(index) - 1, solution.Min(timeVar)));

                Console.Write("{0} Time({1},{2}) -> ", manager.IndexToNode(index), solution.Min(timeVar),
                    solution.Max(timeVar));
                index = solution.Value(routing.NextVar(index));
            }
            var endTimeVar = timeDimension.CumulVar(index);
            var totalTime = solution.Min(endTimeVar);
            routeForVehicle.RemoveAt(0);

            if (routeForVehicle.Count == 0)
            {
                continue;
            }
            
            var journey = new Journey{CustomerCompanyId = companyId, IsOfficeDest = isOfficeDest, Office = office, Stops = new List<Stop>()};

            for (int j = 0; j < routeForVehicle.Count; j++)
            {
                journey.Stops.Add(new Stop{
                    EmployeeAddress = employees[routeForVehicle[j].Item1].Addresses.First(),
                    StopOrder = j + 1,
                    PickupTime = isOfficeDest ? 
                        new TimeSpan((long)office.WorkingStartTime.TotalSeconds - totalTime + routeForVehicle[j].Item2) :
                        new TimeSpan((long)office.WorkingEndTime.TotalSeconds + routeForVehicle[j].Item2)
                });
            }

            answer.Add(journey);
        }
        return answer;
    }
}