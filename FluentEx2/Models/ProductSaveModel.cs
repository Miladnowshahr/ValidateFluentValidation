
using FluentValidation;

namespace FluentEx2.Models
{
    public class ProductSaveModel : IActionRequest
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
    }

    public class ProductSaveModelValidation : AbstractValidator<ProductSaveModel>
    {
        public ProductSaveModelValidation()
        {
            RuleFor(r => r.Name).NotEmpty().NotNull();
            RuleFor(r => r.ImagePath).NotNull().NotEmpty();
            RuleFor(r => r.CategoryName).NotEmpty().NotNull();
        }
    }
    public interface IActionRequest
    {

    }
}

