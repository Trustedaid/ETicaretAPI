using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ETicaretAPI.Application.Validators.Products;

public class CreateProductValidator : AbstractValidator<VMCreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Name is required")
            .MaximumLength(50).MinimumLength(2)
            .WithMessage("Please enter a valid name between 2 and 50 characters");
        
        RuleFor(x => x.Stock).NotEmpty().NotNull().WithMessage("Stock is required")
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to 0");

        RuleFor(x => x.Price).NotEmpty().NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}