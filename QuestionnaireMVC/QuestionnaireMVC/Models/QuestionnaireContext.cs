using Microsoft.EntityFrameworkCore;

namespace QuestionnaireMVC.Models
{
    public class QuestionnaireContext : DbContext, IQuestionnaireContext
    {
        /// <summary>
        /// В конструкторе проверяем наличие БД. Если нет, то БД создается
        /// </summary>
        public QuestionnaireContext(DbContextOptions<QuestionnaireContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<MaritalStatus> MaritalStatuses { get; set; }

        public DbSet<Sex> Sexs { get; set; }
    }
}