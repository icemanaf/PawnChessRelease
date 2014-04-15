
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;


namespace chessboard
{
    //This class represents the systems high performance timer
    //and is used to profile the performance elsewhere in the system.
    class PerfTimer
    {
        private long  _StartTime;
        private long  _EndTime;
        private long _Frequency;

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);
        private void Reset()
        {
            _StartTime = 0;
            _EndTime = 0;
        }


        public PerfTimer()
        {
            if (!QueryPerformanceFrequency(out _Frequency))
                throw new ApplicationException("System high performance timer could not be initialized!.");
        }

        public void StartMeasurement()
        {
            //todo
            QueryPerformanceCounter(out _StartTime);
        }
        public double  StopAndGetMeasurement()
        {
            QueryPerformanceCounter(out _EndTime);

            return (double)((_EndTime - _StartTime)/(double)_Frequency);
        }
        public void ClearTimer()
        {
            //reset the timer
            Reset();
        }
        
    }
}
