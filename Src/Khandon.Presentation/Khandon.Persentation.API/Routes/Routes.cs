namespace Khandon.Persentation.API
{
    public struct Routes
    {
        public struct V1
        {
            public struct Admin
            {
                public struct BookGroup
                {
                    public const string GetAllBookGroup = "V1/BookGroup/GetAllBookGroup";
                    public const string CreateBookGroup = "V1/BookGroup/CreateBookGroup";
                    public const string EditBookGroup = "V1/BookGroup/EditBookGroup";
                }

            }
            public struct User
            {
                public struct Users
                {
                    public const string SignUpUser = "V1/Users/SignUpUser";
                    public const string LoginInUser = "V1/Users/LoginInUser";
                    public const string ConfirmAccount = "V1/Users/ConfirmAccount";
                    //----------Not Implemented----------
                    public const string RestPasssword = "V1/Users/RestPasssword";
                    public const string RecoveryPassword = "V1/Users/RecoveryPassword";
                    public const string RecoveryPasswordConfirm = "V1/Users/RecoveryPasswordConfirm";

                }
                public struct Book
                {
                    public const string GetBookList = "V1/Book/GetBookList";
                    public const string CreateBook = "V1/Book/CreateBook";
                    public const string EditBook = "V1/Book/EditBook/{Id}";
                    public const string DeleteBook = "V1/Book/EditBook/{Id}";
                }

                public struct Chapter
                {
                    public const string GetChapterById = "V1/Chapter/GetChapterById/{Id}";
                    public const string GetChapterBookId = "V1/Chapter/GetChapterBookId";
                    public const string CreateChapter = "V1/Chapter/CreateChapter";
                    public const string EditChapter = "V1/Chapter/EditChapter";
                    public const string DeleteChapter = "V1/Chapter/DeleteChapter/{Id}";
                }

                public struct Study
                {
                    public const string GetStudyById = "V1/Study/GetStudyById/{Id}";
                    public const string GetPeriodTimeSocityActivity = "V1/Study/GetPeriodTimeSocityActivity";
                    public const string GetStudy= "V1/Study/GetStudyByGroupId";
                    public const string GetSocietyStatus = "V1/Study/GetSocietyStatus";
                    public const string CreateStudy = "V1/Study/CreateStudy";
                    public const string EditStudy = "V1/Study/EditStudy";
                    public const string DeleteStudy = "V1/Study/DeleteStudy/{Id}";
                }
            }
        }
    }
}
