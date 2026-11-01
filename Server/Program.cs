using System.Net;
using System.Net.Sockets;
public class Program
{
    public static bool CheckWinner(string[] X)
    {
        //int a = 0;
        //if ( a == 0)
        //    return true;

        // Horizontal
        if (X[0] == X[1] && X[1] == X[2]) return true;
        if (X[3] == X[4] && X[4] == X[5]) return true;
        if (X[6] == X[7] && X[7] == X[8]) return true;

        // Vertical
        if (X[0] == X[3] && X[3] == X[6]) return true;
        if (X[1] == X[4] && X[4] == X[7]) return true;
        if (X[2] == X[5] && X[5] == X[8]) return true;

        // Diagonal
        if (X[0] == X[4] && X[4] == X[8]) return true;
        if (X[2] == X[4] && X[4] == X[6]) return true;

        return false;
    }
    //--------------------------

    public static async Task Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();

        Console.WriteLine("Server is waiting...");

        TcpClient clientX = await server.AcceptTcpClientAsync();
        Console.WriteLine("Player X connected!");

        TcpClient clientO = await server.AcceptTcpClientAsync();
        Console.WriteLine("Player O connected!");

        StreamReader readerX = new StreamReader(clientX.GetStream());
        StreamWriter writerX = new StreamWriter(clientX.GetStream()) { AutoFlush = true };

        StreamReader readerO = new StreamReader(clientO.GetStream());
        StreamWriter writerO = new StreamWriter(clientO.GetStream()) { AutoFlush = true };

        await writerX.WriteLineAsync("Role|X");
        await writerO.WriteLineAsync("Role|O");

        int xqazan = 0;
        int oqazan = 0;
        while (true)
        {

            bool Reset = false;
            bool isGameOver = true;
            string[] X = new string[9];
            X[0] = "X0";
            X[1] = "X1";
            X[2] = "X2";
            X[3] = "X3";
            X[4] = "X4";
            X[5] = "X5";
            X[6] = "X6";
            X[7] = "X7";
            X[8] = "X8";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("X and O game Starting");
            Console.WriteLine("Information.\nIlk once X baslayacaq her kesin sadece 1 oyun gedis haqqi olajaq \nOyun basladi\n");
            Console.ResetColor();
            bool[] Xana = new bool[9];
            Xana[0] = true;
            Xana[1] = true;
            Xana[2] = true;
            Xana[3] = true;
            Xana[4] = true;
            Xana[5] = true;
            Xana[6] = true;
            Xana[7] = true;
            Xana[8] = true;
            void Board()
            {
                for (int a = 0; a < 3; a++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"{X[i]} | ");
                        Console.ResetColor();
                        if (i == 2 || i == 5)
                        {
                            Console.WriteLine("\n");
                        }
                    }
                    break;
                }
            }
            //--------------------------
            Board();
            bool hesab = true;
            string Sira = "X";
            int say = 0;
            while (isGameOver)
            {
                StreamReader currentReader;
                StreamWriter currentWriter;
                string board = "Board|" + string.Join(",", X);
                await writerX.WriteLineAsync(board);
                await writerO.WriteLineAsync(board);
                await writerX.WriteLineAsync(hesab ? "Turn|X" : "Wait|O");
                await writerO.WriteLineAsync(hesab ? "Wait|X" : "Turn|O");



                if (CheckWinner(X))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{Sira} tebriler ayqa qalibsiniz .....");
                    if (Sira == "X")
                        xqazan++;
                    else
                        oqazan++;

                    Console.WriteLine($"Score Board : X: {xqazan}  -  O: {oqazan}");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("1. New Game\n2. Finish");
                    Console.Write("Choise: ");
                    Console.ResetColor();
                    string DEVAM = Console.ReadLine();
                    int s = Convert.ToInt32(DEVAM);
                    if (s == 1)
                    {
                        Console.WriteLine("New Game Starting......");
                        //Sira = "X";
                        //Board();
                        Reset = true;
                        break;
                    }
                    else if (s == 2)
                    {
                        Console.WriteLine("Game Finishing.........");
                        isGameOver = false;
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: 1 or 2 daxil ede bilersiniz !!!");
                        Console.ResetColor();
                    }
                }
                try
                {
                    if (say >= 9)
                    {
                        Console.WriteLine("Draw");
                        break;
                    }
                    if (hesab==true)
                    {
                        Sira = "X";
                    }
                    else
                    {
                        Sira = "O";
                    }
                    Console.WriteLine($"\nSecim sirasi -> {Sira}");
                    Console.Write("X: ");
                    string Choise = Console.ReadLine();
                    if (int.TryParse(Choise, out int C))
                    {
                        if (C < 9 && C >= 0)
                        {
                            if (Xana[C] == true)
                            {
                                X[C] = Sira;
                                Console.WriteLine("Chosie Added");
                                Xana[C] = false;
                                Console.Clear();
                                Board();
                                say++;
                                if (hesab == true)
                                {
                                    hesab = false;
                                    currentReader = readerX;
                                    currentWriter = writerX;
                                }
                                else
                                {
                                    hesab = true;
                                    currentReader = readerO;
                                    currentWriter = writerO;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Secdiyiniz xana Evvelceden secilibdir !!!");
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Sadece 9 xanadaki numberlari daxil ede bilersiniz !!!");
                            Console.ResetColor();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex}");
                    Console.ResetColor();
                }
            }
            if (Reset)
                continue;
        }
    }

}