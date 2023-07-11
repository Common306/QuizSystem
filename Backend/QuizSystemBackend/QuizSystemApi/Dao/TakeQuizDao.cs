using Microsoft.AspNetCore.Mvc;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class TakeQuizDao
    {
        public static TakeQuiz Create(TakeQuiz take)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.TakeQuizzes.Add(take);
                    context.SaveChanges();
                    return take;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
