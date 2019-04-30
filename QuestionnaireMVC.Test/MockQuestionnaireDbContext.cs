using System.Collections.Generic;
using QuestionnaireMVC.Models;

namespace QuestionnaireMVC.Test
{
    public class MockQuestionnaireDbContext
    {
        public MockQuestionnaireDbContext()
        {
            MaritalStatuses = new List<MaritalStatus>
            {
                new MaritalStatus
                {
                    Id = 1,
                    Name = "Холост(Не замужем)"
                },

                new MaritalStatus
                {
                    Id = 2,
                    Name = "Женат(Замужем)"
                }
            };
            QuestionTypes = new List<QuestionType>
            {
                new QuestionType
                {
                    TypeId = 1,
                    TypeName = "int"
                },
                new QuestionType
                {
                    TypeId = 2,
                    TypeName = "string"
                },
                new QuestionType
                {
                    TypeId = 3,
                    TypeName = "date"
                },
                new QuestionType
                {
                    TypeId = 4,
                    TypeName = "bool"
                },
                new QuestionType
                {
                    TypeId = 5,
                    TypeName = "sexEnum"
                },
                new QuestionType
                {
                    TypeId = 6,
                    TypeName = "maritalStatusEnum"
                }
            };
            Questions = new List<Question>
            {
                new Question
                {
                    QuestionId = 1,
                    TypeId = 2,
                    QuestionContent = "Введите имя"
                },
                new Question
                {
                    QuestionId = 2,
                    TypeId = 1,
                    QuestionContent = "Введите возраст"
                },
                new Question
                {
                    QuestionId = 3,
                    TypeId = 5,
                    QuestionContent = "Введите пол"
                },
                new Question
                {
                    QuestionId = 4,
                    TypeId = 3,
                    QuestionContent = "Введите дату рождения"
                },
                new Question
                {
                    QuestionId = 5,
                    TypeId = 6,
                    QuestionContent = "Введите семейное положение"
                },
                new Question
                {
                    QuestionId = 6,
                    TypeId = 4,
                    QuestionContent = "Любите ли Вы программировать"
                }
            };
            Sexs = new List<Sex>
            {
                new Sex
                {
                    Id = 1,
                    Name = "Женский"
                },
                new Sex
                {
                    Id = 2,
                    Name = "Мужской"
                }
            };
            Answers=new List<Answer>();
        }

        public List<Answer> Answers { get; set; }

        public List<MaritalStatus> MaritalStatuses { get; set; }

        public List<QuestionType> QuestionTypes { get; set; }

        public List<Question> Questions { get; set; }

        public List<Sex> Sexs { get; set; }
    }
}