using System.Net;
using System.Net.Mail;
using PurviEnterprises.Models;

namespace PurviEnterprises.Services;

public class EnquiryEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EnquiryEmailService> _logger;

    public EnquiryEmailService(IConfiguration configuration, ILogger<EnquiryEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task NotifyOwnerAsync(EnquiryViewModel enquiry)
    {
        var host = _configuration["Smtp:Host"];
        var username = _configuration["Smtp:Username"];
        var password = _configuration["Smtp:Password"];
        var ownerEmail = _configuration["Smtp:OwnerEmail"];

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(ownerEmail))
        {
            _logger.LogWarning("SMTP is not configured. Enquiry was saved locally but no owner email was sent.");
            return;
        }

        try
        {
            using var message = new MailMessage(username, ownerEmail)
            {
                Subject = $"New Purvi Enterprises enquiry: {enquiry.Service}",
                Body = $"Name: {enquiry.Name}\nPhone: {enquiry.Phone}\nEmail: {enquiry.Email}\nService: {enquiry.Service}\n\nProject details:\n{enquiry.Message}",
                IsBodyHtml = false
            };
            message.ReplyToList.Add(new MailAddress(enquiry.Email));

            using var client = new SmtpClient(host, _configuration.GetValue<int>("Smtp:Port", 587))
            {
                EnableSsl = _configuration.GetValue("Smtp:EnableSsl", true),
                Credentials = new NetworkCredential(username, password)
            };

            await client.SendMailAsync(message);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Enquiry was saved, but owner email notification failed.");
        }
    }
}
