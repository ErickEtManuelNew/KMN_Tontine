using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using KMN_Tontine.Application.Interfaces;
using KMN_Tontine.Shared.Options;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly CurrencyOptions _currencyOptions;

    public EmailService(
        IConfiguration configuration,
        IOptions<CurrencyOptions> currencyOptions)
    {
        _configuration = configuration;
        _currencyOptions = currencyOptions.Value;
        
        // Configuration SMTP basée sur l'environnement
        var host = _configuration["Email:Host"];
        var port = int.Parse(_configuration["Email:Port"]);
        var username = _configuration["Email:User"];
        var password = _configuration["Email:Password"];
        _fromEmail = _configuration["Email:FromEmail"];
        _fromName = _configuration["Email:FromName"];

        _smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail, _fromName),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await _smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            // Log l'erreur
            Console.WriteLine($"Erreur d'envoi d'email: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendEmailConfirmationAsync(string to, string userName, string confirmationLink)
    {
        var subject = "Confirmez votre adresse email - KMN Tontine";
        var htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Bienvenue sur KMN Tontine, {userName}!</h2>
                <p>Votre compte a été approuvé par un administrateur. Pour finaliser votre inscription, veuillez confirmer votre adresse email.</p>
                <p style='margin: 25px 0;'>
                    <a href='{confirmationLink}' 
                       style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                        Confirmer mon email
                    </a>
                </p>
                <p>Si le bouton ne fonctionne pas, copiez et collez ce lien dans votre navigateur:</p>
                <p>{confirmationLink}</p>
                <p>Ce lien expire dans 24 heures.</p>
                <p>Cordialement,<br>L'équipe KMN Tontine</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, htmlContent);
    }

    public async Task<bool> SendAccountApprovedAsync(string to, string userName, string confirmationLink)
    {
        var subject = "Votre compte a été approuvé - KMN Tontine";
        var htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Félicitations {userName}!</h2>
                <p>Votre compte KMN Tontine a été approuvé par nos administrateurs.</p>
                <p>Pour commencer à utiliser nos services, veuillez confirmer votre adresse email en cliquant sur le lien ci-dessous:</p>
                <p style='margin: 25px 0;'>
                    <a href='{confirmationLink}' 
                       style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                        Confirmer mon email
                    </a>
                </p>
                <p>Si le bouton ne fonctionne pas, copiez et collez ce lien dans votre navigateur:</p>
                <p>{confirmationLink}</p>
                <p>Ce lien expire dans 24 heures.</p>
                <p>Cordialement,<br>L'équipe KMN Tontine</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, htmlContent);
    }

    public async Task<bool> SendAccountRejectedAsync(string to, string userName, string reason)
    {
        var subject = "Statut de votre inscription - KMN Tontine";
        var htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Cher(e) {userName},</h2>
                <p>Nous avons examiné votre demande d'inscription à KMN Tontine.</p>
                <p>Malheureusement, nous ne pouvons pas approuver votre inscription pour la raison suivante:</p>
                <p style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #dc3545;'>
                    {reason}
                </p>
                <p>Si vous pensez qu'il s'agit d'une erreur ou si vous souhaitez plus d'informations, n'hésitez pas à nous contacter.</p>
                <p>Cordialement,<br>L'équipe KMN Tontine</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, htmlContent);
    }

    public async Task<bool> SendPaymentReminderAsync(string to, string userName, decimal remainingAmount, string promiseReference)
    {
        var subject = "Rappel de paiement - KMN Tontine";
        var htmlContent = $@"
            <html>
            <body style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Cher(e) {userName},</h2>
                <p>Nous avons reçu un paiement partiel pour votre promesse #{promiseReference}.</p>
                <p>Il reste un montant de <strong>{remainingAmount.ToString(_currencyOptions.Format)} {_currencyOptions.Symbol}</strong> à régler.</p>
                <p>Veuillez effectuer le paiement du montant restant dès que possible pour éviter tout retard.</p>
                <p style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #ffc107;'>
                    <strong>Détails de la promesse :</strong><br>
                    - Numéro de promesse : #{promiseReference}<br>
                    - Montant restant : {remainingAmount.ToString(_currencyOptions.Format)} {_currencyOptions.Symbol}
                </p>
                <p>Si vous avez déjà effectué ce paiement, veuillez ignorer cet email.</p>
                <p>Cordialement,<br>L'équipe KMN Tontine</p>
            </body>
            </html>";

        return await SendEmailAsync(to, subject, htmlContent);
    }
} 