using Tecan.Sila2;

namespace CashDesk.Infrastructure;

// Wrapper for server data to be used in dependency injection 
public class ServerDataWrapper
{
    public class TerminalServerData
    {
        public ServerData Data { get; set; }

        public TerminalServerData(ServerData data)
        {
            Data = data;
        }
    }

    public class BankServerData
    {
        public ServerData Data { get; set; }

        public BankServerData(ServerData data)
        {
            Data = data;
        }
    }
    
}