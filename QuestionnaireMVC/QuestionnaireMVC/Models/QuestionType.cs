using System.ComponentModel.DataAnnotations;

namespace QuestionnaireMVC.Models
{
    public class QuestionType
    {
        [Key]
        public int TypeId { get; set; }

        public string TypeName { get; set; }
    }
}