using System.Linq;
using WebApplication2.Models;
using WebApplication2.ViewModel;

namespace WebApplication2.ModelsAction
{
    public class UserAction
    {
        public static bool CreateUser(User user)
        {
            using(var db = new DBContext())
            {
                User tmp = db.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
                if(tmp != null)
                {
                    db.Dispose();
                    return false;
                }
                else
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            return true;
        }
        public static string UpdateUser(UpdateUserViewModel user)
        {
            string result = "";
            using (var db = new DBContext())
            {
                User tmp = db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                //must be specific instead of tmp = user, that won't work
                if(user.NewPassword != null)
                {
                    if (user.Password == tmp.Password)
                    {
                        if(user.NewPassword == user.RetypeNewPassword)
                        {
                            tmp.Password = user.NewPassword;
                        }
                        else
                        {
                            result = "Error: New Password and Retyped New Password doesn't match";
                        }
                    }
                    else
                    {
                         result = "Error: Current password is not correct ";
                    }
                }
                tmp.Name = user.Name;
                tmp.PhoneNumber = user.PhoneNumber;
                tmp.Email = user.PhoneNumber;
                db.SaveChanges();
            }
            return result;
        }
    }
}