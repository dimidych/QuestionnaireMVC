using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuestionnaireMVC.Models
{
    public interface IQuestionnaireRepository : IDisposable
    {
        Task<int> CalculateNewRespondentId();
        Task<Question> CalculateQuestion(int questionId);
        Task<bool> IsLastQuestion(int questionId);
        void AddAnswer(Answer answer);
        Task SaveChanges();
        Task<string> GetAnswerContent(int respondentId, int questionId);
        Task<IEnumerable<MaritalStatus>> GetMaritalStatusList();
        Task<IEnumerable<Sex>> GetSexList();
    }
}