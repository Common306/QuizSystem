using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class QuestionDao
    {
        public static List<Question> GetListByQuizId(int quizId, User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Question>? questions = context.Questions.Include(x => x.Quiz)
                        .Where(x => x.QuizId == quizId && (user.Role.RoleId == 1 || x.Quiz.CreatorId == user.UserId)).ToList();
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

        public static Question Update(int id, Question updateQuestion, User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Question? question = context.Questions.Include(x => x.Quiz)
                        .FirstOrDefault(x => x.QuestionId == id && (user.Role.RoleId == 1 || x.Quiz.CreatorId == user.UserId));
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

        public static bool Delete(int id, User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Question? question = context.Questions.Include(x => x.Quiz).FirstOrDefault(x => x.QuestionId == id && (user.RoleId == 1 || x.Quiz.CreatorId == user.UserId));
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

        public static List<Question> GetListByQuizId(int quizId)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Question>? questions = context.Questions.Include(x => x.Quiz)
                        .Where(x => x.QuizId == quizId && x.IsActive == true).ToList();
                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Question> GetKeyOfQuiz(int quizId)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Question>? questions = context.Questions.Include(x => x.Quiz)
                        .Where(x => x.QuizId == quizId && x.IsActive == true)
                        .Include(x => x.Answers.Where(a => a.IsActive == true && a.IsCorrect == true))
                        .ToList();
                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<Question> CreateListQuestion(List<Question> questions)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.Questions.AddRange(questions);
                    context.SaveChanges();
                    return questions;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
