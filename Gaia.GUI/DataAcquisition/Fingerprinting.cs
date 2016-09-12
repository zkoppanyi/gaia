using Gaia.Core;
using Gaia.Core.DataStreams;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gaia.GUI.DataAcquisition
{
    public class Fingerprinting : Algorithm
    {
        public DataStream OutputDataStream { get; set; }
        public static FingerprintingFactory Factory
        {
            get
            {
                return new FingerprintingFactory();
            }
        }

        public class FingerprintingFactory : AlgorithmFactory
        {
            public String Name { get { return "Collect WiFi fingerprinting data."; } }
            public String Description { get { return "Collect WiFi fingerprinting data."; } }

            public Fingerprinting Create(Project project, IMessanger messanger, WifiFingerptiningDataStream outputDataStream)
            {
                Fingerprinting algorithm = new Fingerprinting(project, messanger, Name, Description);
                algorithm.OutputDataStream = outputDataStream;
                return algorithm;
            }
        }

        private Fingerprinting(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {

        }

        public override AlgorithmResult Run()
        {
            WlanClient client = new WlanClient();

            if ((client.Interfaces == null) || (client.NoWifiAvailable == true))
            {
                WriteProgress(100);
                WriteMessage("Error: No Wifi Available!");
                return AlgorithmResult.InputMissing;
            }

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

                        GlobalAccess.WriteConsole("Found network with SSID " + System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString());
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
                WriteMessage("Error occured during the data acquisition: " + ex.Message);
                return AlgorithmResult.Failure;
            }

            WriteProgress(100);
            return AlgorithmResult.Sucess;
        }

    }
}
