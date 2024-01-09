using System.ComponentModel.DataAnnotations;

namespace ELMSApplication.Properties.Attributes{
  public class PasswordValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var password = value as string;

        if (string.IsNullOrEmpty(password))
            return false;

        if (password.Length < 6)
            return false;

        var hasUpperCase = false;
        var hasLowerCase = false;
        var hasNumber = false;
        var hasNonAlphanumeric = false;

        foreach (var c in password)
        {
            if (char.IsUpper(c))
                hasUpperCase = true;
            else if (char.IsLower(c))
                hasLowerCase = true;
            else if (char.IsDigit(c))
                hasNumber = true;
            else if (!char.IsLetterOrDigit(c))
                    hasNonAlphanumeric = true;
        }

        return hasUpperCase && hasLowerCase && hasNumber && hasNonAlphanumeric;
    }
}

}