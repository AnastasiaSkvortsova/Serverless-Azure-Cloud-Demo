using System.ComponentModel.DataAnnotations;

namespace Ana.ToDo.FunctionApp
{
    public class ToDoItem
    {
        public int Id {get; set;}

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name {get; set;}
        [Required]
        public bool IsComplete {get; set;}

    }
}