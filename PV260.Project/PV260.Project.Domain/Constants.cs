namespace PV260.Project.Domain;

public static class Constants
{
    public static class Error
    {
        public const string NotFoundFormat = "{0} with provided {1} not found.";
        public const string Unauthorized = "User is unauthorized. Please, login.";
        public const string Unexpected = "Unexpected error occured. Please, try again later.";
    }
}
