using System;

namespace Networking
{
    class Program
    {
        static void Main(string[] args)
        {
            Subnet sn = new Subnet(Subnet.CalcNetworkAddress(new IP("10.160.123.0"), 24), 24);
            Console.WriteLine(sn);
            Console.WriteLine(sn.NetworkMask());
            Console.WriteLine(sn.Broadcast());
        }
    }
}
