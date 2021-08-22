using System;
using System.Collections.Generic;

namespace Networking
{
    class Program
    {
        static void Main(string[] args)
        {
            // Subnet sn = new Subnet(Subnet.CalcNetworkAddress(new IP("10.160.123.0"), 24), 24);
            // Console.WriteLine(sn);
            // Console.WriteLine(sn.NetworkMask());
            // Console.WriteLine(sn.Broadcast());

            IP a = new IP("10.0.0.0");
            IP b = new IP("10.222.255.65");

            Subnet s = new Subnet(b, a);
            Console.WriteLine(s);
            Console.WriteLine(s.NetworkMask());
            Console.WriteLine(s.Broadcast());
        }
    }
}
