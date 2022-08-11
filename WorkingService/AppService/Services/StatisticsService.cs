using System.Runtime.Serialization;
using WorkingService.AppService.Interfaces;

namespace WorkingService.AppService.Services
{
    [DataContract]
    public class StatisticsService : IStatisticsService
    {
        [DataMember]
        public int SuccessTacts { get; set; }

        [DataMember]
        public int ErrorTacts { get; set; }

        [DataMember]
        public int AllTacts { get; set; }
    }
}
