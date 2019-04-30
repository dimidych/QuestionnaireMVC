using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireMVC.Models;

namespace QuestionnaireMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionnaireRepository _questionnaireRepo;

        /// <summary>
        /// В конструкторе происходит подвязка к репозиторию через DI
        /// </summary>
        /// <param name="questionnaireRepo"></param>
        public HomeController(IQuestionnaireRepository questionnaireRepo)
        {
            _questionnaireRepo = questionnaireRepo;
        }

        /// <summary>
        /// Вычисляется идентификатор нового респондента.
        /// Происходит перенаправление к первому вопросу 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var respondentId = await _questionnaireRepo.CalculateNewRespondentId();
            return RedirectToAction("AnswerTheQuestion", "Home",
                new {respondentId = respondentId, questionId = 1});
        }

        /// <summary>
        /// В GET запросе конфигурируем вью-модель для отображения вопроса на форме
        /// </summary>
        /// <param name="respondentId">идентификатор респондента</param>
        /// <param name="questionId">идентификатор вопроса</param>
        [HttpGet]
        public IActionResult AnswerTheQuestion(int respondentId, int questionId)
        {
            var questionnaireVm = new QuestionnaireViewModel(_questionnaireRepo, respondentId, questionId);
            return View(questionnaireVm);
        }

        /// <summary>
        /// В POST получаем нужные нам данные с формы (с проверкой модели).
        /// Если проверка модели проходит успешно, то заносим данные ответа в кэш репозитория (для дальнейшего добавления в БД).
        /// Проверяем, если данные соответствуют последнему вопросу, то завершаем опрос и перенаправляемся к финишной странице.
        /// Если вопрос не последний, переходим к следующему вопросу
        /// </summary>
        /// <param name="questionnaireVm">данные с формы сохраняются во вью-модели</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTheQuestion(QuestionnaireViewModel questionnaireVm)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(questionnaireVm.AnswerContent))
            {
                ModelState.AddModelError("", "Поля заполнены не полностью или не верно.");
                return View(questionnaireVm);
            }

            _questionnaireRepo.AddAnswer(new Answer
            {
                AnswerContent = questionnaireVm.AnswerContent,
                QuestionId = questionnaireVm.QuestionId,
                RespondentId = questionnaireVm.RespondentId
            });

            if (await _questionnaireRepo.IsLastQuestion(questionnaireVm.QuestionId))
                return RedirectToAction("FinishQuestionnaire");

            var nextQuestionOrder = questionnaireVm.QuestionId + 1;
            return RedirectToAction("AnswerTheQuestion", "Home",
                new {respondentId = questionnaireVm.RespondentId, questionId = nextQuestionOrder});
        }

        /// <summary>
        /// Финишная страница опроса.
        /// Данные из кэша репозитория сохраняются в БД
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FinishQuestionnaire()
        {
            await _questionnaireRepo.SaveChanges();
            return View();
        }
    }
}
