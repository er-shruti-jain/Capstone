public static class UtilityService
{
    public static bool IsValidInput(string input, string allowedCharacters)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        var validchars = allowedCharacters.ToHashSet();

        var isvalid = input.All(c => char.IsLetterOrDigit(c) || validchars.Contains(c));

        return isvalid;

    }

}