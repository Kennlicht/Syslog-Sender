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
using System.Threading;

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

            string address = targetTextBox.Text;
            if (!(String.IsNullOrWhiteSpace(address)))
            {
                // Create and start the thread with an anonymous delegate using a lambda expression
                Thread pingThread = new Thread(() => this.pingServer(address));
                pingThread.Start();
            }
        }
        
        void SendButtonClick(object sender, EventArgs e)
        {
            statusLabel.Text = "";
        }
        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();

        }
        
        // This method is called as a thread
        private void pingServer(string address)
        {
            //bool result_var = false;
            
            try
            {
                const int timeout = 2000;
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(address, timeout);

                switch (pingReply.Status)
                {
                    case IPStatus.Success:
                        //result_var = true;
                        //statusLabel.Text = "Ok, " + address + " ist erreichbar.";
                        break;
                        
                    case IPStatus.TimedOut:
                    case IPStatus.TimeExceeded:
                        //statusLabel.Text = "Zeitüberschreitung!";
                        
                        break;
                    case IPStatus.DestinationHostUnreachable:
                        //statusLabel.Text = "Unerreichbar!";
                        break;
                    case IPStatus.HardwareError:
                        //statusLabel.Text = "Hardware-Fehler!";
                        break;
                    default:
                        //statusLabel.Text = "Nix Ping!";
                        break;
                }
            }
            catch (PingException)
            {
               //statusLabel.Text = "Ungültige Adresse!";
               //MessageBox.Show(ex.Message, "Ping", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (ArgumentException)
            {
               //statusLabel.Text = "Ungültige Adresse!";
            }
         }
    }
}
