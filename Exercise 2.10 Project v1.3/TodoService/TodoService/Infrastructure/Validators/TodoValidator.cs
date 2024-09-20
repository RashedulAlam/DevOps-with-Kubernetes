using FluentValidation;
using TodoService.Business.Model;

namespace TodoService.Infrastructure.Validators
{
    public class TodoValidator : AbstractValidator<TodoCreateRequest>
    {
        public TodoValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(140).WithMessage("Maximum length is 140");
        }
    }
}
