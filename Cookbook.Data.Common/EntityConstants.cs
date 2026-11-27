namespace Cookbook.Data.Common
{
    /// <summary>
    /// Defines maximum property lengths and database column types.
    /// </summary>
    public static class EntityConstants
    {
        public static class RecipeEntity
        {
            public const int TitleMaxLength = 100;

            public const int DescriptionMaxLength = 100;

            public const int InstructionsMaxLength = 4000;

            public const int ImageUrlMaxLength = 200;
        }

        public static class CategoryEntity
        {
            public const int NameMaxLength = 50;
        }

        public static class IngredientEntity
        {            
            public const int NameMaxLength = 70;
        }

        public static class RecipeIngredientEntity
        {
            public const int IngredientQuantityMaxLength = 70;
        }

        public static class PlannedMealEntity
        {
            public const string PlannedDateColumnType = "date";
        }
    }
}
