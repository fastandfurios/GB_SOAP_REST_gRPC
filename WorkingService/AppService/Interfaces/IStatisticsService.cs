namespace WorkingService.AppService.Interfaces
{
    public interface IStatisticsService
    {
        int SuccessTacts { get; set; }
        int ErrorTacts { get; set; }
        int AllTacts { get; set; }
    }
}
