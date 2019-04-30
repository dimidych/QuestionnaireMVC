using System.ComponentModel.DataAnnotations;

namespace QuestionnaireMVC.Models
{
    public class Question
    {
        [Key] public int QuestionId { get; set; }

        public string QuestionContent { get; set; }

        public int TypeId { get; set; }

        public QuestionType QuestionType { get; set; }
    }
}