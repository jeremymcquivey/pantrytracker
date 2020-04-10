namespace PantryTracker.Model.Auth
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string UserProfileId { get; set; }
        public int? ProjectId { get; set; }
        public string Value { get; set; }
    }
}
