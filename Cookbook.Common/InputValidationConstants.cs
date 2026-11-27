namespace Cookbook.Common
{
    /// <summary>
    /// Provides input validation constants.
    /// </summary>
    public static class InputValidationConstants
    {
        public static class RecipeValidation
        {
            /* TITLE START */
            /// <summary>
            /// Recipe Title min length
            /// </summary>
            public const int TitleMinLength = 3;
            /// <summary>
            /// Recipe Title max length
            /// </summary>
            public const int TitleMaxLength = 100;
            /* TITLE END */

            /* DESCRIPTION START */
            /// <summary>
            /// Recipe Description max length
            /// </summary>
            public const int DescriptionMaxLength = 100;
            /* DESCRIPTION END */

            /* INSTRUCTIONS START */
            /// <summary>
            /// Recipe Instructions min length
            /// </summary>
            public const int InstructionsMinLength = 3;
            /// <summary>
            /// Recipe Instructions max length
            /// </summary>
            public const int InstructionsMaxLength = 4000;
            /* INSTRUCTIONS END */

            /* IMAGE_URL START */
            /// <summary>
            /// Recipe ImageUrl max length
            /// </summary>
            public const int ImageUrlMaxLength = 200;
            /* IMAGE_URL END */
        }

        public static class CategoryValidation
        {
            /* NAME START */
            /// <summary>
            /// Category Name min length
            /// </summary>
            public const int NameMinLength = 2;
            /// <summary>
            /// Category Name max length
            /// </summary>
            public const int NameMaxLength = 50;
            /* NAME END */
        }

        public static class IngredientValidation
        {
            /* NAME START */
            /// <summary>
            /// Ingredient name min length
            /// </summary>
            public const int NameMinLength = 2;
            /// <summary>
            /// Ingredient name max length
            /// </summary>
            public const int NameMaxLength = 70;
            /* NAME END */
        }

        public static class RecipeIngredientValidation
        {
            /* QUANTITY START */
            /// <summary>
            /// Recipe Ingredient Quantity min length
            /// </summary>
            public const int IngredientQuantityMinLength = 1;
            /// <summary>
            /// Recipe Ingredient Quantity max length
            /// </summary>
            public const int IngredientQuantityMaxLength = 70;
            /* QUANTITY END */
        }
    }
}
