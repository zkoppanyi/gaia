using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gaia.GUI.DataAcquisition
{
    public class Fingerprinting : Algorithm
    {
        public DataStream OutputDataStream { get; set; }

        [DisplayName("Samples to be acquired")]
        public int SampleNumber { get; set; }

        [DisplayName("Waiting time [s]")]
        public double WaitingTime { get; set; }

        [DisplayName("Continous")]
        public bool IsContinous { get; set; }

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

            public Fingerprinting Create(Project project, WifiFingerptiningDataStream outputDataStream)
            {
                Fingerprinting algorithm = new Fingerprinting(project, Name, Description);
                algorithm.OutputDataStream = outputDataStream;
                return algorithm;
            }
        }

        private Fingerprinting(Project project, String name, String description) : base(project, name, description)
        {
            this.SampleNumber = 10;
            this.WaitingTime = 0.5;
            this.IsContinous = false;
        }

        protected override AlgorithmResult run()
        {

            try
            {
                OutputDataStream.Open();
                int i = 0;
                WlanClient client = new WlanClient();

                while (true)
                {
                    i++;
                    if ((i >= SampleNumber) && (this.IsContinous == false))
                    {
                        break;
                    }

                    if (IsCanceled())
                    {
                        WriteMessage("Processing canceled");
                        break;
                        //return AlgorithmResult.Failure;
                    }

                   
                    if ((client.Interfaces == null) || (client.NoWifiAvailable == true))
                    {
                        if (i == 0)
                        {
                            WriteProgress(100);
                            WriteMessage("Error: No Wifi Available!");
                            return AlgorithmResult.InputMissing;
                        }
                        else
                        {
                            WriteMessage("Warning: No Wifi Available!");
                            Thread.Sleep(Convert.ToInt32(WaitingTime * 1000.0));
                            continue;
                        }
                    }

                    foreach (WlanInterface wlanIface in client.Interfaces)
                    {
                        wlanIface.Scan();
                        WlanBssEntry[] wlanBssEntries = wlanIface.GetNetworkBssList();
                        double ts = Utilities.GetGeneralInternalTimestamp();

                        foreach (WlanBssEntry network in wlanBssEntries)
                        {
                            int rss = network.rssi;
                            byte[] macAddr = network.dot11Bssid;

                            /*WriteMessage("Time and date: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                            WriteMessage("Timestamp: " + ts);
                            WriteMessage("SSID: " + System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).ToString() + Environment.NewLine);
                            WriteMessage("Signal: " + network.linkQuality);
                            WriteMessage("BSS Type: " + network.dot11BssType);
                            WriteMessage("MAC: " + Utilities.MACAddressToString(macAddr));
                            WriteMessage("RSSID: " + rss.ToString());
                            WriteMessage(" ");*/

                            WifiFingerprintingDataLine dataLine = new WifiFingerprintingDataLine();
                            dataLine.TimeStamp = ts;
                            dataLine.MAC = macAddr;
                            dataLine.SignalStrength = rss;
                            OutputDataStream.AddDataLine(dataLine);
                        }
                        WriteMessage(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + " Int #: " + client.Interfaces.Length + " AP #: " + wlanBssEntries.Length);
                    }

                    WriteProgress((double)i/(double)SampleNumber*100);
                    Thread.Sleep(Convert.ToInt32(WaitingTime*1000.0));
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                WriteMessage("Error occured during the data acquisition: " + ex.Message);
                return AlgorithmResult.Failure;
            }
            finally
            {
                OutputDataStream.Close();
            }

            WriteProgress(100);
            project.Save();
            return AlgorithmResult.Sucess;
        }

    }
}
