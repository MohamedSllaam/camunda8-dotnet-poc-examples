namespace camunda8_dotnet_poc_examples.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string employeeName);
}

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(string email, string employeeName)
    {
        _logger.LogInformation("Sending welcome email to {Email} for {EmployeeName}", email, employeeName);

        // Simulate email sending
        await Task.Delay(500);
        _logger.LogInformation("Welcome email sent to {Email}", email);
    }
}