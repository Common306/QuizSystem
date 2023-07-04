﻿using Microsoft.EntityFrameworkCore;
using QuizSystemApi.Models;

namespace QuizSystemApi.Dao
{
    public class QuizDao
    {
        public static List<Quiz> GetAll(User user)
        {
            try
            {
                using (var context = new DBContext())
                {
                    List<Quiz> quizzes = context.Quizzes.Include(x => x.Creator)
                        .Where(x => user.RoleId == 1 || x.CreatorId == user.UserId).ToList();
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
    }
}