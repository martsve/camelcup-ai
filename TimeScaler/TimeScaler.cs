using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Delver.TimeScaler
{
    public class TimeScaler
    {
        static TimeSpan benchmark = TimeSpan.FromTicks(273898499);

        static int iterations = 1000000000;

        static double warmupScalingFactor = 1.0;

        private class TestObject
        {
            public long Value { get; set; }

            public TestObject(long val)
            {
                Value = val;
            }

            public void ModifyValue(double fact)
            {
                Value = (long)(Value * fact);
            }
        }

        public static double FindScalingFactor(double warmUpFactor = 1.0)
        {
            var elapsed = GetElapsedTimeForScaling(warmUpFactor);
            return benchmark.Ticks / (double)elapsed.Ticks;
        }

        private static TimeSpan GetElapsedTimeForScaling(double warmUpFactor = 1.0)
        {
            // warmup
            long value = 31;
            int Nwarmup = (int)(iterations * warmUpFactor);
            for (int i = 1; i < Nwarmup; i++) 
            {
                var obj = new TestObject(value);
                obj.ModifyValue(i);
                value = obj.Value;
            }

            Trace.WriteLine("Value first time: " + value);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            value = 13;
            for (int i = 1; i < iterations; i++) 
            {
                var obj = new TestObject(value);
                obj.ModifyValue(i);
                value = obj.Value;
            }

            Trace.WriteLine("Final value: " + value);

            watch.Stop();
            return watch.Elapsed;
        }

        static void Main(string[] args)
        {        
            var elapsed = GetElapsedTimeForScaling(warmupScalingFactor);
            double factor = benchmark.Ticks / (double)elapsed.Ticks;
            Console.WriteLine($"Elapsed ticks: {elapsed.Ticks}" );
            Console.WriteLine($"Scaling factor: {factor}" );
            Console.ReadLine();
        }
        
    }
}
