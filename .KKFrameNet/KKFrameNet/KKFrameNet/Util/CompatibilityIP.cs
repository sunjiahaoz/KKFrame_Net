﻿using System;
using System.Runtime.InteropServices;

using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine; 
namespace KK.Frame.Net
{

    public class CompatibilityIP
    {
        [DllImport("__Internal")]
        private static extern string getIPv6(string host, string port);

        private static string GetIPv6(string host, string port, bool IsIphone)
        {
            if (IsIphone)
            {
                return getIPv6(host, port);
            }
            else
            {
                return host + "&&ipv4";
            }
        }
        public static void GetIpType(string serverIp, string serverPort, bool bIsIPhone, out string newServerIp, out AddressFamily newServerAddressFamily)
        {
            newServerAddressFamily = AddressFamily.InterNetwork;
            newServerIp = serverIp;
            try
            {
                string mIPv6 = GetIPv6(serverIp, serverPort, bIsIPhone);
                if (!string.IsNullOrEmpty(mIPv6))
                {
                    string[] strTemp = Regex.Split(mIPv6, "&&");
                    if (strTemp.Length >= 2)
                    {
                        string type = strTemp[1];
                        if (type == "ipv6")
                        {
                            newServerIp = strTemp[0];
                            newServerAddressFamily = AddressFamily.InterNetworkV6;
                        }
                        else if (type == "ipv4")
                        {
                            newServerIp = strTemp[0];
                            newServerAddressFamily = AddressFamily.InterNetwork;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }


}
