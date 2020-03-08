using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebapiWinservice.DB;

namespace WebapiWinservice.Services
{
    public class Ip2LocationService
    {
        private readonly ip2locationContext _ctx;

        public Ip2LocationService(ip2locationContext ctx)
        {
            _ctx = ctx;
        }

        public Ip2locationDb11 GetInfo(string ip)
        {
            var di = Dot2LongIP(ip);
            return  _ctx.Ip2locationDb11.AsNoTracking().FirstOrDefault(x => di >= x.IpFrom && di <= x.IpTo);
        }

        public double Dot2LongIP(string DottedIP)
        {
            int i;
            string[] arrDec;
            double num = 0;
            if (DottedIP == "")
            {
                return 0;
            }
            else
            {
                arrDec = DottedIP.Split('.');
                for (i = arrDec.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(arrDec[i]) % 256) * Math.Pow(256, (3 - i)));
                }
                return num;
            }
        }
    }
}
