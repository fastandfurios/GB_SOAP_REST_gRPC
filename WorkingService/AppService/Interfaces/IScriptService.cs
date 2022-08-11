namespace WorkingService.AppService.Interfaces
{
    public interface IScriptService
    {
        bool Compile();
        void Run();
    }
}
