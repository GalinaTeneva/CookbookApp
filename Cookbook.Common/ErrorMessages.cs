namespace Cookbook.Common
{
    /// <summary>
    /// Provides validation error messages.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Provides validation error messages for recipe properties and required ingredient input.
        /// </summary>
        public static class Recipe
        {
            /* Title Start */
            public const string TitleRequiredMessage = "Please enter a recipe title.";
            public const string TitleMaxLengthMessage = "The recipe title cannot exceed {1} characters.";
            public const string TitleLengthMessage = "The recipe title must be between {2} and {1} characters.";
            /* Title End */

            /* Description Start */
            public const string DescriptionMaxLengthMessage = "The description cannot exceed {1} characters.";
            /* Description End */

            /* Instructions Start */
            public const string InstructionsRequiredMessage = "Please enter the recipe instructions.";
            public const string InstructionsMaxLengthMessage = "The instructions cannot exceed {1} characters.";
            public const string InstructionsLengthMessage = "The instructions must be between {2} and {1} characters.";
            /* Instructions Start */

            /* ImageUrl Start */
            public const string ImageUrlMaxLengthMessage = "The image URL cannot exceed {1} characters.";
            /* ImageUrl End */

            /* CreatedOn Start */
            public const string CreatedOnRequiredMessage = "The recipe creation date is required.";
            /* CreatedOn End */

            /* Category Start */
            public const string CategoryRequiredMessage = "Please select a category.";
            /* Category End */

            /* User Start */
            public const string UserRequiredMessage = "The recipe creator is required.";
            /* User Start */

            public const string IngredientsTextRequiredMessage = "Please enter the recipe ingredients using one quantity | ingredient entry per line.";
        }

        /// <summary>
        /// Provides validation error messages for category properties.
        /// </summary>
        public static class Category
        {
            public const string NameRequiredMessage = "The category name is required.";
            public const string NameMaxLengthMessage = "The category name cannot exceed {1} characters.";
            public const string NameLengthMessage = "The category name must be between {2} and {1} characters.";
        }

        /// <summary>
        /// Provides validation error messages for ingredient properties.
        /// </summary>
        public static class Ingredient
        {
            public const string NameRequiredMessage = "The ingredient name is required.";
            public const string NameMaxLengthMessage = "The ingredient name cannot exceed {1} characters.";
            public const string NameLengthMessage = "The ingredient name must be between {2} and {1} characters.";
        }

        /// <summary>
        /// Provides validation error messages for ingredient quantities and required
        /// recipe and ingredient associations.
        /// </summary>
        public static class RecipeIngredient
        {
            //Quantity Start
            public const string QuantityRequiredMessage = "The ingredient quantity is required.";
            public const string QuantityMaxLengthMessage = "The ingredient quantity cannot exceed {1} characters.";
            public const string QuantityLengthMessage = "The ingredient quantity must be between {2} and {1} characters.";
            //Quantity End

            //Recipe Start
            public const string RecipeRequiredMessage = "The associated recipe is required.";
            //Recipe End
            
            //Ingredient Start
            public const string IngredientRequiredMessage = "The associated ingredient is required.";
            //Ingredient End                       
        }

        /// <summary>
        /// Provides validation error messages for required planned meal information.
        /// </summary>
        public static class PlannedMeal
        {
            public const string RecipeRequiredMessage = "Please select a recipe for the planned meal.";
            public const string UserRequiredMessage = "The planned meal creator is required.";
            public const string PlannedDateRequiredMessage = "Please select a date for the planned meal.";
        }

        /// <summary>
        /// Error messages used in RecipeFormInputModel.Validate
        /// </summary>
        /// <remarks>
        /// Not suitable for using in data annotation validation attributes
        /// </remarks>
        public static class RecipeIngredientInput
        {
            public const string InvalidFormatMessage = "Ingredient on line {0} must use the format: quantity | ingredient.";
            public const string QuantityLengthMessage = "The quantity on line {0} must be between {1} and {2} characters.";
            public const string NameLengthMessage = "The ingredient name on line {0} must be between {1} and {2} characters.";
            public const string DuplicateIngredientMessage = "Ingredient '{0}' on line {1} appears more than once in this recipe.";
        }
    }
}
