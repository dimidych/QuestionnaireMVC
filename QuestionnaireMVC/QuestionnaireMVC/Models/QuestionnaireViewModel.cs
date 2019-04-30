using System.Collections.Generic;

namespace QuestionnaireMVC.Models
{
    /// <summary>
    /// Представление данных модели (вью-модель)
    /// </summary>
    public class QuestionnaireViewModel
    {
        private readonly IQuestionnaireRepository _questionnaireRepo;

        /// <summary>
        /// Конструктор для отправки данных на форму
        /// </summary>
        /// <param name="questionnaireRepo"></param>
        /// <param name="respondentId"></param>
        /// <param name="questionId"></param>
        public QuestionnaireViewModel(IQuestionnaireRepository questionnaireRepo, int respondentId, int questionId)
        {
            _questionnaireRepo = questionnaireRepo;
            RespondentId = respondentId;
            QuestionId = questionId;
            CurrentQuestion = questionnaireRepo.CalculateQuestion(QuestionId).Result;
            IsThisLastQuestion = questionnaireRepo.IsLastQuestion(QuestionId).Result;
            AnswerContent = questionnaireRepo.GetAnswerContent(respondentId, questionId).Result;
        }

        /// <summary>
        /// Конструктор для получения данных с формы
        /// </summary>
        public QuestionnaireViewModel()
        {
        }

        /// <summary>
        /// ИД респондента
        /// </summary>
        public int RespondentId { get; set; }

        /// <summary>
        /// Ид вопроса
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Флаг - текущий вопрос последний
        /// </summary>
        public bool IsThisLastQuestion { get; set; }

        /// <summary>
        /// Хранит информацию о вопросе
        /// </summary>
        public Question CurrentQuestion { get; }

        /// <summary>
        /// Содержимое ответа в виде строки
        /// </summary>
        public string AnswerContent { get; set; }

        /// <summary>
        /// Список полов в виде, удобном для отображения в выпадающем списке
        /// </summary>
        public  IEnumerable<Sex> SexList =>  _questionnaireRepo?.GetSexList().Result;

        /// <summary>
        /// Список видов семейного положения в виде, удобном для отображения в выпадающем списке
        /// </summary>
        public IEnumerable<MaritalStatus> MaritalStatusList => _questionnaireRepo?.GetMaritalStatusList().Result;

        /// <summary>
        /// Представление содержимого ответа в виде, удобном для отображения в чекбоксе
        /// </summary>
        public bool AnswerContentAsBool
        {
            get => bool.TryParse(AnswerContent, out var answerContentAsBool) && answerContentAsBool;
            set => AnswerContent = value.ToString();
        }
    }
}