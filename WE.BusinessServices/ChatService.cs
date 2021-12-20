using System.Collections.Generic;
using System.Linq;
using WE.BusinessServices.Interface;
using WE.DataAccess;
using WE.BusinessEntities;
using System;
using System.IO;

namespace WE.BusinessServices
{
    public class ChatService : IChatService
    {
        private WEvahaEntities _entities = new WEvahaEntities();
        DashBoardService dashboardService = new DashBoardService();

        public List<MessageUserList> GetChatUsersMsgCountResult(int userId)
        {
            var chatUsers = _entities.USP_WE_GETCHATUSERWITHMSGCOUNT(userId).OrderByDescending(p => p.CreatedDate).ToList();

            var messageUsers = new List<MessageUserList>();
            var userLabel = _entities.TBL_MESSAGELABELS.Where(x => x.UserId == userId).Select(s => new LabelUserList()
            {
                LabelId = s.MessageLabelId,
                LabelName = s.LabelName,
                CanDelete = (bool)s.CanDelete
            }).ToList();
            foreach (var item in chatUsers)
            {
                if (item.ProfileId > 0)
                {
                    UserProfilePic userProfilePic = dashboardService.GetProfilePic(userId);
                    //UserProfile userProfile = dashboardService.GetProfiledetails(userId);
                    var touserLabelId = _entities.TBL_MESSAGEUSERLABELS.Where(x => x.FromUserId == userId && x.ToUserId == item.ProfileId).Select(x => x.MessageLabelId).ToList();
                    var touserlabelResponse = new List<LabelUserList>();
                    foreach (var labelId in touserLabelId)
                    {
                        touserlabelResponse.Add(userLabel.FirstOrDefault(x => x.LabelId == labelId));
                    }
                    TBL_MESSAGEARCHIVE Archive = _entities.TBL_MESSAGEARCHIVE.FirstOrDefault(x => x.FromUserId == userId && x.ToUserId == item.ProfileId);
                    messageUsers.Add(new MessageUserList()
                    {
                        ProfileId = item.ProfileId,
                        ProfileName = item.ProfileName,
                        UserProfilePic = dashboardService.GetProfilePic(item.ProfileId),
                        UnreadMsg = item.UnreadMsg,
                        LastMessageDate = item.CreatedDate,
                        LastMessage = item.LastMessageDetail,
                        LastSeenStringDate = item.LastSeenDate,
                        CurrentUserProfileURL = userProfilePic,
                        CurrentUserProfileName = "Me",//userProfile.ProfileName,
                        IsStar = item.IsStar,
                        CurrentUserLabel = userLabel,
                        ToUserLabel = touserlabelResponse.Count > 0 ? touserlabelResponse : null,
                        IsArchive = Archive != null && Archive.IsArchive,
                        IsReported = item.IsReported,
                        IsSpam = item.IsSpam
                    });
                }
            }

            return messageUsers;
        }

        public List<MessageHistory> GetMessageList(MessageListModel messageListModel)
        {
            var getmessageHistory = _entities.USP_WE_GETMESSAGEHISTORYBYID(messageListModel.FromUserId, messageListModel.ToUserId, messageListModel.PageIndex = 1, messageListModel.PageSize = 1000).ToList();

            var messageHistories = new List<MessageHistory>();
            foreach (var item in getmessageHistory)
            {
                if (item.FromUserId > 0)
                {
                    messageHistories.Add(new MessageHistory()
                    {
                        MessageId = item.MessageHistoryId,
                        FromUserId = (int)item.FromUserId,
                        ToUserId = item.ToUserId,
                        UserProfilePic = dashboardService.GetProfilePic(item.FromUserId),
                        FromUserName = item.FromUserName,
                        MessageDate = item.MessageDate,
                        MessageDetail = item.MessageDetail,
                        TotalRowCount = item.TotalRowCount,
                        CreatedDate = item.CreatedDate,
                        FileName = item.FileName,
                        IsBlock = item.IsSpam,
                        IsReported = item.IsReported,
                        FileUniqueId = item.FileUniqueId,
                        IsStar = item.IsStar
                    });
                }
            }

            return messageHistories;
        }

        public bool StarUser(StarUserRequest starUserRequest)
        {
            try
            {
                var starResponse = _entities.TBL_MESSAGESTARS.FirstOrDefault(x => x.FromUserId == starUserRequest.FromuserId && x.ToUserId == starUserRequest.TouserId);
                if (starResponse != null)
                {
                    starResponse.IsStar = starUserRequest.IsStar;
                }
                else
                {
                    _entities.TBL_MESSAGESTARS.Add(new TBL_MESSAGESTARS()
                    {
                        FromUserId = starUserRequest.FromuserId,
                        ToUserId = starUserRequest.TouserId,
                        IsStar = starUserRequest.IsStar,
                        CreatedDate = DateTime.Now,
                        CreatedBy = starUserRequest.FromuserId
                    });
                }
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UnreadUserMessage(ActionMessageRequest unreadMessageRequest)
        {
            try
            {
                var unreadMessage = _entities.TBL_MESSAGEHISTORY.Where(x => x.FromUserId == unreadMessageRequest.FromuserId && x.ToUserId == unreadMessageRequest.TouserId).ToList();

                foreach (var item in unreadMessage)
                {
                    item.IsMessageRead = false;
                }
                _entities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteUserConversation(ActionMessageRequest deleteMessageRequest, string path)
        {
            try
            {
                var messageList = _entities.TBL_MESSAGEHISTORY.Where(x => (x.FromUserId == deleteMessageRequest.FromuserId && x.ToUserId == deleteMessageRequest.TouserId) || (x.FromUserId == deleteMessageRequest.TouserId && x.ToUserId == deleteMessageRequest.FromuserId)).ToList();

                foreach (var item in messageList)
                {
                    var fileAttchments = _entities.TBL_MESSAGEATTACHMENTS.Where(x => x.MessageHistoryId == item.MessageHistoryId).FirstOrDefault();
                    if (fileAttchments != null)
                    {
                        FileInfo file = new FileInfo(Path.Combine(path, fileAttchments.FileUniqueId));
                        if (file.Exists)
                            file.Delete();

                        _entities.TBL_MESSAGEATTACHMENTS.Remove(fileAttchments);
                    }
                }
                _entities.SaveChanges();

                _entities.TBL_MESSAGEHISTORY.RemoveRange(messageList);
                _entities.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool ReportMessage(int messageId, out ReportUserModel reportUserModel)
        {
            reportUserModel = new ReportUserModel();
            try
            {
                var messageList = _entities.TBL_MESSAGEHISTORY.Where(x => x.MessageHistoryId == messageId).FirstOrDefault();

                if (messageList != null)
                {
                    messageList.IsReported = true;
                    _entities.SaveChanges();

                    UserProfile userProfile = dashboardService.GetProfiledetails(messageList.FromUserId);
                    UserProfile userProfile1 = dashboardService.GetProfiledetails(messageList.ToUserId);
                    reportUserModel = new ReportUserModel
                    {
                        ProfileId = "WV00" + userProfile.ProfileId,
                        UserName = userProfile.ProfileName + " " + (string.IsNullOrEmpty(userProfile.ProfileLastName) ? "" : userProfile.ProfileLastName),
                        ReportMessage = messageList.MessageDetail,
                        ToUserProfileId = "WV00" + userProfile1.ProfileId,
                        UserEmail = string.IsNullOrEmpty(userProfile1.Email) ? "" : userProfile1.Email
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool DeleteLabel(long labelId)
        {
            try
            {
                var label = _entities.TBL_MESSAGELABELS.Where(x => x.MessageLabelId == labelId).FirstOrDefault();

                if (label != null)
                {
                    _entities.TBL_MESSAGELABELS.Remove(label);
                    _entities.TBL_MESSAGEUSERLABELS.RemoveRange(_entities.TBL_MESSAGEUSERLABELS.Where(x => x.MessageLabelId == labelId).ToList());
                    _entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public long AddLabel(AddLabelRequest addLabelRequest)
        {
            try
            {
                if (addLabelRequest != null)
                {
                    var addLabel = new TBL_MESSAGELABELS
                    {
                        UserId = addLabelRequest.UserId,
                        LabelName = addLabelRequest.LabelName,
                        CanDelete = true,
                        IsDelete = false,
                        CreatedBy = addLabelRequest.UserId,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = addLabelRequest.UserId,
                        ModifiedDate = DateTime.Now,
                    };

                    _entities.TBL_MESSAGELABELS.Add(addLabel);
                    _entities.SaveChanges();
                    return addLabel.MessageLabelId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public bool LabelCheck(AddUserLabelRequest addUserLabelRequest)
        {
            try
            {
                if (addUserLabelRequest != null)
                {
                    if (!addUserLabelRequest.IsCheck)
                    {
                        var alredyExits = _entities.TBL_MESSAGEUSERLABELS.Where(x => x.MessageLabelId == addUserLabelRequest.LabelId && x.FromUserId == addUserLabelRequest.FromUserId && x.ToUserId == addUserLabelRequest.ToUserId).FirstOrDefault();
                        _entities.TBL_MESSAGEUSERLABELS.Remove(alredyExits);
                        _entities.SaveChanges();
                    }
                    else
                    {
                        var userLabel = new TBL_MESSAGEUSERLABELS()
                        {
                            ToUserId = addUserLabelRequest.ToUserId,
                            FromUserId = addUserLabelRequest.FromUserId,
                            MessageLabelId = addUserLabelRequest.LabelId,
                            CreatedBy = addUserLabelRequest.FromUserId,
                            CreatedDate = DateTime.Now
                        };
                        _entities.TBL_MESSAGEUSERLABELS.Add(userLabel);
                        _entities.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public long AddQuickResponse(AddQuickResponseRequest addQuickResponseRequest)
        {
            try
            {
                long resId = 0;
                if (addQuickResponseRequest != null)
                {
                    if (addQuickResponseRequest.QuickResponseId != 0)
                    {
                        var response = _entities.TBL_MESSAGEQUICKRESPONSES.Where(x => x.MessageQuickResId == addQuickResponseRequest.QuickResponseId).FirstOrDefault();
                        if (response != null)
                        {
                            response.Title = addQuickResponseRequest.Title;
                            response.Description = addQuickResponseRequest.Description;
                            _entities.SaveChanges();

                            resId = addQuickResponseRequest.QuickResponseId;
                        }
                        else
                        {
                            resId = 0;
                        }
                    }
                    else
                    {
                        var addResponse = new TBL_MESSAGEQUICKRESPONSES
                        {
                            UserId = addQuickResponseRequest.UserId,
                            Title = addQuickResponseRequest.Title,
                            Description = addQuickResponseRequest.Description,
                            IsDelete = false,
                            CreatedBy = addQuickResponseRequest.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedBy = addQuickResponseRequest.UserId,
                            ModifiedDate = DateTime.Now,
                        };


                        _entities.TBL_MESSAGEQUICKRESPONSES.Add(addResponse);
                        _entities.SaveChanges();
                        resId = addResponse.MessageQuickResId;
                    }

                    return resId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool DeleteQuickResponse(int resId)
        {
            try
            {
                if (resId > 0)
                {
                    var response = _entities.TBL_MESSAGEQUICKRESPONSES.Where(x => x.MessageQuickResId == resId).FirstOrDefault();
                    if (response != null)
                    {
                        _entities.TBL_MESSAGEQUICKRESPONSES.Remove(response);
                        _entities.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<AddQuickResponseResponse> ListOfUserQuickResponse(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    List<AddQuickResponseResponse> addQuickResponseResponse = new List<AddQuickResponseResponse>();
                    var quickResponseList = _entities.TBL_MESSAGEQUICKRESPONSES.Where(x => x.UserId == userId).ToList();
                    if (quickResponseList.Count > 0)
                    {
                        foreach (var item in quickResponseList)
                        {
                            addQuickResponseResponse.Add(new AddQuickResponseResponse()
                            {
                                QuickResId = item.MessageQuickResId,
                                Title = item.Title,
                                Description = item.Description
                            });
                        }
                    }
                    else
                    {
                        addQuickResponseResponse = null;
                    }
                    return addQuickResponseResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SendMessage(MessageObject messageObject)
        {
            try
            {
                _entities.TBL_MESSAGEHISTORY.Add(new TBL_MESSAGEHISTORY
                {
                    CreatedDate = DateTime.Now,
                    FromUserId = messageObject.fromuserId,
                    IPAddress = messageObject.IPAddress,
                    IsMessageRead = false,
                    IsReported = false,
                    IsSpam = false,
                    MessageDetail = messageObject.message,
                    ToUserId = messageObject.touserId
                });
                if (!_entities.TBL_MESSAGELABELS.Any(x => x.LabelName == "Follow-up" && x.UserId == messageObject.fromuserId))
                {
                    _entities.TBL_MESSAGELABELS.Add(new TBL_MESSAGELABELS
                    {
                        CanDelete = false,
                        CreatedBy = messageObject.fromuserId,
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        LabelName = "Follow-up",
                        ModifiedBy = messageObject.fromuserId,
                        ModifiedDate = DateTime.Now,
                        UserId = messageObject.fromuserId
                    });
                }
                if (!_entities.TBL_MESSAGELABELS.Any(x => x.LabelName == "Nudge" && x.UserId == messageObject.fromuserId))
                {
                    _entities.TBL_MESSAGELABELS.Add(new TBL_MESSAGELABELS
                    {
                        CanDelete = false,
                        CreatedBy = messageObject.fromuserId,
                        CreatedDate = DateTime.Now,
                        IsDelete = false,
                        LabelName = "Nudge",
                        ModifiedBy = messageObject.fromuserId,
                        ModifiedDate = DateTime.Now,
                        UserId = messageObject.fromuserId
                    });
                }
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ArchiveUser(ArchiveUserRequest archiveUserRequest)
        {
            try
            {
                var starResponse = _entities.TBL_MESSAGEARCHIVE.FirstOrDefault(x => x.FromUserId == archiveUserRequest.FromuserId && x.ToUserId == archiveUserRequest.TouserId);
                if (starResponse != null)
                {
                    starResponse.IsArchive = !archiveUserRequest.IsArchive;
                }
                else
                {
                    _entities.TBL_MESSAGEARCHIVE.Add(new TBL_MESSAGEARCHIVE()
                    {
                        FromUserId = archiveUserRequest.FromuserId,
                        ToUserId = archiveUserRequest.TouserId,
                        IsArchive = !archiveUserRequest.IsArchive,
                        CreatedDate = DateTime.Now,
                        CreatedBy = archiveUserRequest.FromuserId
                    });
                }
                _entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool BlockMessage(int messageId, bool isBlock)
        {
            try
            {
                var messageList = _entities.TBL_MESSAGEHISTORY.Where(x => x.MessageHistoryId == messageId).FirstOrDefault();

                if (messageList != null)
                {
                    messageList.IsSpam = isBlock;
                    _entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
    }
}