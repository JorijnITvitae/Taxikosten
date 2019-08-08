using System;

namespace Taxikosten
{
    class Program
    {
        private static double GetDistance(string prompt)
        {
            Console.Write(prompt);
            try
            {
                return Math.Abs(Convert.ToDouble(Console.ReadLine()));
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR");
                return -1.0;
            }
        }

        private static int GetDay(string prompt)
        {
            Console.Write(prompt);
            string day = Console.ReadLine().ToLower();
            switch (day)
            {
                case ("monday"): return 1;
                case ("tuesday"): return 2;
                case ("wednesday"): return 3;
                case ("thursday"): return 4;
                case ("friday"): return 5;
                case ("saturday"): return 6;
                case ("sunday"): return 7;

                default:
                    Console.WriteLine("ERROR");
                    return -1;
            }
        }

        private static int GetHour(string prompt)
        {
            Console.Write(prompt);
            try
            {
                int hour = Math.Abs(Convert.ToInt32(Console.ReadLine()));
                if (hour < 24) return hour;
            }
            catch { }

            Console.WriteLine("ERROR");
            return -1;
        }

        private static int GetMinute(string prompt)
        {
            Console.Write(prompt);
            try
            {
                int hour = Math.Abs(Convert.ToInt32(Console.ReadLine()));
                if (hour < 60) return hour;
            }
            catch { }

            Console.WriteLine("ERROR");
            return -1;
        }

        static void Main(string[] args)
        {
            double km = GetDistance("KM = ");
            if (km < 0) return;

            int day = GetDay("Day of the week = ");
            if (day < 0) return;

            int startHour = GetHour("Hour of departure = ");
            if (startHour < 0) return;
            
            int startMinute = GetMinute("Minute of departure = ");
            if (startMinute < 0) return;

            int endHour = GetHour("Hour of arrival = ");
            if (endHour < 0) return;
            
            int endMinute = GetMinute("Minute of arrival = ");
            if (endMinute < 0) return;

            DateTime startTime = new DateTime(2000, 1, 1, startHour, startMinute, 0);
            DateTime endTime = new DateTime(2000, 1, 1, endHour, endMinute, 0);
            TimeSpan duration = endTime - startTime;
            Console.Write("Duration = ");
            Console.WriteLine(duration);
            if (startTime > endTime)
            {
                Console.WriteLine("ERROR - START AFTER END");
                return;
            }

            DateTime eightAM = new DateTime(2000, 1, 1, 8, 0, 0);
            DateTime sixPM = new DateTime(2000, 1, 1, 18, 0, 0);

            const double tarifCheap = 0.25;
            const double tarifExpensive = 0.45;
            double costTime = 0.0;

            if (startTime >= sixPM || endTime < eightAM)
            {
                costTime = duration.TotalMinutes * tarifExpensive;
            }
            else if (startTime >= eightAM && endTime < sixPM)
            {
                costTime = duration.TotalMinutes * tarifCheap;
            }
            else if (startTime < eightAM)
            {
                double costBeforeEight = (eightAM - startTime).TotalMinutes * tarifExpensive;

                if (endTime < sixPM)
                {
                    double costBeforeSix = (endTime - eightAM).TotalMinutes * tarifCheap;
                    costTime = costBeforeEight + costBeforeSix;
                }
                else
                {
                    double costBeforeSix = (sixPM - eightAM).TotalMinutes * tarifCheap;
                    double costAfterSix = (endTime - sixPM).TotalMinutes * tarifExpensive;
                    costTime = costBeforeEight + costBeforeSix + costAfterSix;
                }
            }

            const double tarifDistance = 1.0;
            double costDistance = km * tarifDistance;

            double cost = costTime + costDistance;

            if ((day == 5 && startTime.Hour >= 22) || (day == 6 || day == 7) || (day == 1 && startTime.Hour < 7))
            {
                const double extraCost = 0.15;
                cost *= (1.0 + extraCost);
            }

            cost = Math.Round(cost, 2, MidpointRounding.AwayFromZero);

            Console.Write("Cost = $");
            Console.WriteLine(cost);
        }
    }
}
