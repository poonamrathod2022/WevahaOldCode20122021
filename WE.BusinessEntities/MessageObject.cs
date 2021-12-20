namespace WE.BusinessEntities
{
    public class MessageObject
    {
        public int touserId { get; set; }

        public int fromuserId { get; set; }

        public string message { get; set; }

        public string fileName { get; set; }

        public string filePath { get; set; }
        public string IPAddress { get; set; }
    }

    public class StarUserRequest
    {
        public int TouserId { get; set; }

        public int FromuserId { get; set; }

        public bool IsStar { get; set; }
    }

    public class ActionMessageRequest
    {
        public int TouserId { get; set; }

        public int FromuserId { get; set; }

    }

    public class AddLabelRequest
    {
        public int UserId { get; set; }

        public string LabelName { get; set; }

    }

    public class AddUserLabelRequest
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public long LabelId { get; set; }

        public bool IsCheck { get; set; }

    }

    public class AddQuickResponseRequest
    {
        public int QuickResponseId { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }

    public class AddQuickResponseResponse
    {
        public long QuickResId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }

    public class ArchiveUserRequest
    {
        public int TouserId { get; set; }

        public int FromuserId { get; set; }

        public bool IsArchive { get; set; }
    }
}