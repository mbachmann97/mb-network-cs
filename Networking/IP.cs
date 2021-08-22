using System;
using System.Linq;

namespace Networking
{
    class IP
    {
        private uint packagedIP = 0b0u;

        public IP(string IpString)
        {
            packagedIP = uintFromString(IpString);
        }

        public IP(uint Ip)
        {
            if (IsValid(uintToString(Ip)))
            {
                packagedIP = Ip;
            }
            else
            {
                throw new FormatException("given uint does not convert to a valid IP");
            }
        }

        public override string ToString()
        {
            return uintToString(packagedIP);
        }

        /// <summary>Checks if the given IP is valid</summary>
        public static bool IsValid (string Ip)
        {
            try
            {
                uintFromString(Ip);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static uint uintFromString(string ipString)
        {
            uint packaged = 0b0u;
            string[] stringOctets = ipString.Split('.');
            if (stringOctets.Length == 4)
            {
                ushort[] octets = stringOctets
                                .Select(ushort.Parse)
                                .ToArray();

                ushort offset = 24;
                foreach (int octet in octets)
                {
                    if (octet >= 0 && octet <= 255)
                    {
                        packaged += (uint)octet << offset;
                        offset -= 8;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("One or more octets not in valid range");
                    }
                }

                return packaged;
            }
            else
            {
                throw new FormatException("IP string format invalid");
            }
        }

        private static string uintToString(uint ip)
        {
            string ipString = "";
            string binaryString = Convert.ToString(ip, toBase: 2).PadLeft(32, '0');
            short offset = 0;

            for (int i = 0; i < 4; i++)
            {
                ipString += Convert.ToInt32(binaryString.Substring(offset, 8), 2);
                if (i != 3) ipString += ".";
                offset += 8;
            }

            return ipString;
        }

        public static uint operator &(uint a, IP b) => a & b.packagedIP;
        public static uint operator |(IP a, IP b) => a.packagedIP | b.packagedIP;
        public static IP operator ~(IP IpToInverse) => new IP(~IpToInverse.packagedIP);

        public static bool operator !=(IP a, IP b) => a.packagedIP != b.packagedIP;
        public static bool operator ==(IP a, IP b) => a.packagedIP == b.packagedIP;

        public static bool operator <(IP a, IP b) => a.packagedIP < b.packagedIP;
        public static bool operator >(IP a, IP b) => a.packagedIP > b.packagedIP;
        public static bool operator <=(IP a, IP b) => a.packagedIP <= b.packagedIP;
        public static bool operator >=(IP a, IP b) => a.packagedIP >= b.packagedIP;

        public static uint operator -(IP a, IP b) => a.packagedIP - b.packagedIP;
        public static IP operator -(IP a, uint b) => new IP(a.packagedIP - b);
        public static IP operator +(IP a, uint b) => new IP(a.packagedIP + b);

        public override int GetHashCode()
        {
            return (int)packagedIP;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                IP temp = (IP)obj;

                return temp.ToString() == ToString();
            }
        }
    }
}
