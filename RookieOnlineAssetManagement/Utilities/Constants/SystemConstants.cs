namespace RookieOnlineAssetManagement.Utilities.Constants
{
    public class SystemConstants
    {
        //Common
        public const string APIReturnPermissionDenied = "Permission Denied";

        //User
        public const string APIUpdateUserReturnSuccess = "Update Success";
        public const string APIUpdateUserReturnError = "Update Failed";
        public const string APIFindUserByIdErrorInValidId = "Can not find user";

        //Asset
        public const string APIFindAssetByIdErrorInValidId = "Can not find asset";
        public const string APIUpdateAssetReturnSuccess = "Update Asset Success";
        public const string APIUpdateAssetReturnError = "Update Asset Failed";

        //Assignment
        public const string APIFindAssignmentByIdErrorInValidId = "Can not find assignment";
        public const string APIUpdateAssignmentReturnSuccess = "Update Assignment Success";
        public const string APIUpdateAssignmentReturnError = "Update Assignment Failed";

        //Return Asset
        public const string APICreateReturnAssetSuccess = "Request return asset successfully";
    }
}