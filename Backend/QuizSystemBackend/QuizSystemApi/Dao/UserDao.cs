using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;
using System.Linq;

namespace QuizSystemApi.Dao
{
    public class UserDao
    {
        public static User? Login(User userLogin)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == userLogin.Username 
                    && x.Password == userLogin.Password 
                    && x.IsEnable == true);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User? Register(User userRegister)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.FirstOrDefault(x => x.Username == userRegister.Username);
                    if(user != null)
                    {
                        return null;
                    }
                    context.Users.Add(userRegister);
                    context.SaveChanges();
                    return userRegister;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<User> GetAll(string? search, int? page)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<User> users = new List<User>();
                    int pageSize = 10;
                    int pageNumber = (int)(page == null ? 1 : page);
                    if (search == null)
                    {
                        users = context.Users
                        .Include(x => x.Role)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                    } else
                    {
                        string valueSearch = search.ToLower().Trim();
                        users = context.Users.Include(x => x.Role)
                            .Where(x => x.Username.ToLower().Contains(valueSearch) ||
                                x.Role.RoleName.ToLower().Contains(valueSearch) ||
                                x.FullName.ToLower().Contains(valueSearch) ||
                                x.PhoneNumber.ToLower().Contains(valueSearch)
                            ).Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                    return users;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User Get(int id)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.Include(x => x.Role).FirstOrDefault(x => x.UserId == id);
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User Create(User newUser)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.FirstOrDefault(x => x.Username == newUser.Username);
                    if (user != null)
                    {
                        return null;
                    }
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    return newUser;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User Update(int id, User updateUser)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.FirstOrDefault(x => x.UserId == id);
                    if (user == null)
                    {
                        return null;
                    }
                    user.Password = updateUser.Password;
                    user.RoleId = updateUser.RoleId;
                    user.FullName = updateUser.FullName;
                    user.PhoneNumber = updateUser.PhoneNumber;
                    user.UpdateAt = DateTime.Now;
                    user.IsEnable = updateUser.IsEnable;
                    context.SaveChanges();
                    return updateUser;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                using (var context = new DBContext())
                {
                    User? user = context.Users.FirstOrDefault(x => x.UserId == id);
                    if(user == null)
                    {
                        return false;
                    }
                    context.Users.Remove(user);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static int Total(string? search)
        {
            try
            {
                using (var context = new DBContext())
                {
                    if(search != null)
                    {
                        string valueSearch = search.ToLower().Trim();
                        return context.Users
                            .Where(x => x.Username.ToLower().Contains(valueSearch) ||
                                x.Role.RoleName.ToLower().Contains(valueSearch) ||
                                x.FullName.ToLower().Contains(valueSearch) ||
                                x.PhoneNumber.ToLower().Contains(valueSearch)).Count();
                    }
                    return context.Users.Count();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
