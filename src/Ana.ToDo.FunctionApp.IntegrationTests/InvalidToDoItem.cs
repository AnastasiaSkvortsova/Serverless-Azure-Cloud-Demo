using System.ComponentModel.DataAnnotations;

namespace Ana.ToDo.FunctionApp.IntegrationTests
{
    public class InvalidToDoItem
    {
        public int Id {get; set;}

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name {get; set;}
        [Required]
        public bool IsComplete {get; set;}

    }
}