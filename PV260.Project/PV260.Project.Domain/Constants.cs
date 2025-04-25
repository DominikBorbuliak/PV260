namespace PV260.Project.Domain;

public static class Constants
{
    public static class Error
    {
        public const string NotFoundFormat = "{0} with provided {1} not found.";
        public const string Unauthorized = "User is unauthorized. Please, login.";
        public const string Unexpected = "Unexpected error occured. Please, try again later.";
    }

    public static class Email
    {
        public const string Subject = "New ARK Diff Report Available";
        public const string NoChanges = "A new diff report has been generated, but no changes were detected.";
        public const string ChangesIntroFormat = "A new diff report has been generated with {0} change(s):";

        public const string ChangeAddedFormat = "{0} ({1}) — Added with {2} shares.";
        public const string ChangeRemovedFormat = "{0} ({1}) — Removed (had {2} shares).";
        public const string ChangeModifiedFormat = "{0} ({1}) — Shares changed from {2} to {3}.";
        public const string ChangeUnknownFormat = "{0} — Unknown change.";
    }
}
