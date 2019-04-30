using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireMVC.Models;

namespace QuestionnaireMVC.Test
{
    public class MockQuestionnaireRepository : IQuestionnaireRepository
    {
        private readonly MockQuestionnaireDbContext _context;
        private IEnumerable<Answer> _answerCache;

        public MockQuestionnaireRepository(MockQuestionnaireDbContext context)
        {
            _context = context;
            _answerCache = new List<Answer>();
        }

        public void Dispose()
        {
            _answerCache = null;
        }

        public async Task<int> CalculateNewRespondentId()
        {
            if (_context==null||!_context.Answers.Any())
                return 1;

            return await Task.Run(() => _context.Answers.Max(x => x.RespondentId) + 1);
        }

        public async Task<Question> CalculateQuestion(int questionId)
        {
            if (!_context.Questions.Any())
                return null;

            var result = await Task.Run(() => _context.Questions.FirstOrDefault(x => x.QuestionId == questionId));

            if (result != null)
                result.QuestionType =
                    await Task.Run(() => _context.QuestionTypes.FirstOrDefault(x => x.TypeId == result.TypeId));

            return result;
        }

        public async Task<bool> IsLastQuestion(int questionId)
        {
            if (!_context.Questions.Any())
                return true;

            return await Task.Run(() => _context.Questions.Count == questionId);
        }

        public void AddAnswer(Answer answer)
        {
            var existedAnswer = _answerCache.FirstOrDefault(x =>
                x.QuestionId == answer.QuestionId && x.RespondentId == answer.RespondentId);

            if (existedAnswer != null)
                ((List<Answer>) _answerCache).Remove(existedAnswer);

            ((List<Answer>) _answerCache).Add(answer);
        }

        public async Task SaveChanges()
        {
            try
            {
                await Task.Run(() => _context.Answers.AddRange(_answerCache));
            }
            finally
            {
                _answerCache = new List<Answer>();
            }
        }

        public async Task<string> GetAnswerContent(int respondentId, int questionId)
        {
            var answer = await Task.Run(() => _answerCache.FirstOrDefault(x =>
                x.RespondentId == respondentId && x.QuestionId == questionId));
            return answer == null ? string.Empty : answer.AnswerContent;
        }

        public async Task<IEnumerable<MaritalStatus>> GetMaritalStatusList()
        {
            return await Task.Run(() => _context.MaritalStatuses.ToList());
        }

        public async Task<IEnumerable<Sex>> GetSexList()
        {
            return await Task.Run(() => _context.Sexs.ToList());
        }
    }
}