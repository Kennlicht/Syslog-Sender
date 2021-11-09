/*
 * Created by SharpDevelop.
 * User: Halfmann.Achim
 * Date: 08.11.2021
 * Time: 13:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Syslog_Sender
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
        
        void MainFormLoad(object sender, EventArgs e)
        {
            statusLabel.Text = "";
            
            //TODO: Fix Quick'n'dirty-Code
            
            // Lokale IP-Adressen ermitteln
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress ipAddress in addressList)
            {
                // Nur IPv4
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    sourceComboBox.Items.Add(ipAddress.ToString());
            }
            //foreach (IPAddress ipAddress in addressList)
            //{
            //    // Nur IPv6
            //    if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            //        sourceComboBox.Items.Add(ipAddress.ToString());
            //}
            
            //TODO: und letzten aktiven Eintrag wieder aktivieren
            
            //TODO: letzten Eintrag in Target wieder eintragen
        }
        
        void PingButtonClick(object sender, EventArgs e)
        {
            statusLabel.Text = "";

            if (targetTextBox.TextLength == 0)
                return;
            string address = targetTextBox.Text;
            const int timeout = 2000;

            //TODO: Worker-Thread
            
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(address, timeout);

                switch (pingReply.Status)
                {
                    case IPStatus.Success:
                        statusLabel.Text = "Ok, " + address + " ist erreichbar.";
                        break;
                        
                    case IPStatus.TimedOut:
                    case IPStatus.TimeExceeded:
                        statusLabel.Text = "Zeitüberschreitung!";
                        break;
                    case IPStatus.DestinationHostUnreachable:
                        statusLabel.Text = "Unerreichbar!";
                        break;
                    case IPStatus.HardwareError:
                        statusLabel.Text = "Hardware-Fehler!";
                        break;
                    default:
                        statusLabel.Text = "Nix Ping!";
                        break;
                }
            }
            catch (PingException)
            {
               statusLabel.Text = "Ungültige Adresse!";
            }
            catch (ArgumentException)
            {
               statusLabel.Text = "Ungültige Adresse!";                
            }
        }
        
        void SendButtonClick(object sender, EventArgs e)
        {
            statusLabel.Text = "";          
        }
    }
}
