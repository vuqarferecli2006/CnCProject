namespace CnC.Application.Shared.Permissions;

public class Permission
{
    public static class Category
    {
        public const string MainCategoryCreate = "Category.MainCreate";
        public const string SubCategoryCreate = "Category.SubCreate";
        public const string MainCategoryUpdate = "Category.MainUpdate";
        public const string SubCategoryUpdate = "Category.SubUpdate";
        public const string MainCategoryDelete = "Category.MainDelete";
        public const string SubCategoryDelete = "Category.SubDelete";

        public static List<string> All = new()
        {
            MainCategoryCreate,
            SubCategoryCreate,
            MainCategoryUpdate,
            SubCategoryUpdate,
            SubCategoryDelete,
            MainCategoryDelete
        };
    }
    public static class AskedQuestions
    {
        public const string AskedQuestionsCreate = "AskedQuestions.Create";
        public const string AskedQuestionsUpdate = "AskedQuestions.Update";

        public static List<string> All = new()
        {
            AskedQuestionsCreate,
            AskedQuestionsUpdate,
        };
    }

    public static class Basket
    {
        public const string AddBasket = "Basket.Add";
        public const string DeleteBasket = "Basket.Delete";
        public const string GetAllBasket = "Basket.GetAll";
        public static List<string> All = new()
        {
            AddBasket,
            DeleteBasket,
            GetAllBasket
        };
    }
    public static class Bio
    {
        public const string CreateBio = "Bio.Create";
        public const string UpdateBio = "Bio.Update";
        public const string GetAll= "Bio.Create";
        public static List<string> All = new()
        {
            CreateBio,
            UpdateBio,
            GetAll
        };
    }
    public static class Download
    {
        public const string GetAllDownload = "Download.GetAll";
        public const string GetByIdDownload = "Download.GetById";
        public static List<string> All = new()
        {
            GetAllDownload,
            GetByIdDownload,
        };
    }
    public static class Favourite
    {
        public const string AddProductFavourite = "Favourite.Add";
        public const string RemoveProductFavourite = "Favourite.Remove";
        public const string GetAllProductFavourite = "Favourite.GetAll";
        public static List<string> All = new()
        {
            AddProductFavourite,
            RemoveProductFavourite,
            GetAllProductFavourite
        };
    }
    public static class InformationModel
    {
        public const string AddInformationModel = "InformationModel.Add";
        public const string UpdateInformationModel = "InformationModel.Update";
        public static List<string> All = new()
        {
            AddInformationModel,
            UpdateInformationModel,
        };
    }
    public static class Order
    {
        public const string CreateOrder = "Order.Create";
        public const string ChooseProductForOrder = "Order.ChooseProduct";
        public const string CancelProductForOrder = "Order.CancelProduct";
        public const string GetAllOrder = "Order.GetAll";
        public const string GetPaidOrder = "Order.GetPaid";
        public static List<string> All = new()
        {
            CreateOrder,
            ChooseProductForOrder,
            CancelProductForOrder,
            GetAllOrder,
            GetPaidOrder
        };
    }
    public static class Payment
    {
        public const string ChoosePaymentMethod = "Payment.ChooseMethod";
        public const string PaymentCreate = "Payment.Create";
        public static List<string> All = new()
        {
            ChoosePaymentMethod,
            PaymentCreate,
        };
    }
    public static class Product
    {
        public const string CreateProduct = "Product.Create";
        public const string UpdateProduct = "Product.Update";
        public const string CreateProductDescription = "ProductDescription.Create";
        public const string UpdateProductDescription = "ProductDescription.Update";
        public const string DeleteProduct = "Product.Create";

        public static List<string> All = new()
        {
            CreateProduct,
            UpdateProduct,
            CreateProductDescription,
            DeleteProduct,
            UpdateProductDescription
        };
    }
    public static class Role
    {
        public const string CreateRole = "Role.Create";
        public const string AssignRole = "Role.Assign";

        public static List<string> All = new()
        {
            CreateRole,
            AssignRole,
        };
    }
    public static class User
    {
        public const string LogOut = "User.LogOut";
        public const string ResetPasssword = "User.ResetPassword";
        public const string ChangePasssword = "User.ChangePassword";
        public const string ProfileImageUpdated = "User.ProfileUpdate";
        public static List<string> All = new()
        {
            LogOut,
            ResetPasssword,
            ChangePasssword,
            ProfileImageUpdated
        };
    }
    public static class UserCreationByAdmin
    {
        public const string CreateUserByAdmin = "UserByAdmin.Create";
        public static List<string> All = new()
        {
            CreateUserByAdmin,
        };
    }
}
