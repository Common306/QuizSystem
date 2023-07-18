using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;
using QuizSystemApi.Dto.Response;

namespace QuizSystemApi.Dao
{
    public class QuizDao
    {
        public static List<Quiz> GetAll(User user, string? search, int? page)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Quiz> quizzes = new List<Quiz>();
                    int pageSize = 10;
                    int pageNumber = (int)(page == null ? 1 : page);
                    if (search == null)
                    {
                        quizzes = context.Quizzes.Include(x => x.Creator)
                        .Where(x => user.RoleId == 1 || x.CreatorId == user.UserId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                    } else
                    {
                        string valueSearch = search.ToLower().Trim();
                        quizzes = context.Quizzes.Include(x => x.Creator)
                        .Where(x => (user.RoleId == 1 || x.CreatorId == user.UserId)).ToList();
                            quizzes = quizzes.Where(x => x.Title.ToLower().Contains(valueSearch) ||
                            x.Description.ToLower().Contains(valueSearch))
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                    }
                    
                    return quizzes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Quiz Get(int id, User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Quiz? quiz = context.Quizzes.Include(x => x.Creator)
                        .Where(x => user.RoleId == 1 || x.CreatorId == user.UserId)
                        .FirstOrDefault(x => x.QuizId == id);
                    return quiz;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Quiz Create(Quiz quiz)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.Quizzes.Add(quiz);
                    context.SaveChanges();
                    return quiz;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Quiz Update(int id, Quiz updateQuiz, User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Quiz? quiz = context.Quizzes.Include(x => x.Creator)
                        .FirstOrDefault(x => x.QuizId == id && (user.RoleId == 1 || x.CreatorId == user.UserId));
                    if (quiz == null)
                    {
                        return null;
                    }
                    quiz.Title = updateQuiz.Title;
                    quiz.Description = updateQuiz.Description;
                    quiz.IsPublish = updateQuiz.IsPublish;
                    quiz.StartAt = updateQuiz.StartAt;
                    quiz.EndAt = updateQuiz.EndAt;
                    quiz.PassScore = updateQuiz.PassScore;
                    quiz.QuizCode = updateQuiz.QuizCode;
                    context.SaveChanges();
                    return updateQuiz;
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
                    Quiz? quiz = context.Quizzes.
                        FirstOrDefault(x => x.QuizId == id && (user.RoleId == 1 || x.CreatorId == user.UserId));
                    if (quiz == null)
                    {
                        return false;
                    }
                    context.Quizzes.Remove(quiz);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<TakeQuiz> ListResults(int id, User user, string? search, int? page)
        {
            try
            {
                using (var context = new DBContext())
                {
                    int pageSize = 10;
                    int pageNumber = (int)(page == null ? 1 : page);
                    List<TakeQuiz> takequiz = context.TakeQuizzes
                        .Include(x => x.User).Include(x => x.Quiz)
                        .Where(x => x.QuizId == id && (user.RoleId == 1 || x.Quiz.CreatorId == user.UserId))
                        .ToList();
                    if(search != null)
                    {
                        string valueSearch = search.ToLower().Trim();
                        takequiz = takequiz.Where(x => x.User.FullName.ToLower().Contains(valueSearch) ||
                            x.Quiz.Title.ToLower().Contains(valueSearch) ||
                            x.Score.ToString().Contains(valueSearch)
                        ).ToList();
                    }
                    takequiz = takequiz.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                    return takequiz;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ReviewQuizDtoResponse ReviewQuiz(int id, User user)
        {
            try
            {
                using (var _context = new DBContext())
                {
                    TakeQuiz? takeQuiz = _context.TakeQuizzes.Include(x => x.Quiz).Include(x => x.User)
                        .FirstOrDefault(x => x.TakeQuizId == id 
                        && (user.RoleId == 1 || user.UserId == x.Quiz.CreatorId || user.UserId == x.User.UserId));

                    var takeAnswer = _context.TakeAnswers
                 .Where(ta => ta.TakeQuizId == takeQuiz.TakeQuizId).ToList();

                    var reviewModel = new ReviewQuizDtoResponse();
                    reviewModel.TakeQuiz = takeQuiz;
                    reviewModel.Quiz = _context.Quizzes.Find(takeQuiz.QuizId);
                    reviewModel.Questions = _context.Questions.Include(q => q.Answers)
                        .Where(q => q.QuizId == reviewModel.Quiz.QuizId)
                        .ToList();

                    foreach (var q in reviewModel.Questions)
                    {
                        foreach (var a in q.Answers)
                        {
                            var ck = takeAnswer.Any(n => n.AnswerId == a.AnswerId);
                            reviewModel.ReviewAnswer.Add(a.AnswerId, ck);
                        }


                        var originalAnswers = q.Answers.Where(a => a.IsCorrect == true).Select(a => a.AnswerId).OrderBy(k => k).ToList();

                        var compareAnswer = takeAnswer.Where(a => a.QuestionId == q.QuestionId).
                            Select(a => a.AnswerId).OrderBy(k => k).ToList();

                        bool canGetFullQuestionScore = Enumerable.SequenceEqual(originalAnswers, compareAnswer);

                        reviewModel.QuestionScore[q.QuestionId] = (canGetFullQuestionScore) ? q.Score : 0;
                    }

                    reviewModel.TotalScore = caculateScore(id);

                    return reviewModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static double? caculateScore(int takeQuizId)
        {
            using (var _context = new DBContext())
            {
                TakeQuiz takeQuiz = _context.TakeQuizzes.Find(takeQuizId);

                if (takeQuiz is null) throw new Exception("Not found take quiz");

                var questions = _context.Questions.Where(q => q.QuizId == takeQuiz.QuizId).Include(q => q.Answers).ToList();
                var userAnswers = _context.TakeAnswers.Where(ta => ta.TakeQuizId == takeQuizId).ToList();

                if (userAnswers.Count == 0) return 0;
                double? score = 0;

                foreach (var q in questions)
                {
                    var trueAnwersOfQuestion = q.Answers.Where(a => a.IsCorrect == true).Select(a => a.AnswerId).OrderBy(k => k).ToList();
                    var userAnswerOfQuestion = userAnswers.Where(a => a.QuestionId == q.QuestionId).Select(a => a.AnswerId).OrderBy(k => k).ToList();
                    if (Enumerable.SequenceEqual(trueAnwersOfQuestion, userAnswerOfQuestion))
                    {
                        score += q.Score;
                    }
                }

                return score;
            }
        }

        public static List<Quiz> GetAll()
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Quiz> quizzes = context.Quizzes.Include(x => x.Creator)
                        .Include(x => x.Questions.Where(q => q.IsActive == true))
                        .Where(x => x.IsPublish == true).ToList();
                    return quizzes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Quiz Get(int id)
        {
            try
            {
                using (var context = new DBContext())
                {
                    Quiz? quiz = context.Quizzes.Include(x => x.Creator).FirstOrDefault(x => x.QuizId == id);
                    return quiz;
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
                    if (search == null)
                    {
                        return context.Quizzes.Count();
                    }
                    else
                    {
                        string valueSearch = search.ToLower().Trim();
                        return context.Quizzes.Where(x => x.Title.Contains(valueSearch) ||
                            x.Description.Contains(valueSearch)).Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public static int TotalQuizResult(int id, string? search)
        {
            try
            {
                using (var context = new DBContext())
                {
                    if (search == null)
                    {
                        return context.TakeQuizzes.Where(x => x.QuizId == id).Count();
                    }
                    else
                    {
                        string valueSearch = search.ToLower().Trim();
                        return context.TakeQuizzes.Include(x => x.User).Include(x => x.Quiz).Where(x => x.QuizId == id &&
                        (
                            x.User.FullName.ToLower().Contains(valueSearch) ||
                            x.Quiz.Title.ToLower().Contains(valueSearch) ||
                            x.Score.ToString().Contains(valueSearch)
                        )).Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
