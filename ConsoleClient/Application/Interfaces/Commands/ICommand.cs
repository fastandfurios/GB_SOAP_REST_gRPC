namespace ConsoleClient.Application.Interfaces.Commands
{
    public interface ICommand<out T>
    {
        T Execute(object parameter);
    }
}
