using System.ServiceModel;
using WorkingService.AppService.Services;

namespace WorkingService.AppService.Interfaces
{
    [ServiceContract]
    public interface IWorkingServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateStatistics(StatisticsService service);
    }
}
