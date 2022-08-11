using System.ServiceModel;

namespace WorkingService.AppService.Interfaces
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWorkingServiceCallback))]
    public interface IWorkingService
    {
        [OperationContract(IsOneWay = true)]
        void RunScript();

        [OperationContract(IsOneWay = true)]
        void UpdateAndCompileScript(string fileName);

        [OperationContract(IsOneWay = true)]
        void CompileScript();
    }
}
