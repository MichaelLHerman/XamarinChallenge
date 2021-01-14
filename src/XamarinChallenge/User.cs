using System;
using System.Collections.Generic;

namespace XamarinChallenge
{
    public class User : IEquatable<User?>
    {
        public User(string userName, string password)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string UserName { get; }
        public string Password { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User? other)
        {
            return other != null &&
                   UserName == other.UserName &&
                   Password == other.Password;
        }

        public override int GetHashCode()
        {
            int hashCode = 1155857689;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            return hashCode;
        }
    }

}
