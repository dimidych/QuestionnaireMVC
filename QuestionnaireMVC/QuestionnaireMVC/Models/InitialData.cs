using System.Linq;

namespace QuestionnaireMVC.Models
{
    /// <summary>
    /// Cоздаем начальные данные опросника
    /// </summary>
    internal static class InitialData
    {
        public static void Initialize(QuestionnaireContext context)
        {
            if (!context.MaritalStatuses.Any())
            {
                context.MaritalStatuses.AddRange(new MaritalStatus
                    {
                        Name = "Холост(Не замужем)"
                    },
                    new MaritalStatus
                    {
                        Name = "Женат(Замужем)"
                    }
                );
            }

            if (!context.Sexs.Any())
            {
                context.Sexs.AddRange(new Sex
                    {
                        Name = "Женский"
                    },
                    new Sex
                    {
                        Name = "Мужской"
                    }
                );
            }

            if (!context.QuestionTypes.Any())
            {
                context.QuestionTypes.AddRange(new QuestionType
                    {
                        TypeName = "int",
                        /*HtmlPattern =
                            "<input type='number' name='AnswerContent' id='AnswerContent' />"  */
                    },
                    new QuestionType
                    {
                        TypeName = "string",
                       /* HtmlPattern =
                            "<input type='text' name='AnswerContent' id='AnswerContent' />" */
                    },
                    new QuestionType
                    {
                        TypeName = "date",
                        /*HtmlPattern =
                            "<input type='date' id='AnswerContent' name='AnswerContent'  />"  */
                    },
                    new QuestionType
                    {
                        TypeName = "bool",
                       /* HtmlPattern =
                            "<input type='checkbox' name='AnswerContent' id='AnswerContent'  />"     */
                    },
                    new QuestionType
                    {
                        TypeName = "sexEnum",
                        /*HtmlPattern =
                            "<select name='AnswerContent' id='AnswerContent'><option value='1'>муж</option><option value='0'>жен</option></select>"*/
                    },
                    new QuestionType
                    {
                        TypeName = "maritalStatusEnum",
                        /*HtmlPattern =
                            "<select name='AnswerContent' id='AnswerContent'><option value='1'>Женат(Замужем)</option><option value='0'>Холост(Не замужем)</option></select>"*/
                    });
            }

            if (!context.Questions.Any())
            {
                context.Questions.AddRange(new Question
                    {
                        TypeId = 2,
                        QuestionContent = "Введите имя"
                    },
                    new Question
                    {
                        TypeId = 1,
                        QuestionContent = "Введите возраст"
                    },
                    new Question
                    {
                        TypeId = 5,
                        QuestionContent = "Введите пол"
                    },
                    new Question
                    {
                        TypeId = 3,
                        QuestionContent = "Введите дату рождения"
                    },
                    new Question
                    {
                        TypeId = 6,
                        QuestionContent = "Введите семейное положение"
                    },
                    new Question
                    {
                        TypeId = 4,
                        QuestionContent = "Любите ли Вы программировать"
                    });
            }

            context.SaveChanges();
        }
    }
}