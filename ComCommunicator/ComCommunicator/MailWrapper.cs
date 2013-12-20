using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EASendMail;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ComCommunicator
{
    public class MailWrapper
    {
        private string _emailAddressFrom = "lorenzo.vorabbi@gmail.com";
        private string _emailPasswordFrom = "4cm7oMGI";
        private List<string> _listMailAddressTo = new List<string>();

        public string AddressFrom
        {
            set
            {
                if (value.Contains("@gmail.com") == false)
                {
                    MessageBox.Show("Sendere email must belong to gmail domain", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _emailAddressFrom = value; 
            }
            get { return _emailAddressFrom; }
        }

        public string PasswordFrom
        {
            set { _emailPasswordFrom = value; }
            get { return _emailPasswordFrom; }
        }

        public int NumberOfClients
        {
            get { return _listMailAddressTo.Count; }
        }

        public MailWrapper()
        {
            _listMailAddressTo.Add("andilamce@gmail.com");
            _listMailAddressTo.Add("lorenzo.vorabbi@gmail.com");
            _listMailAddressTo.Add("ermandlamce@gmail.com");
        }

        public bool SendEmailToClients(string subject, string text)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (SendMail(_listMailAddressTo.ElementAt(i), subject, text, null) == false)
                {
                    //MessageBox.Show("Error sending e-mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public bool SendEmailToClients(string subject, string text, string attachmentAddress)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (SendMail(_listMailAddressTo.ElementAt(i), subject, text, attachmentAddress) == false)
                {
                    //MessageBox.Show("Error sending e-mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public bool SendEmailToClient(string client, string subject, string text)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (String.Compare(_listMailAddressTo.ElementAt(i), client) == 0)
                {
                    if (SendMail(_listMailAddressTo.ElementAt(i), subject, text, null) == false)
                    {
                        //MessageBox.Show("Error sending e-mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SendEmailToClient(string client, string subject, string text, string attachmentAddress)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (String.Compare(_listMailAddressTo.ElementAt(i), client) == 0)
                {
                    if (SendMail(_listMailAddressTo.ElementAt(i), subject, text, attachmentAddress) == false)
                    {
                        //MessageBox.Show("Error sending e-mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool AddClientForEmail(string clientEmail)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (String.Compare(_listMailAddressTo.ElementAt(i), clientEmail) == 0)
                {
                    return false;
                }
            }

            _listMailAddressTo.Add(clientEmail);
            return true;
        }

        public bool RemoveClientForEmail(string clientEmail)
        {
            for (int i = 0; i < _listMailAddressTo.Count; i++)
            {
                if (String.Compare(_listMailAddressTo.ElementAt(i), clientEmail) == 0)
                {
                    _listMailAddressTo.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private bool SendMail(string mailto, string subject, string text, string attachmentAddress)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Your gmail email address
            oMail.From = _emailAddressFrom;

            // Set recipient email address
            oMail.To = mailto;

            // Set email subject
            oMail.Subject = subject;

            // Set email body
            oMail.TextBody = text;

            if (attachmentAddress != null)
            {
                oMail.AddAttachment(attachmentAddress);
            }

            // Gmail SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.gmail.com");

            // Set 465 port
            oServer.Port = 465;

            // detect SSL/TLS automatically
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            // Gmail user authentication
            // For example: your email is "gmailid@gmail.com", then the user should be the same
            oServer.User = _emailAddressFrom;
            oServer.Password = _emailPasswordFrom;

            try
            {
                oSmtp.SendMail(oServer, oMail);
                return true;
            }
            catch (Exception excp)
            {
                //MessageBox.Show(excp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
