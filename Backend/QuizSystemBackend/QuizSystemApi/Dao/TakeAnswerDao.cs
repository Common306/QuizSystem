using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class TakeAnswerDao
    {
        public static List<TakeAnswer> Create(List<TakeAnswer> takes)
        {
            try
            {
                using (var context = new DBContext())
                {
                    foreach (var takeAnswer in takes)
                    {
                        context.TakeAnswers.Add(takeAnswer);
                        context.SaveChanges();
                    }
                    return takes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
