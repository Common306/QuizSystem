using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class QuestionDao
    {
        public static List<Question> GetListByQuizId(int quizId)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Question>? questions = context.Questions.Include(x => x.Quiz).Where(x => x.QuizId == quizId).ToList();
                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Question Create(Question question)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.Questions.Add(question);
                    context.SaveChanges();
                    return question;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Question Update(int id, Question updateQuestion)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Question? question = context.Questions.FirstOrDefault(x => x.QuestionId == id);
                    if (question == null)
                    {
                        return null;
                    }
                    question.Content = updateQuestion.Content;
                    question.Score = updateQuestion.Score;
                    question.MultipleChoice = updateQuestion.MultipleChoice;
                    question.IsActive = updateQuestion.IsActive;
                    context.SaveChanges();
                    return updateQuestion;
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
                    Question? question = context.Questions.FirstOrDefault(x => x.QuestionId == id);
                    if (question == null)
                    {
                        return false;
                    }
                    context.Questions.Remove(question);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
