/*
 * Created by SharpDevelop.
 * User: Halfmann.Achim
 * Date: 24.11.2021
 * Time: 07:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Syslog
{
    /// <summary>
    /// Description of Syslog.
    /// </summary>
    public class Message
    {
        public Message()
        {
        }
        
        public enum FacilityEnum : int
        {
            kernel      = 0,    // kernel messages
            user        = 1,    // user-level messages
            mail        = 2,    // mail system
            system      = 3,    // system daemons
            security    = 4,    // security/authorization messages (note 1)
            internally  = 5,    // messages generated internally by syslogd
            printer     = 6,    // line printer subsystem
            news        = 7,    // network news subsystem
            uucp        = 8,    // UUCP subsystem
            cron        = 9,    // clock daemon (note 2) changed to cron
            security2   = 10,   // security/authorization messages (note 1)
            ftp         = 11,   // FTP daemon
            ntp         = 12,   // NTP subsystem
            audit       = 13,   // log audit (note 1)
            alert       = 14,   // log alert (note 1)
            clock       = 15,   // clock daemon (note 2)
            local0      = 16,   // local use 0  (local0)
            local1      = 17,   // local use 1  (local1)
            local2      = 18,   // local use 2  (local2)
            local3      = 19,   // local use 3  (local3)
            local4      = 20,   // local use 4  (local4)
            local5      = 21,   // local use 5  (local5)
            local6      = 22,   // local use 6  (local6)
            local7      = 23,   // local use 7  (local7)
        }
        
        public enum SeverityEnum : int
        {
            emergency   = 0,    // Emergency: system is unusable
            alert       = 1,    // Alert: action must be taken immediately
            critical    = 2,    // Critical: critical conditions
            error       = 3,    // Error: error conditions
            warning     = 4,    // Warning: warning conditions
            notice      = 5,    // Notice: normal but significant condition
            info        = 6,    // Informational: informational messages
            debug       = 7,    // Debug: debug-level messages
        }
    }
}
