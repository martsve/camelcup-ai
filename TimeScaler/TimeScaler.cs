using System;
using System.Diagnostics;

namespace Delver.TimeScaler
{
    public class TimeScaler
    {
        static readonly TimeSpan _benchmark = TimeSpan.FromTicks(273898499);

        static readonly int _iterations = 1000000000;

        static readonly double _warmupScalingFactor = 1.0;

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
            return _benchmark.Ticks / (double)elapsed.Ticks;
        }

        private static TimeSpan GetElapsedTimeForScaling(double warmUpFactor = 1.0)
        {
            // warmup
            long value = 31;
            var nwarmup = (int)(_iterations * warmUpFactor);
            for (var i = 1; i < nwarmup; i++) 
            {
                var obj = new TestObject(value);
                obj.ModifyValue(i);
                value = obj.Value;
            }

            Trace.WriteLine("Value first time: " + value);

            var watch = Stopwatch.StartNew();
            
            value = 13;
            for (var i = 1; i < _iterations; i++) 
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
            var elapsed = GetElapsedTimeForScaling(_warmupScalingFactor);
            var factor = _benchmark.Ticks / (double)elapsed.Ticks;
            Console.WriteLine($"Elapsed ticks: {elapsed.Ticks}" );
            Console.WriteLine($"Scaling factor: {factor}" );
            Console.ReadLine();
        }
        
    }
}
