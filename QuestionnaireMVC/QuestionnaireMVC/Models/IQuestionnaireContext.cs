using Microsoft.EntityFrameworkCore;

namespace QuestionnaireMVC.Models
{
    public interface IQuestionnaireContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<QuestionType> QuestionTypes { get; set; }
        DbSet<MaritalStatus> MaritalStatuses { get; set; }
        DbSet<Sex> Sexs { get; set; }
    }
}