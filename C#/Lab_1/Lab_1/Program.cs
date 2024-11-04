using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Lab_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int cntThreads = 4; 
            int time = 4;
            int step = 2;

            Breaker breaker = new Breaker(time);
            Thread[] threads = new Thread[cntThreads];

            for (int i = 0; i < cntThreads; i++)
            {
                threads[i] = new Thread(new Calc(breaker, step, i + 1).Calculate);
                threads[i].Start();
            }
            new Thread(breaker.Stop).Start();
            Console.ReadLine();

        }
    }
    class Calc
    {
        private Breaker breaker;
        private int step;
        private int idd;

        public Calc(Breaker breaker, int step, int idd)
        {
            this.breaker = breaker;
            this.step = step;
            this.idd = idd; 
        }
        public void Calculate()
        {
            long sum = 0;
            long numOfSteps = 0;
            bool isStop;
            do
            {
                sum += step;
                numOfSteps++;
                isStop = breaker.IsStop();

            } while (!isStop);
            Console.WriteLine($"Id: {idd}\t Sum: {sum}\t Num Of Steps: {numOfSteps}\t Step: {step}\t Time: {breaker.GetTime}");
        }
    }

    class Breaker
    {
        private bool isStop;
        private int time;
        
        [MethodImplAttribute(MethodImplOptions.Synchronized)]
        public bool IsStop() { return isStop; }
        public int  GetTime { get => time; }

        public Breaker(int time) 
        {
            this.time = time > 0 ? time : 1;
        }

        public void Stop() 
        { 
            Thread.Sleep(time * 1000);
            isStop = true;
        }
    }
}
