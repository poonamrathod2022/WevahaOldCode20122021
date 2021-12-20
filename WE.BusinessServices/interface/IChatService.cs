using System.Collections.Generic;
using WE.BusinessEntities;

namespace WE.BusinessServices.Interface
{
    public interface IChatService
    {
        List<MessageUserList> GetChatUsersMsgCountResult(int userId);
        List<MessageHistory> GetMessageList(MessageListModel messageListModel);
        bool StarUser(StarUserRequest starUserRequest);
        bool UnreadUserMessage(ActionMessageRequest unreadMessageRequest);
        bool DeleteUserConversation(ActionMessageRequest deleteMessageRequest, string path);
        bool ReportMessage(int messageId, out ReportUserModel reportUserModel);
        bool DeleteLabel(long labelId);
        long AddLabel(AddLabelRequest addLabelRequest);
        bool LabelCheck(AddUserLabelRequest addUserLabelRequest);
        long AddQuickResponse(AddQuickResponseRequest addQuickResponseRequest);
        bool DeleteQuickResponse(int resId);
        List<AddQuickResponseResponse> ListOfUserQuickResponse(int userId);
        bool SendMessage(MessageObject messageObject);
        bool BlockMessage(int messageId, bool isBlock);
        bool ArchiveUser(ArchiveUserRequest archiveUserRequest);
      
    }
}