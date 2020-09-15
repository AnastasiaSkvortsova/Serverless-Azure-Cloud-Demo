using FluentValidation;

namespace Ana.ToDo.FunctionApp
{
    public class ToDoValidator : AbstractValidator<ToDoItem>
    {
        public ToDoValidator()
        {
            RuleFor(ToDoItem => ToDoItem.Name).NotEmpty();
            RuleFor(ToDoItem => ToDoItem.Name).Length(3, 60);
        }
    }

}