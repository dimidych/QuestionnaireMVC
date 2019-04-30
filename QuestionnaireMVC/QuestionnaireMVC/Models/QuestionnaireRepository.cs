using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace QuestionnaireMVC.Models
{
    public class QuestionnaireRepository : IQuestionnaireRepository
    {
        private readonly IQuestionnaireContext _questionnaireContext;
        private IEnumerable<Answer> _answerCache;

        /// <summary>
        /// Репозиторий основных команд
        /// </summary>
        /// <param name="questionnaireContext">контекст БД</param>
        public QuestionnaireRepository(IQuestionnaireContext questionnaireContext)
        {
            _questionnaireContext = questionnaireContext;
            _answerCache = new List<Answer>();
        }

        /// <summary>
        /// Чистим кэш ответов. Диспозим контекст БД
        /// </summary>
        public void Dispose()
        {
            _answerCache = null;
            ((QuestionnaireContext) _questionnaireContext).Dispose();
        }

        /// <summary>
        /// Вычисляем ид нового респондента
        /// </summary>
        public async Task<int> CalculateNewRespondentId()
        {
            if (!_questionnaireContext.Answers.Any())
                return 1;

            return await _questionnaireContext.Answers.MaxAsync(x => x.RespondentId) + 1;
        }

        /// <summary>
        /// Получаем вопрос по его ид
        /// </summary>
        /// <param name="questionId">ид вопроса</param>
        public async Task<Question> CalculateQuestion(int questionId)
        {
            if (!_questionnaireContext.Questions.Any())
                return null;

            var result = await _questionnaireContext.Questions.FirstOrDefaultAsync(x => x.QuestionId == questionId);

            if (result != null)
                result.QuestionType =
                    await _questionnaireContext.QuestionTypes.FirstOrDefaultAsync(x => x.TypeId == result.TypeId);

            return result;
        }

        /// <summary>
        /// Проверяем, является ли вопрос последним
        /// </summary>
        /// <param name="questionId">ид вопроса</param>
        public async Task<bool> IsLastQuestion(int questionId)
        {
            if (!_questionnaireContext.Questions.Any())
                return true;

            return await _questionnaireContext.Questions.CountAsync() == questionId;
        }

        /// <summary>
        /// Добавляем ответ в кэш
        /// </summary>
        /// <param name="answer">Ответ</param>
        public void AddAnswer(Answer answer)
        {
            var existedAnswer = _answerCache.FirstOrDefault(x =>
                x.QuestionId == answer.QuestionId && x.RespondentId == answer.RespondentId);

            if (existedAnswer != null)
                ((List<Answer>) _answerCache).Remove(existedAnswer);

            ((List<Answer>) _answerCache).Add(answer);
        }

        /// <summary>
        /// Сохраняем кэш ответов в БД.
        /// Затем чистим кэш
        /// </summary>
        public async Task SaveChanges()
        {
            try
            {
                await _questionnaireContext.Answers.AddRangeAsync(_answerCache);
                await ((QuestionnaireContext) _questionnaireContext).SaveChangesAsync();
            }
            finally
            {
                _answerCache = new List<Answer>();
            }
        }

        /// <summary>
        /// Получаем строковое представление ответа из кэша
        /// </summary>
        /// <param name="respondentId">ид респондента</param>
        /// <param name="questionId">ид вопроса</param>
        public async Task<string> GetAnswerContent(int respondentId, int questionId)
        {
            var answer = await Task.Run(() => _answerCache.FirstOrDefault(x =>
                x.RespondentId == respondentId && x.QuestionId == questionId));
            return answer == null ? string.Empty : answer.AnswerContent;
        }

        /// <summary>
        /// Список полов
        /// </summary>
        public async Task<IEnumerable<Sex>> GetSexList()
        {
            return await _questionnaireContext.Sexs.ToListAsync();
        }

        /// <summary>
        /// Список семейных статусов
        /// </summary>
        public async Task<IEnumerable<MaritalStatus>> GetMaritalStatusList()
        {
            return await _questionnaireContext.MaritalStatuses.ToListAsync();
        }
    }
}