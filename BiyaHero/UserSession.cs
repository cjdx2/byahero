using Microsoft.Maui.Storage;

namespace BiyaHero;

public static class UserSession
{
    private const string UserEmailKey = "SignedInUserEmail";
    private const string UserNameKey = "UserFullName";

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

    // Save the user's full name to session storage
    public static void SaveUserFullName(string fullName) {
        Preferences.Set(UserNameKey, fullName);  // Save the full name as a single string
    }

    // Get the stored full name from session storage
    public static string GetUserFullName() {
        return Preferences.Get(UserNameKey, string.Empty); // Default to an empty string if not found
    }

    // Update the stored full name in session storage
    public static void UpdateUserFullName(string newFullName) {
        Preferences.Set(UserNameKey, newFullName);  // Update the saved full name to the new one
    }

    // Clear all session data (logout the user)
    public static void ClearSession()
    {
        Preferences.Remove(UserEmailKey);  // Remove the email from session storage
        Preferences.Remove(UserNameKey);
        // Optionally, clear other authentication data if applicable
    }
}
