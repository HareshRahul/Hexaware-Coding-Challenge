using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace InsuranceClaim.entity
{
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public User() { }

        public User(int userId, string username, string password, string role)
        {
            try
            {
                this.userId = userId;
                this.username = username;
                this.password = password;
                this.role = role;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating User: " + ex.Message);
            }
        }

        public override string ToString()
        {
            return $"UserId: {userId}, Username: {username}, Role: {role}";
        }
    }
}
