namespace CnC.Application.Shared.Permissions;

public class Permission
{
    public static class Category
    {
        public const string MainCategoryCreate = "Category.MainCreate";
        public const string SubCategoryCreate = "Category.SubCreate";
        public const string MainCategoryUpdate = "Category.MainUpdate";
        public const string SubCategoryUpdate = "Category.SubUpdate";
        public const string Delete = "Category.Delete";

        public static List<string> All = new()
        {
            MainCategoryCreate,
            SubCategoryCreate,
            MainCategoryUpdate,
            SubCategoryUpdate,
            Delete
        };
    }

    public static class Account
    {
        public const string AssignRole = "Account.AssignRole";


        public static List<string> All = new()
        {
            AssignRole
        };
    }
}
