using Microsoft.Maui.Storage;

namespace BiyaHero;

public static class UserSession
{
    private const string UserEmailKey = "SignedInUserEmail";

    // Save the user's email to session storage
    public static void SaveUserEmail(string email)
    {
        Preferences.Set(UserEmailKey, email);
    }

    // Get the currently stored email from session storage
    public static string GetUserEmail()
    {
        return Preferences.Get(UserEmailKey, string.Empty); // Default to an empty string if not found
    }

    // Update the stored email in session storage
    public static void UpdateUserEmail(string newEmail)
    {
        Preferences.Set(UserEmailKey, newEmail);  // Update the saved email to the new one
    }

    // Clear all session data (logout the user)
    public static void ClearSession()
    {
        Preferences.Remove(UserEmailKey);  // Remove the email from session storage
        // Optionally, clear other authentication data if applicable
    }
}
