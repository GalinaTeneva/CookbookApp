using Cookbook.Web.ViewModels.Genre;
using static Cookbook.Common.EntityValidation.RecipeEntityValidation;
using static Cookbook.Common.EntityValidation.IngredientEntityValidation;
using System.ComponentModel.DataAnnotations;

namespace Cookbook.Web.ViewModels.Recipe
{
    public abstract class RecipeFormInputModel : IValidatableObject
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [StringLength(InstructionsMaxLength, MinimumLength = InstructionsMinLength)]
        public string Instructions { get; set; } = null!;

        [StringLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        [Required]
        public string IngredientsText { get; set; } = null!;

        public int CategoryId { get; set; }

        public IEnumerable<AllCategoriesViewModel> Categories { get; set; } = new List<AllCategoriesViewModel>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string[] lines = IngredientsText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');


            var ingredientNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] parts = lines[i].Split('|', StringSplitOptions.TrimEntries);

                if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    yield return new ValidationResult($"Ingredient on line {i + 1} must use teh propper format.", new[] { nameof(IngredientsText) });

                    continue;
                }

                string quantity = parts[0];
                string ingredientName = parts[1];

                if (quantity.Length > IngredientQuantityMaxLength)
                {
                    yield return new ValidationResult($"The quantity on line {i + 1} cannot exceed {IngredientQuantityMaxLength} characters.",
                        new[] { nameof(IngredientsText) });
                }

                if (ingredientName.Length < NameMinLength || ingredientName.Length > NameMaxLength)
                {
                    yield return new ValidationResult(
                        $"The ingredient name on line {i + 1} must be between {NameMinLength} and {NameMaxLength} characters.",
                        new[] { nameof(IngredientsText) });
                }

                if (!ingredientNames.Add(ingredientName))
                {
                    yield return new ValidationResult($"Ingredient '{ingredientName}' on line {i + 1} appears more than once in this recipe.",
                        new[] { nameof(IngredientsText) });
                }
            }
        }
    }
}
