using System.Text.RegularExpressions;

namespace CnC.Application.Shared.Helpers.SlugHelpers;

public static class SlugHelper
{
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.ToLowerInvariant();

        text = text
            .Replace("ə", "e")
            .Replace("ö", "o")
            .Replace("ü", "u")
            .Replace("ğ", "g")
            .Replace("ş", "s")
            .Replace("ç", "c");

        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

        text = Regex.Replace(text, @"\s+", "-").Trim('-');

        text = Regex.Replace(text, @"-+", "-");

        return text;
    }
}
