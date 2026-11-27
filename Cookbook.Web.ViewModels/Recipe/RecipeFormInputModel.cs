using Cookbook.Web.ViewModels.Genre;
using System.ComponentModel.DataAnnotations;

using static Cookbook.Common.InputValidationConstants.RecipeValidation;
using static Cookbook.Common.InputValidationConstants.IngredientValidation;
using static Cookbook.Common.InputValidationConstants.RecipeIngredientValidation;
using static Cookbook.Common.ErrorMessages.Recipe;
using static Cookbook.Common.ErrorMessages.RecipeIngredientInput;

namespace Cookbook.Web.ViewModels.Recipe
{
    public abstract class RecipeFormInputModel : IValidatableObject
    {
        [Required(ErrorMessage = TitleRequiredMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthMessage)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = InstructionsRequiredMessage)]
        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength, ErrorMessage = InstructionsLengthMessage)]
        public string Instructions { get; set; } = null!;

        [StringLength(ImageUrlMaxLength, ErrorMessage = ImageUrlMaxLengthMessage)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = IngredientsTextRequiredMessage)]
        public string IngredientsText { get; set; } = null!;

        [Required(ErrorMessage = CategoryRequiredMessage)]
        public int CategoryId { get; set; }

        public IEnumerable<AllCategoriesViewModel> Categories { get; set; } = new List<AllCategoriesViewModel>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(IngredientsText))
                yield break;

            string[] lines = IngredientsText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');

            var ingredientNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] parts = lines[i].Split('|', StringSplitOptions.TrimEntries);

                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    yield return new ValidationResult(string.Format(InvalidFormatMessage, i + 1), new[] {nameof(IngredientsText)});

                    continue;
                }

                string quantity = parts[0];
                string ingredientName = parts[1];

                if (quantity.Length < IngredientQuantityMinLength || quantity.Length > IngredientQuantityMaxLength)
                {
                    yield return new ValidationResult(string.Format(QuantityLengthMessage, (i + 1), IngredientQuantityMinLength, IngredientQuantityMaxLength),
                        new[] { nameof(IngredientsText) });
                }

                if (ingredientName.Length < NameMinLength || ingredientName.Length > NameMaxLength)
                {
                    yield return new ValidationResult(
                        string.Format(NameLengthMessage, (i+1), NameMinLength, NameMaxLength),
                        new[] { nameof(IngredientsText) });
                }

                if (!ingredientNames.Add(ingredientName))
                {
                    yield return new ValidationResult(string.Format(DuplicateIngredientMessage, ingredientName, (i+1)),
                        new[] { nameof(IngredientsText) });
                }
            }
        }
    }
}
