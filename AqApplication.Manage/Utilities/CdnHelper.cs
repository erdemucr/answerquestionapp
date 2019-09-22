
using Cloudinary;

namespace AnswerQuestionApp.Manage.Utilities
{
    public static class CdnHelper
    {
        public static Cloudinary.Uploader GetUploader()
        {
            var cloudinary = new Cloudinary.Uploader(new AccountConfiguration("duy1lacup", "553468819898758", "n3TlFL_L_65xjuYLCIuBgDdqf5c"));
            return cloudinary;
        }
    }
}
