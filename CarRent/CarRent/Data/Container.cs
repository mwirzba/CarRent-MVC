

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

    public enum RentalStatus : byte
    {
        Reservation=1,
        Checked = 2,
        Archival =3
    }

}
