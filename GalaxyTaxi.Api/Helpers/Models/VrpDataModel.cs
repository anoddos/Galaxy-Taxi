namespace GalaxyTaxi.Api.Api.Models;

public class VrpDataModel
{
        public long[,] TimeMatrix;
        public long[,] TimeWindows;
        public long[] VehicleCapacities;
        public int VehicleNumber;
        public int Depot;

        public VrpDataModel(long[,] timeMatrix, long[,] timeWindows, int vehicleNumber, int depot, int capacity)
        {
            VehicleNumber = vehicleNumber;
            Depot = depot;
            TimeMatrix = timeMatrix;
            TimeWindows = timeWindows;
            VehicleCapacities = new long[vehicleNumber];
            for (int i = 0; i < vehicleNumber; i++)
            {
                VehicleCapacities[i] = capacity;
            }
        }
}