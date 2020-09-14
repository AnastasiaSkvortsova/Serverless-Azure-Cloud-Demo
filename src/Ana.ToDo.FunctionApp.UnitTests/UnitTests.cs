using System;
using Ana.ToDo.FunctionApp;
using Xunit;

namespace Ana.Todo.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void ValidationFailsWithEmptyName()
        {
            ToDoValidator validator = new ToDoValidator();

            var toDoItem = new ToDoItem{
                Name = "",
                IsComplete = false
            };

            var result = validator.Validate(toDoItem);
            
            Assert.False(result.IsValid);
        }
        
        [Fact]
        public void ValidationFailsWithNullName()
        {
            ToDoValidator validator = new ToDoValidator();

            var toDoItem = new ToDoItem{
                Name = null,
                IsComplete = false
            };

            var result = validator.Validate(toDoItem);
            
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidationFailsWithTooShortName()
        {
            ToDoValidator validator = new ToDoValidator();

            var toDoItem = new ToDoItem{
                Name = "a",
                IsComplete = false
            };

            var result = validator.Validate(toDoItem);
            
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidationFailsWithTooLongName()
        {
            ToDoValidator validator = new ToDoValidator();

            var toDoItem = new ToDoItem{
                Name = "kVYrsidknLPTGalHyAbOpDOk6NBPXt22iTQyuTUagBKqPyvqyrsQqJxbbmkcq",
                IsComplete = false
            };

            var result = validator.Validate(toDoItem);
            
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidationFailsWithNoCompletionStatus()
        {
            ToDoValidator validator = new ToDoValidator();

            var toDoItem = new ToDoItem{
                Name = "kVYrsidknLPTGalHyAbOpDOk6NBPXt22iTQyuTUagBKqPyvqyrsQqJxbbmkcq"
            };

            var result = validator.Validate(toDoItem);
            
            Assert.False(result.IsValid);
        }
    }
}
