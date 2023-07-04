﻿using QuizSystemApi.Dao;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;

namespace QuizSystemApi.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        public List<Question> GetListByQuizId(int quizId, User user)
        {
            return QuestionDao.GetListByQuizId(quizId, user);
        }

        public Question Create(Question question)
        {
            return QuestionDao.Create(question);
        }

        public Question Update(int id, Question question, User user)
        {
            return QuestionDao.Update(id, question, user);
        }

        public bool Delete(int id, User user)
        {
            return QuestionDao.Delete(id, user);
        }
    }
}
