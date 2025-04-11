using Microsoft.Extensions.Options;

namespace Blueprints.RabbitClient.Configuration;

public class RabitConfigurationValidator : IValidateOptions<RabbitConfiguration>
{
    public ValidateOptionsResult Validate(string name, RabbitConfiguration options)
    {
        var validationMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(options.Host))
        {
            validationMessage += "Host is required \n";
        }

        if (options.Port <= 0)
        {
            validationMessage += "Port is required \n";
        }

        if (string.IsNullOrWhiteSpace(options.UserName))
        {
            validationMessage += "UserName is required \n";
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            validationMessage += "Password is required \n";
        }

        if (!string.IsNullOrEmpty(validationMessage))
        {
            return ValidateOptionsResult.Fail(validationMessage);
        }
        
        return ValidateOptionsResult.Success;
    }
}