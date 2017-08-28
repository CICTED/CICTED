using CICTED.Domain.Infrastucture.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit.Text;

namespace CICTED.Domain.Infrastucture.Services
{
    public class EmailServices : IEmailServices
    {
        public async Task<bool> EnviarEmail(string email, string link, string subject = "Confirm Email - CICTED", string password = null)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("informativo@cicted.com.br"));
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = GetHTMLBody(link, password);

                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.AuthenticationMechanisms.Remove("NTLM");
                    client.Connect("server22.integrator.com.br", 465);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("informativo@cicted.com.br", "cictedunitau2015");

                    client.Send(message);
                    client.Disconnect(true);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string GetHTMLBody(string url, string password = null)
        {
            var html = "<div>"
                        + "<div>"
                        + "<div style =\"font-family:Helvetica; height:auto; width:600px; margin:auto\">"
                          + "<div style =\"color:#fff; text-align:center; font-size:35px; height:75px; width:560px; padding:20px\">"
                            + "<div style =\"float:left; margin-left:10px; margin-top:10px\"><div style=\"display: inline-block;\"><button type=\"button\" class=\"_at_6 o365button\" aria-labelledby=\"_ariaId_1073\"><img src=\"http://www.cicted.com.br/imagens/logo_white_shadow.png\" alt=\"V CICTED\" style=\"width:170px\"><span class=\"_fc_3 owaimg\" style=\"display: none;\"> </span><span class=\"_fc_4 o365buttonLabel\" id=\"_ariaId_1073\" style=\"display: none;\"></span></button></div></div>"
                              + "<div style =\"float:right; margin-right:10px; margin-top:10px\"><div style=\"display: inline-block;\"><button type=\"button\" class=\"_at_6 o365button\" aria-labelledby=\"_ariaId_1074\"><img src=\"http://www.cicted.com.br/imagens/tema.png\" alt=\"CICTED\" style=\"height:50px\"><span class=\"_fc_3 owaimg\" style=\"display: none;\"> </span><span class=\"_fc_4 o365buttonLabel\" id=\"_ariaId_1074\" style=\"display: none;\"></span></button></div></div>"
                                + "</div>"
                                + "<div style =\"font-size:15px; padding-top:25px; text-align:justify; width:500px; margin:0 auto\">"
                        + "Para confirmar sua conta do <span class=\"highlight\" id=\"0.7043774154285753\" name=\"searchHitInReadingPane\">CICTED</span> <a href=\"{0}\" target=\"_blank\">clique aqui</a>. <br>"
                        + "<br>"
                        + $"{(password == "" ? "" : "<b>SUA SENHA: " + password + "</b><br>")}"
                        + "<div align =\"right\">Atenciosamente,<br>"
                        + "Sistema <span class=\"highlight\" id=\"0.8189381140174152\" name=\"searchHitInReadingPane\">CICTED</span>.<br>"
                        + "<br>"
                        + "</div>"
                        + "</div>"
                        + "<div style =\"background-color:#808080; font-size:13px; text-align:center; color:#fff; height:50px; width:560px; padding:20px\">"
                        + "Esta mensagem pode conter informações privilegiadas e/ou de caráter confidencial, não podendo ser retransmitida sem autorização do remetente.Se você não é o destinatário ou pessoa autorizada a recebê-la,pedimos que ignore este email e informamos que o seu uso, divulgação, cópia ou arquivamento são proibidos.<br>"
                        + "<br>"
                        + "</div>"
                        + "</div>"
                        + "<br>"
                        + "<br>"
                        + "<b>Para enviar seus comentários, envie para<span class=\"highlight\" id=\"0.03207074854009684\" name=\"searchHitInReadingPane\">cicted</span>@unitau.br .<br>"
                        + "Esta é uma mensagem gerada automaticamente, portanto, não deve ser respondida.</b></div>"
                        + "</div>";

            return string.Format(html, url);
        }

        public async Task<bool> EnviarEmailRecuperarConta(string email, string link, string subject = "Confirm Email - CICTED")
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("informativo@cicted.com.br"));
                message.To.Add(new MailboxAddress(email));
                message.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = GetHTMLBodyRecuperarSenha(link);

                message.Body = builder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.AuthenticationMechanisms.Remove("NTLM");
                    client.Connect("server22.integrator.com.br", 465);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("informativo@cicted.com.br", "cictedunitau2015");

                    client.Send(message);
                    client.Disconnect(true);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string GetHTMLBodyRecuperarSenha(string url)
        {
            var html = "<div>"
                        + "<div>"
                        + "<div style =\"font-family:Helvetica; height:auto; width:600px; margin:auto\">"
                          + "<div style =\"color:#fff; text-align:center; font-size:35px; height:75px; width:560px; padding:20px\">"
                            + "<div style =\"float:left; margin-left:10px; margin-top:10px\"><div style=\"display: inline-block;\"><button type=\"button\" class=\"_at_6 o365button\" aria-labelledby=\"_ariaId_1073\"><img src=\"http://www.cicted.com.br/imagens/logo_white_shadow.png\" alt=\"V CICTED\" style=\"width:170px\"><span class=\"_fc_3 owaimg\" style=\"display: none;\"> </span><span class=\"_fc_4 o365buttonLabel\" id=\"_ariaId_1073\" style=\"display: none;\"></span></button></div></div>"
                              + "<div style =\"float:right; margin-right:10px; margin-top:10px\"><div style=\"display: inline-block;\"><button type=\"button\" class=\"_at_6 o365button\" aria-labelledby=\"_ariaId_1074\"><img src=\"http://www.cicted.com.br/imagens/tema.png\" alt=\"CICTED\" style=\"height:50px\"><span class=\"_fc_3 owaimg\" style=\"display: none;\"> </span><span class=\"_fc_4 o365buttonLabel\" id=\"_ariaId_1074\" style=\"display: none;\"></span></button></div></div>"
                                + "</div>"
                                + "<div style =\"font-size:15px; padding-top:25px; text-align:justify; width:500px; margin:0 auto\">"
                        + "Para confirmar sua conta do <span class=\"highlight\" id=\"0.7043774154285753\" name=\"searchHitInReadingPane\">CICTED</span> <a href=\"{0}\" target=\"_blank\">clique aqui</a>. <br>"
                        + "<br>"
                        + "<div align =\"right\">Atenciosamente,<br>"
                        + "Sistema <span class=\"highlight\" id=\"0.8189381140174152\" name=\"searchHitInReadingPane\">CICTED</span>.<br>"
                        + "<br>"
                        + "</div>"
                        + "</div>"
                        + "<div style =\"background-color:#808080; font-size:13px; text-align:center; color:#fff; height:50px; width:560px; padding:20px\">"
                        + "Esta mensagem pode conter informações privilegiadas e/ou de caráter confidencial, não podendo ser retransmitida sem autorização do remetente.Se você não é o destinatário ou pessoa autorizada a recebê-la,pedimos que ignore este email e informamos que o seu uso, divulgação, cópia ou arquivamento são proibidos.<br>"
                        + "<br>"
                        + "</div>"
                        + "</div>"
                        + "<br>"
                        + "<br>"
                        + "<b>Para enviar seus comentários, envie para<span class=\"highlight\" id=\"0.03207074854009684\" name=\"searchHitInReadingPane\">cicted</span>@unitau.br .<br>"
                        + "Esta é uma mensagem gerada automaticamente, portanto, não deve ser respondida.</b></div>"
                        + "</div>";

            return string.Format(html, url);
        }

    }
    
}
