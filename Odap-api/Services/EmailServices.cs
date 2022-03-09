using Odap.Models;
using System;
using System.Net;
using System.Net.Mail; 

namespace OdapApi.Services
{
    public static  class EmailServices
    {
        public static  string SendEmailForNetworkNotification(RAngle rAngle)
        {
            string approveStatus;
            if (rAngle.IsApproved)
            {
               approveStatus = "The change has already been sent to the Antenna since it was made directly by Authorized Operator <br> <br> ";
            }
            else
            {
               string approvalLink = $"https://localhost:44305/api/Odap/approveRAngle/{rAngle.Id}";
               approveStatus = $"Kindly approve <a href={approvalLink} target=\"_blank\"> here </a> for the switch to be done, or ignore otherwise. <br> <br> ";
            }
            try
            {
                var credentials = new NetworkCredential("humphryshikunzi9@gmail.com", "2288Shiks.");
                var mail = new MailMessage()
                {
                    From = new MailAddress("humphryshikunzi9@gmail.com"),
                    Subject = "Odap Network Switch Alert.",
                    Body = $"Hello,<br><br> " +
                    $"This is Odap Network Monitoring. <br> <br> " +
                    $"Network has shifted from the following location <strong> {rAngle.LocationFrom} </strong>, to the following location <strong> {rAngle.LocationTo}</strong>, by an angle change of <strong> {rAngle.Angle}° </strong>on <strong>{DateTime.Now.ToShortDateString()}</strong> at <strong>{DateTime.Now.TimeOfDay}. </strong><br><br>" +
                    approveStatus +
                    $"Thank you. <br> <br> " +
                    $"Regards, <br><br> " +
                    $"Odap Network Administration and Monitoring  <br> " +
                    $"Nairobi,  Kenya"
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(rAngle.EmailToSendTo));
               var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                client.Send(mail);
                return mail.Body;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }         
    }
}
