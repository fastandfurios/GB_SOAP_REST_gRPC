using ChatService.Interfaces.Services;
using ChatService.Models;

namespace ChatService.Services
{
    public class MessageService : IMessageService
    {
        private int _counter = 1000;
        private Dictionary<int, Client> _clients = new();   

        public int AddClient(string name)
        {
            lock (_clients)
            {
                _clients[++_counter] = new Client
                {
                    Id = _counter,
                    Name = name
                };
            }

            return _counter;  
        }
    }
}
