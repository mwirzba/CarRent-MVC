

namespace CarRent.Data
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class Claims
    {

    }

    public enum RentalStatus
    {
        Reservation=0,
        Checked = 1,
        Archival =2
    }

}
