namespace GalaxyTaxi.Api.Api.Models;

public class VrpDataModel
{
        public long[,] TimeMatrix;
        public long[,] TimeWindows;
        public int VehicleNumber;
        public int Depot;

        public VrpDataModel(long[,] timeMatrix, long[,] timeWindows, int vehicleNumber, int depot)
        {
            VehicleNumber = vehicleNumber;
            Depot = depot;
            TimeMatrix = timeMatrix;
            TimeWindows = timeWindows;
        }
}