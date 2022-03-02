using System.Text;
using System.Text.Json;

class Program
{
    private static Mutex mutex = new Mutex();
    private static Random random = new Random();
    private static string numbersInFile_S1 = "Step_1";
    private static string numbersInFile_S2 = "Step_2";
    private static string numbersInFile_S3 = "Step_3";
    private static List<int> numbers = new List<int>();
    private static bool IsPrime(int n)
    {
        int a = 0;
        for (int i = 1; i <= n; i++)
        {
            if (n % i == 0)
            {
                a++;
            }
        }
        if (a == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static void Step_1()
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            int a = random.Next(0, 100);
            numbers.Add(a);
            if (IsPrime(a) == true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" " + a + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.Write(" " + a + " ");
            }
        }
        try
        {
            string json = JsonSerializer.Serialize(numbers);
            File.WriteAllText(numbersInFile_S1, json);
        }
        catch (Exception)
        {

        }
    }
    private static void Step_2()
    {
        string json = File.ReadAllText(numbersInFile_S1, Encoding.UTF8);
        List<int> numbers = JsonSerializer.Deserialize<List<int>>(json);
        foreach (var item in numbers)
        {
            if (IsPrime(item) == true)
            {
                using (StreamWriter writer = new StreamWriter(numbersInFile_S2, true))
                {
                    writer.WriteLine(item);
                }
            }
        }


    }
    private static void Step_3()
    {
        string text = null;
        if (File.Exists(numbersInFile_S2))
        {
            using (StreamReader reader = new StreamReader(numbersInFile_S2, true))
            {
                text = reader.ReadToEnd();
            }
            char[] separator = new char[] { '\n', '\r', ' ' };
            List<string> tempNambers = text.Split(separator, StringSplitOptions.RemoveEmptyEntries).ToList();
            //numbers = tempNambers.Select(int.Parse).ToList();
            foreach (var item in tempNambers)
            {
                if (item.Substring(item.Length - 1) == "7")
                {
                    using (StreamWriter writer = new StreamWriter(numbersInFile_S3, true))
                    {
                        writer.Write(" " + item + " ");
                    }
                }
            }
        }

    }
    private static void Stats()
    {
        if (File.Exists(numbersInFile_S3))
        {
            string text = File.ReadAllText(numbersInFile_S3);
            Console.WriteLine(text);
        }
        else
        {
            Console.WriteLine("Empty :(");
        }
    }
    private static void Start()
    {
        mutex.WaitOne();
        Thread t1 = new Thread(Step_1);
        t1.Start();
        t1.Join();
        Thread t2 = new Thread(Step_2);
        t2.Start();
        t2.Join();
        Thread t3 = new Thread(Step_3);
        t3.Start();
        t3.Join();
        mutex.ReleaseMutex();
    }

    static void Main(string[] args)
    {
        int input = 0;
        do
        {
            Console.Write("\n1)Start searching\n2)Show Stats\n3)Exit\nEnter: ");
            input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    {
                        if (File.Exists(numbersInFile_S2))
                        {
                            File.Delete(numbersInFile_S2);
                        }
                        if (File.Exists(numbersInFile_S3))
                        {
                            File.Delete(numbersInFile_S3);
                        }
                        Console.Clear();
                        Console.Write("How Many Thread do\nEnter: ");
                        int thr = int.Parse(Console.ReadLine());
                        for (int i = 0; i < thr; i++)
                        {
                            mutex.WaitOne();
                            Thread thread = new Thread(Start);
                            thread.Start();
                            mutex.ReleaseMutex();
                        }
                        Thread.Sleep(400);
                        break;
                    }
                case 2: { Console.Clear(); Stats(); break; }
                case 3: { Console.Clear(); return; break; }
                default: { Console.Clear(); Console.WriteLine("Enter try options!!!"); break; }
            }
        } while (true);

    }
}