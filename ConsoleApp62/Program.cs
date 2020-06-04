using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace ConsoleApp62
{
    interface test_inter
    {
        string Indent(int count);
        string TopIndent(int count);
    }
    abstract class Library : test_inter
    {
        public abstract string Indent(int count);
        public abstract string TopIndent(int count);


        public string Name { get; set; }
        public string Adress { get; set; }

    }
    class WorkingDay : Library
    {
        public DateTime Date { get; set; }
        public int BookOut { get; set; }
        public int BookIn { get; set; }
        public override string Indent(int count)
        {
            if (count > 0)
            {
                return "".PadLeft(count);
            }
            return "";
        }
        public override string TopIndent(int count)
        {
            string a = "";
            for (int i = 0; i < count; i++)
            {
                a += "\n";
            }
            return a;
        }
    }
    class Program
    {

        static int TD = 25;
        static int UTD = 5;
        static void Main(string[] args)
        {

            string path = "FILE.json";
            List<WorkingDay> Days = ReadFile(path);
            while (true)
            {
                Console.Clear();
                Show(Days);
                var k = Console.ReadKey().Key;
                Console.Clear();
                switch (k)
                {
                    case ConsoleKey.A:
                        if (Days == null)
                        {
                            Days = new List<WorkingDay>();
                            Days.Add(CreateNewDay());
                        }
                        else
                        {
                            Days.Add(CreateNewDay());
                        }
                        break;
                    case ConsoleKey.D:
                        DelteDay(Days);
                        break;
                    case ConsoleKey.C:
                        ChangeData(Days);
                        break;
                    case ConsoleKey.Enter:
                        return;
                        break;
                    case ConsoleKey.M:
                        MidlleBookMove(Days);
                        break;
                    case ConsoleKey.B:
                        CountOfBookMore(Days);
                        break;
                    case ConsoleKey.P:
                        PairReturned(Days);
                        break;

                }
                SaveFile(path, Days);
            }

        }
        static void Show(List<WorkingDay> a)
        {
            string LibName = "[ Lib Name ]";
            string Adress = "[  Adress  ]";
            string Date = "[   Date   ]";
            string BookOut = "[Book out]";
            string Bookin = "[Book in]";
            Library lib = new WorkingDay();

            Console.WriteLine(lib.TopIndent(UTD) + lib.Indent(TD) + LibName + Adress + Date + BookOut + Bookin);
            if (a != null && a.Count > 0)
            {
                foreach (WorkingDay Day in a)
                {
                    Console.WriteLine(lib.Indent(TD) + "[" + Day.Name + lib.Indent(LibName.Length - 2 - Day.Name.Length) + "]"
                        + "[" + Day.Adress + lib.Indent(Adress.Length - 2 - Day.Adress.Length) + "]"
                        + "[" + Day.Date.ToString("dd/MM/yyyy") + lib.Indent(Date.Length - 2 - Day.Date.Date.ToString("dd/MM/yyyy").Length) + "]"
                        + "[" + Day.BookOut + lib.Indent(BookOut.Length - 2 - Day.BookOut.ToString().Length) + "]"
                        + "[" + Day.BookIn + lib.Indent(Bookin.Length - 2 - Day.BookIn.ToString().Length) + "]");
                }
            }
            Console.WriteLine(lib.TopIndent(1) + lib.Indent(TD) + "Press [A - To add new day][D - Delete day][C - Change day]");
            Console.WriteLine(lib.Indent(TD) + "[M - The average movement of books per day for the period]");
            Console.WriteLine(lib.Indent(TD) + "[B - The number of days books were published than were returned]");
            Console.WriteLine(lib.Indent(TD) + "[P - Days when an even number of books are published and returned are odd]");
        }
        public static List<WorkingDay> ReadFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            List<WorkingDay> Ret = new List<WorkingDay>();
            Ret = JsonConvert.DeserializeObject<List<WorkingDay>>(File.ReadAllText(path));
            return Ret;
        }
        public static void SaveFile(string path, List<WorkingDay> data)
        {
            string serialize = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            if (serialize.Count() > 1)
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                }
                File.WriteAllText(path, serialize, Encoding.UTF8);
            }
        }

        public static WorkingDay CreateNewDay()
        {
            Console.Clear();
            WorkingDay Day = new WorkingDay();
            Console.WriteLine("Enter Name of library");
            Day.Name = Console.ReadLine();
            Console.WriteLine("Enter adress of library");
            Day.Adress = Console.ReadLine();
            Console.WriteLine("Enter date of day like 01.06.2020");
            Day.Date = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
            Console.WriteLine("Enter Book in count");
            Day.BookIn = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Enter Book out count");
            Day.BookOut = Convert.ToInt16(Console.ReadLine());
            return Day;
        }
        public static void DelteDay(List<WorkingDay> Days)
        {
            if (Days != null)
            {
                Console.WriteLine("Choose date of day that`s you want to delete");
                var s = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
                Days.RemoveAll(x => x.Date == s);

            }
        }
        public static void ChangeData(List<WorkingDay> Days)
        {
            Console.WriteLine("Choose date of day that`s you want to change");
            var s = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
            WorkingDay day = Days.Find(x => x.Date == s);
            if (day != null)
            {
                Console.WriteLine("Choose value of day that`s you want to change \n1)Name\n2)Adress\n3)Date like 01.06.2020\n4)Book Out\n5)Book In");
                char a = Console.ReadKey().KeyChar;
                Console.WriteLine("Choose new value");
                switch (a)
                {
                    case '1':
                        day.Name = Console.ReadLine();
                        break;
                    case '2':
                        day.Adress = Console.ReadLine();
                        break;
                    case '3':
                        day.Date = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
                        break;
                    case '4':
                        day.BookOut = Convert.ToInt16(Console.ReadLine());
                        break;
                    case '5':
                        day.BookIn = Convert.ToInt16(Console.ReadLine());
                        break;
                }
            }
        }
        //
        public static void MidlleBookMove(List<WorkingDay> Days)
        {
            Console.Clear();
            Console.WriteLine("Choose earlier date like 01.06.2020");
            var date1 = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
            Console.WriteLine("Choose later date like 01.06.2020");
            var date2 = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
            for (int i = 0; i < Days.Count; i++)
            {
                for (int k = 0; k < Days.Count; k++)
                {
                    if (Days[i].Date < Days[k].Date)
                    {
                        var Temp = Days[k];
                        Days[k] = Days[i];
                        Days[i] = Temp;
                    }
                }
            }
            float count = 0;
            float Move = 0;
            for (int i = 0; i < Days.Count; i++)
            {
                if (Days[i].Date > date1 && Days[i].Date < date2)
                {
                    Move += (Days[i].BookIn + Days[i].BookOut);
                    count++;
                }
            }
            Console.WriteLine("The average movement of books per day for the period :" + Move / count);
            Console.ReadKey();
        }
        public static void CountOfBookMore(List<WorkingDay> Days)
        {
            List<WorkingDay> Cut = new List<WorkingDay>();
            foreach (WorkingDay d in Days)
            {
                if (d.BookIn < d.BookOut)
                {
                    Cut.Add(d);
                }
            }
            Show(Cut);
            Console.ReadKey();
        }
        public static void PairReturned(List<WorkingDay> Days)
        {
            List<WorkingDay> Cut = new List<WorkingDay>();
            foreach (WorkingDay d in Days)
            {
                if (d.BookOut % 2 == 0 && d.BookIn % 2 != 0)
                {
                    Cut.Add(d);
                }
            }
            Show(Cut);
            Console.ReadKey();
        }
    }
}
