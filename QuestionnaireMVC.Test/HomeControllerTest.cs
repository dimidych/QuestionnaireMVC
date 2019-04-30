using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireMVC.Controllers;
using QuestionnaireMVC.Models;
using Xunit;

namespace QuestionnaireMVC.Test
{
    public class HomeControllerTest
    {
        private HomeController InitializeHomeController(out IQuestionnaireRepository repository,
            out MockQuestionnaireDbContext context)
        {
            context = new MockQuestionnaireDbContext();
            repository = new MockQuestionnaireRepository(context);
            return new HomeController(repository);
        }

        private HomeController InitializeHomeController(out IQuestionnaireRepository repository)
        {
            return InitializeHomeController(out repository, out var context);
        }

        [Fact]
        public async Task IndexActionRedirectTest()
        {
            var homeController = InitializeHomeController(out var repository);
            var indexCall = await homeController.Index();
            var viewResult = Assert.IsType<RedirectToActionResult>(indexCall);
            Assert.Equal(viewResult.ActionName, "AnswerTheQuestion");
            Assert.Equal(viewResult.ControllerName, "Home");
            var respondentIdExpected = await repository.CalculateNewRespondentId();
            Assert.Equal((int) (viewResult.RouteValues["questionId"]), 1);
            Assert.Equal(respondentIdExpected, (int) (viewResult.RouteValues["respondentId"]));
        }

        [Fact]
        public async Task AnswerTheStringQuestionGetTest()
        {
            var homeController = InitializeHomeController(out var repository, out var context);
            var respondentIdExpected = await repository.CalculateNewRespondentId();
            var answerTheStringQuestionGetCall = homeController.AnswerTheQuestion(respondentIdExpected, 1);
            var viewResult = Assert.IsType<ViewResult>(answerTheStringQuestionGetCall);
            var model = Assert.IsAssignableFrom<QuestionnaireViewModel>(viewResult.Model);
            Assert.False(model.AnswerContentAsBool);
            Assert.Equal(model.IsThisLastQuestion, context.Questions.Count() == model.QuestionId);
            Assert.NotNull(model.CurrentQuestion);
            Assert.False(string.IsNullOrEmpty(model.CurrentQuestion.QuestionContent));
            Assert.Equal(model.CurrentQuestion.TypeId,
                context.QuestionTypes
                    .FirstOrDefault(x => x.TypeName.Equals("string", StringComparison.InvariantCultureIgnoreCase))
                    .TypeId);
        }

        [Fact]
        public async Task AnswerTheEnumQuestionGetTest()
        {
            var homeController = InitializeHomeController(out var repository, out var context);
            var respondentIdExpected = await repository.CalculateNewRespondentId();
            var answerTheEnumQuestionGetCall = homeController.AnswerTheQuestion(respondentIdExpected, 5);
            var viewResult = Assert.IsType<ViewResult>(answerTheEnumQuestionGetCall);
            var model = Assert.IsAssignableFrom<QuestionnaireViewModel>(viewResult.Model);
            Assert.False(model.AnswerContentAsBool);
            Assert.Equal(model.IsThisLastQuestion, context.Questions.Count() == model.QuestionId);
            Assert.NotNull(model.CurrentQuestion);
            Assert.False(string.IsNullOrEmpty(model.CurrentQuestion.QuestionContent));
            Assert.Equal(model.CurrentQuestion.TypeId,
                context.QuestionTypes
                    .FirstOrDefault(x =>
                        x.TypeName.Equals("maritalStatusEnum", StringComparison.InvariantCultureIgnoreCase))
                    .TypeId);
            Assert.NotEmpty(model.MaritalStatusList);
        }

        [Fact]
        public async Task AnswerTheQuestionWithModelErrorPostTest()
        {
            var homeController = InitializeHomeController(out var repository, out var context);
            var questionnaireVm = new QuestionnaireViewModel();
            var result = await homeController.AnswerTheQuestion(questionnaireVm);
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AnswerTheLastQuestionPostTest()
        {
            var homeController = InitializeHomeController(out var repository, out var context);
            var respondentIdExpected = await repository.CalculateNewRespondentId();
            var questionnaireVm = new QuestionnaireViewModel
            {
                QuestionId = 6,
                IsThisLastQuestion = true,
                RespondentId = respondentIdExpected,
                AnswerContent = "True"
            };
            var result = await homeController.AnswerTheQuestion(questionnaireVm);
            Assert.True(questionnaireVm.AnswerContentAsBool);
            var view = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(view.ActionName.Equals("FinishQuestionnaire", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task AnswerTheQuestionPostTest()
        {
            var homeController = InitializeHomeController(out var repository, out var context);
            var respondentIdExpected = await repository.CalculateNewRespondentId();
            var questionnaireVm = new QuestionnaireViewModel
            {
                QuestionId = 1,
                RespondentId = respondentIdExpected,
                AnswerContent = "Some"
            };
            var result = await homeController.AnswerTheQuestion(questionnaireVm);
            var view = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(view.ActionName.Contains("AnswerTheQuestion"));
            var nextQuestionId = questionnaireVm.QuestionId + 1;
            Assert.Equal(nextQuestionId, (int) (view.RouteValues["questionId"]));
            Assert.Equal(respondentIdExpected, (int) (view.RouteValues["respondentId"]));
        }
    }
}
