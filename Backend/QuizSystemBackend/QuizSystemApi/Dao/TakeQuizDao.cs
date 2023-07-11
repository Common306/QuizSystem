using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class TakeQuizDao
    {
        public static List<TakeQuiz> GetAll()
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<TakeQuiz> quizzes = context.TakeQuizzes.Include(x => x.Quiz).Include(x=>x.User).ToList();
                    return quizzes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
