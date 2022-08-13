using System;
using System.Text;
using WorkingClient.WorkingServiceReference;

namespace WorkingClient.AppClient.Handlers
{
    public class CallbackHandler : IWorkingServiceCallback
    {
        public void UpdateStatistics(StatisticsService service)
        {
            Console.Clear();
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Обновление по статистике выполнения скрипта");
            stringBuilder.AppendLine($"Всего     тактов: {service.AllTacts}");
            stringBuilder.AppendLine($"Успешных  тактов: {service.SuccessTacts}");
            stringBuilder.AppendLine($"Ошибочных тактов: {service.ErrorTacts}");
            Console.WriteLine(stringBuilder);
        }
    }
}
