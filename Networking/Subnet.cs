using System;

namespace Networking
{
    class Subnet
    {
        public IP NetworkAddress { get; }
        private ushort suffix;

        public Subnet(IP NetworkAddress, ushort Suffix)
        {   
            if (Suffix <= 32)
            {
                suffix = Suffix;
            } else
            {
                throw new ArgumentOutOfRangeException("The given suffix is invalid; Expected value between 0 and 32");
            }

            if (CalcNetworkAddress(NetworkAddress, Suffix) == NetworkAddress)
            {
                this.NetworkAddress = NetworkAddress;
            } 
            else
            {
                throw new ArgumentException("The given networkaddress is not valid for the given suffix");
            }
        }

        /// <summary>Creates the smallest possible subnet that includes the two given ip-addresses</summary>
        public Subnet(IP A, IP B)
        {
            uint diff = A > B ? A - B : B - A;

            long tempDiff = (long)diff + 1;
            uint x = 1;
            ushort hostBits = 0;
            while (tempDiff > 0)
            {
                tempDiff -= x;
                x *= 2;
                hostBits++;
            }
            suffix = (ushort)(32 - hostBits);

            NetworkAddress = CalcNetworkAddress(A, suffix);
        }

        /// <summary>Returns a string representation of the network mask</summary>
        /// <returns>String representation of the network mask</returns>
        public string NetworkMask()
        {
            return new IP(calcNetworkMask(suffix)).ToString();
        }
        
        /// <summary>Calculates the possible amount of hosts for the subnet</summary>
        /// <returns>Possible hostcount of the subnet</returns>
        public uint PossibleHostCount()
        {
            return Convert.ToUInt32(Math.Pow(2, 32 - suffix) - 2);
        }

        /// <summary>Returns the broadcast-address of the subnet</summary>
        /// <returns>The broadcast-address of the subnet</returns>
        public IP Broadcast()
        {
            return new IP(CalcNetworkAddress(NetworkAddress, suffix) | InverseNetworkMask());
        }

        /// <summary>Checks if the given host is a valid host in the subnet</summary>
        public bool IsHostAddress(IP host)
        {
            return (host > NetworkAddress && host < Broadcast()) ? true : false;
        }

        /// <summary>Flips the bits of the network-mask and represents the range of host-addresses</summary>
        /// <returns>The flipped network-mask of the subnet</returns>
        public IP InverseNetworkMask()
        {
            return ~new IP(calcNetworkMask(suffix));
        }

        public override string ToString()
        {
            return $"{NetworkAddress}/{suffix}";
        }

        /// <summary>Calculates the correct network address</summary>
        /// <returns>The correct network address</returns>
        public static IP CalcNetworkAddress(IP NetworkAddress, ushort Suffix)
        {   
            return new IP(calcNetworkMask(Suffix) & NetworkAddress);
        }

        /// <summary>Check if two subnets are intersecting</summary>
        public static bool AreIntersecting(Subnet A, Subnet B)
        {
            return  ((A.NetworkAddress >= B.NetworkAddress && A.NetworkAddress <= B.Broadcast()
                    || A.Broadcast() >= B.NetworkAddress && A.Broadcast() <= B.Broadcast())    
                    ||(B.NetworkAddress >= A.NetworkAddress && B.NetworkAddress <= A.Broadcast()
                    || B.Broadcast() >= A.NetworkAddress && B.Broadcast() <= A.Broadcast()));
        }

        private static uint calcNetworkMask(ushort suffix)
        {
            uint networkMask = 0b0u;
            short offset = 31;
            for (int i = 0; i < suffix; i++)
            {
                networkMask += 1u << offset;
                offset -= 1;
            }

            return networkMask;
        }
    }
}
