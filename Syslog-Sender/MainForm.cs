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

//using Syslog;

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
        
        #region UI
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
            foreach (IPAddress ipAddress in addressList)
            {
                // Nur IPv6
                if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    sourceComboBox.Items.Add(ipAddress.ToString());
            }
            
            //TODO: letzten aktiven Eintrag wieder aktivieren
            //TODO: letzten Eintrag in Target wieder eintragen

            foreach (string facility in Enum.GetNames(typeof(Syslog.Message.FacilityEnum)))
                facilityComboBox.Items.Add(facility);
            facilityComboBox.SelectedIndex = 16;      //TODO: letzten aktiven Eintrag wieder aktivieren
                
            foreach (string severity in Enum.GetNames(typeof(Syslog.Message.SeverityEnum)))
                severityComboBox.Items.Add(severity.ToString());
            severityComboBox.SelectedIndex = 7;       //TODO: letzten aktiven Eintrag wieder aktivieren
        }
        
        void PingButtonClick(object sender, EventArgs e)
        {
            statusLabel.Text = "";
            
            string address = targetComboBox.Text;
            if (!(String.IsNullOrWhiteSpace(address)))
            {                
                // Create and start the thread with an anonymous delegate using a lambda expression
                Thread pingThread = new Thread(() => this.pingServer(address));
                // Damit sind die Ausnahmen des Threads angelsächsisch:
                pingThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
                pingThread.Start();
                
                pingButton.Enabled = false;
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
        #endregion
        
        #region Callbacks
        // Update UI from within thread
        public delegate void updateStatusDelegate(bool ok, string msg, string log);
        void updateStatus(bool ok, string msg, string log)
        {
            // Status
            if (!String.IsNullOrEmpty(msg))
            {
                if (ok)
                    statusLabel.ForeColor = System.Drawing.Color.Green;
                else
                    statusLabel.ForeColor = System.Drawing.Color.Red;
                statusLabel.Text = msg.Replace('\r', ' ').Replace('\n', ' ');
                statusLabel.Spring = true;  // ohne geht nicht, kann aber im Designer bereits gesetzt werden.
                //statusStrip1.Update();    // das braucht dann nicht zwangsläufig zu sein...
            }
            
            // Log-Eintrag
            if (!String.IsNullOrEmpty(log))
            {  
                logListBox.Items.Add(log);
                // Scroll to the bottom
                logListBox.TopIndex = logListBox.Items.Count - 1;        
            }
            
            // Normalzustand 
            pingButton.Enabled = true;
            sendButton.Select();
        }
        #endregion
        
        #region Threads
        // This method is called as a thread
        void pingServer(string address)
        {
            bool result_var = false;
            string result_msg = "";
            string result_log = "";
            
            try
            {
                const int timeout = 2000;
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(address, timeout);

                if (pingReply.Status == IPStatus.Success)
                {
                    result_var = true;
                    result_msg = "Ping: " + address + " ok.";
                    result_log = "Ping " + address + " --> " + pingReply.Status + " from " + pingReply.Address;
                }
                else
                {
                    result_msg = "Ping: " + address + " failed!";
                    result_log = "Ping " + address + " --> " + pingReply.Status + "!";
                }
                this.Invoke(new updateStatusDelegate(updateStatus), new object[] {result_var, result_msg, result_log});
            }      
            catch (Exception ex)    // PingException, ArgumentException, ...
            {
                result_msg = "Ping: " + address + " failed!";
                result_log = "Ping " + address + " --> " + ex.Message;
                this.Invoke(new updateStatusDelegate(updateStatus), new object[] {false, result_msg, result_log});
            }   
        }
        #endregion
        
        #region Disable ComboBoxes
        // comboBox.DropDownStyle = ComboBoxStyle.DropDownList; makes it look 3D and sometimes its just plain ugly.
        void SourceComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        void FacilityComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        void SeverityComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
    }
}
