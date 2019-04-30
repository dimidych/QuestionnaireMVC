using System.ComponentModel.DataAnnotations;

namespace QuestionnaireMVC.Models
{
    public class MaritalStatus
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}