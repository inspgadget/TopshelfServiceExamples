using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace BasicWinservice.Services
{
    public class BasicService
    {
        private readonly Timer _timer;

        public BasicService()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer elapsed");
        }

        public bool Start()
        {
            _timer.Start();
            return true;
        }

        public bool Stop()
        {
            _timer.Stop();
            return true;
        }
    }
}
