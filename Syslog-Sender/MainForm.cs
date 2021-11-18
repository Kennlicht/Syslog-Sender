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
        
        // Update UI from within thread
        public delegate void updateStatusDelegate(bool ok, string msg);
        void updateStatus(bool ok, string msg)
        {
            if (ok)
                statusLabel.ForeColor = System.Drawing.Color.Green;
            else
                statusLabel.ForeColor = System.Drawing.Color.Red;
            statusLabel.Text = msg;
        }
        
        // This method is called as a thread
        void pingServer(string address)
        {
            bool result_var = false;
            string result_msg;
            
            try
            {
                const int timeout = 2000;
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(address, timeout);

                switch (pingReply.Status)
                {
                    case IPStatus.Success:
                        result_var = true;
                        result_msg = "Ping: " + address + " is reachable.";
                        break;
                    case IPStatus.TimedOut:
                    case IPStatus.TimeExceeded:
                        result_msg = "Ping: Request timed out!";
                        break;
                    case IPStatus.DestinationHostUnreachable:
                        result_msg = "Ping: Destination unreachable!";
                        break;
                    case IPStatus.HardwareError:
                        result_msg = "Ping: Transmit failed!";
                        break;
                    default:
                        result_msg = "Ping: Error!";
                        break;
                }
                this.Invoke(new updateStatusDelegate(updateStatus), new object[] {result_var, result_msg});
            }
            catch (PingException ex)
            {
                result_msg = "Ping: Exception occured!"; //TODO ex.Message
                this.Invoke(new updateStatusDelegate(updateStatus), new object[] {false, result_msg});
            }
            catch (ArgumentException ex)
            {
                result_msg = "Ping: Unknown argument!";  //TODO ex.Message
                this.Invoke(new updateStatusDelegate(updateStatus), new object[] {false, result_msg});
            }
         }
    }
}
