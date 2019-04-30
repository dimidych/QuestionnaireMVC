using System.ComponentModel.DataAnnotations;

namespace QuestionnaireMVC.Models
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public int RespondentId { get; set; }

        public string AnswerContent { get; set; }

        public Question Question { get; set; }
    }
}