using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PlaylistES.Models;
namespace PlaylistES.DAL
{
    public class UserInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<UserContext>
    {
        protected override void Seed(UserContext context)
        {
            var users = new List<User>
            {
            new User{username="Hello",password="123456",Profilepicture="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQMQsB3l79Js4_u-Z4kQ8PyyYCo_8QOM8bTdw&usqp=CAU"},
            new User{username="Greetings",password="123456",Profilepicture="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQMQsB3l79Js4_u-Z4kQ8PyyYCo_8QOM8bTdw&usqp=CAU"},         
            };

            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
           
        }
    }
}
