// Disable the warning.
#pragma warning disable SYSLIB0011

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        // static long[] Sum_Global = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        static long[] Sum_Global = {};
        static List<Thread> All_Thread = new List<Thread>();

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("../Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            try 
            {
                Data_Global = (byte[]) bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void sum(int threadIndex, int start, int stop)
        {
            Console.WriteLine("Thread {0}, Start {1} -> {2}", threadIndex, start, stop);
            int sum = 0;
            for (int i = start; i < stop; i++)
            {
                if (Data_Global[i] % 2 == 0)
                {
                    sum -= Data_Global[i];
                }
                else if (Data_Global[i] % 3 == 0)
                {
                    sum += (Data_Global[i] * 2);
                }
                else if (Data_Global[i] % 5 == 0)
                {
                    sum += (Data_Global[i] / 2);
                }
                else if (Data_Global[i] % 7 == 0)
                {
                    sum += (Data_Global[i] / 3);
                }
                Data_Global[i] = 0;
            }
            Sum_Global[threadIndex] = sum;
        }
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int y;

            /* Read data from file */
            // Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                // Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            // Console.Write("\n\nWorking...");

            int NThread = int.Parse(args[0]);
            int X = 1000000000 / NThread;
            Sum_Global = new long[NThread];

            sw.Start();
            
            for(int l= 0; l < NThread ; l++) {
                int nThread = All_Thread.Count();
                Thread th = new Thread(() => {sum(nThread, X * nThread, X*(nThread+1)); });
                th.Start();
                All_Thread.Add(th);
            }

            foreach(Thread th in All_Thread) {
                th.Join();
            }
      
            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global.Sum());
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}