namespace Domain.Entities
{
    public class AppUser
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }

        public AppUser(string userName, string email, byte[] passwordHash, byte[] passwordSalt)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void UpdateUsername(string userName)
        {
            UserName = userName;
        }
    }
}
