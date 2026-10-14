namespace CookbookApp.Common
{
    public static class EntityValidation
    {
        public static class RecipeEntityValidation
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

        public static class CategoryEntityValidation
        {
            //Name
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class IngredientEntityValidation
        {
            //Name
            public const int NameMinLength = 2;
            public const int NameMaxLength = 70;

            //Quantity
            public const int IngredientQuantityMinLength = 2;
            public const int IngredientQuantityMaxLength = 30;
        }
    }
}
