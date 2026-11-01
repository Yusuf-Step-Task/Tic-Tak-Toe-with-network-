using System.Net.Sockets;
using System.IO;
public class Program
{
    public static async Task Main(string[] args)
    {
        TcpClient client = new TcpClient();
        await client.ConnectAsync("127.0.0.1", 5000);
        Console.WriteLine("Connected to Server!");
        StreamReader reader = new StreamReader(client.GetStream());
        StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true }; 
        string player = await reader.ReadLineAsync();
        Console.WriteLine($"You are Player {player.Split('|')[1]}");
        while (true)
        {
            string msg = await reader.ReadLineAsync();
            if (msg == null)
            {
                Console.WriteLine("Server disconnected");
                break;
            }
            string[] parts = msg.Split('|');
            string command = parts[0];
            if (command == "Board")
            {
                string[] cells = parts[1].Split(',');
                Console.Clear();
                for (int i = 0; i < 9; i++)
                {
                    Console.Write($"{cells[i]} | ");
                    if (i == 2 || i == 5 || i == 8) 
                    {
                        Console.WriteLine("\n");
                    }
                }
            }
            else if (command == "Turn")
            {
                Console.Write("Enter: ");
                string choise = Console.ReadLine();
                await writer.WriteLineAsync($"Move|{choise}");
            }
            else if (command == "Wait")
            {
                Console.WriteLine($"Reqib ({parts[1]}) playing");
            }
            else if (command == "Win")
            {
                Console.WriteLine($"Qalib: {parts[1]}");
            }
            else if (command == "Draw")
            {
                Console.WriteLine("Draw");
            }
            else if (command == "Error")
            {
                Console.WriteLine($"Xeta: {parts[1]}");
            }
            else if (command == "Exit")
            {
                Console.WriteLine("Game Ending......");
                break;
            }
        }
        client.Close();
    }
}