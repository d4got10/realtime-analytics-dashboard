using DataCollection.API.DTOs;
using FastEndpoints;
using FluentValidation;

namespace DataCollection.API.Validation;

public class DataPostDtoValidator : Validator<DataPostDto>
{
    public DataPostDtoValidator()
    {
        RuleFor(x => x.EventName)
            .NotEmpty()
            .WithMessage("event name is required!")
            .MinimumLength(3)
            .WithMessage("event name is too small!")
            .MaximumLength(255)
            .WithMessage("event name is too large!");
    }
}