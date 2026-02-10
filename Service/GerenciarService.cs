using ERP.Infra;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Linq;
using System.Net.Mime;

namespace ERP.Service
{
    public class GerenciarService
    {
        private IConfiguration _config;
        protected Context context;


        public GerenciarService(Context context,
            IConfiguration config) {

            _config = config;
            this.context = context;
        }

        public void EnviarEmail()
        {
           // #region EnviarEmailCliente
           /*  System.Net.NetworkCredential credentialsSendGrid = new System.Net.NetworkCredential("apikey", "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");
            SmtpClient smtpClientSendGrid = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

            var pedido = context.Pedido.FirstOrDefault();


            string subject = $"Gerenciar Sistemas - Boas Vindas - "+pedido.NomeCliente;
            string templateHtml = $"<h2> Parabéns pela sua aquisição.  </span>";

            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("gerenciar@gerenciarsc.com.br", $"Pedido {pedido.IdPedido} contratado!");
            mailMsg.Subject = subject;
            mailMsg.To.Add(new MailAddress(pedido.Email));
            string html = templateHtml;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            smtpClientSendGrid.Credentials = credentialsSendGrid;

            try
            {
                smtpClientSendGrid.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
            }
            #endregion */

        }
    }
}
