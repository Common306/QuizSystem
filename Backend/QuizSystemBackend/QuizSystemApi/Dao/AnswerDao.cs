using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class AnswerDao
    {
        public static List<Answer> GetListByQuestionId(int questionId)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Answer>? answers = context.Answers.Include(x => x.Question).Where(x => x.QuestionId == questionId).ToList();
                    return answers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Answer> CreateListAnswer(List<Answer> answers)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.Answers.AddRange(answers);
                    context.SaveChanges();
                    return answers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
