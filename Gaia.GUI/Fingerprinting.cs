using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gaia.GUI
{
    public class Fingerprinting
    {
        public void Scan()
        {
            WlanClient client = new WlanClient();

            try
            {
                foreach (WlanInterface wlanIface in client.Interfaces)
                {

                    WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();

                    foreach (WlanBssEntry network in wlanBssEntries)
                    {
                        int rss = network.rssi;
                        //     MessageBox.Show(rss.ToString());
                        byte[] macAddr = network.dot11Bssid;

                        string tMac = "";

                        for (int i = 0; i < macAddr.Length; i++)
                        {

                            tMac += macAddr[i].ToString("x2").PadLeft(2, '0').ToUpper();

                        }

                        GlobalAccess.WriteConsole("Found network with SSID " +  System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString());
                        GlobalAccess.WriteConsole("Signal: " + network.linkQuality);
                        GlobalAccess.WriteConsole("BSS Type: " + network.dot11BssType);
                        GlobalAccess.WriteConsole("MAC: " + tMac);
                        GlobalAccess.WriteConsole("RSSID: " + rss.ToString());


                    }
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }
    }
}
