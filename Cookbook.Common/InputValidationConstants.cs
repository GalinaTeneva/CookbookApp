namespace Cookbook.Common
{
    public static class InputValidationConstants
    {
        public static class RecipeValidation
        {
            //Title
            public const int TitleMinLength = 3;
            public const int TitleMaxLength = 100;

            //Description
            public const int DescriptionMaxLength = 100;

            //Instructions
            public const int InstructionsMinLength = 3;
            public const int InstructionsMaxLength = 4000;

            //ImageUrl
            public const int ImageUrlMaxLength = 200;
        }

        public static class CategoryValidation
        {
            //Name
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class IngredientValidation
        {
            //Name
            public const int NameMinLength = 2;
            public const int NameMaxLength = 70;

            //Quantity
            public const int IngredientQuantityMinLength = 2;
            public const int IngredientQuantityMaxLength = 70;
        }
    }
}
