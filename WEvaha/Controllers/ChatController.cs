using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WE.BusinessEntities;
using WE.BusinessServices;
using WE.BusinessServices.Interface;
using WE.DataAccess;
using WE.Utilities;

namespace WEvaha.Controllers
{
    public class ChatController : Controller
    {
        public readonly IChatService _chatService;
        ChatService chatService = new ChatService();
        private readonly IDashBoardService _dashBoardservice;
        DashBoardService dashboardService = new DashBoardService();
        Models.DashboardProfileModel DBPM = new Models.DashboardProfileModel();
        public ChatController()
        {
            _chatService = chatService;
            _dashBoardservice = dashboardService;
        }

        // GET: Chat
        public ActionResult Index(int? id)
        {
            if (Session["UserId"] != null)
            {
                int ProfileId = Convert.ToInt32(Session["UserId"]);                
                DBPM.MessageCountList = _chatService.GetChatUsersMsgCountResult(ProfileId);
                if (Session["UserUnReadMessages"] != null)
                    Session["UserUnReadMessages"] = DBPM.MessageCountList.Take(10).ToList();
             
                return View(DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }

        [HttpPost]
        public ActionResult GetMessageList(MessageListModel messageListModel)
        {
            var getChatList = _chatService.GetMessageList(messageListModel);
            return Json(new { data = getChatList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChatUploadFiles()
        {
            var messageObject = new MessageObject();
            if (Request.Files.Count > 0)
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                string fname;
                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }
                string fileUniqueId = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                // Get the complete folder path and store the file inside it.  
                string path = Path.Combine(Server.MapPath("~/Content/ChatFile/"), fileUniqueId);
                if (!Directory.Exists(Server.MapPath("~/Content/ChatFile/")))
                    Directory.CreateDirectory(path);
                file.SaveAs(path);

                path = "/Content/ChatFile/" + fileUniqueId;
                var toUserId = Convert.ToInt32(Request.Form["touserId"]);
                var fromUserId = Convert.ToInt32(Request.Form["fromuserId"]);
                var textMessage = string.IsNullOrEmpty(Request.Form["message"]) ? "Please find attached" : Request.Form["message"];
                messageObject = new MessageObject
                {
                    touserId = toUserId,
                    fromuserId = fromUserId,
                    message = textMessage,
                    fileName = fname,
                    filePath = path
                };

                using (WEvahaEntities _entities = new WEvahaEntities())
                {
                    var message = new TBL_MESSAGEHISTORY()
                    {
                        MessageDetail = textMessage,
                        FromUserId = fromUserId,
                        ToUserId = toUserId,
                        CreatedDate = DateTime.Now,
                        IPAddress = Request.UserHostAddress,
                        IsMessageRead = false,
                        IsReported = false,
                        IsSpam = false
                    };

                    _entities.TBL_MESSAGEHISTORY.Add(message);
                    _entities.SaveChanges();
                    _entities.TBL_MESSAGEATTACHMENTS.Add(new TBL_MESSAGEATTACHMENTS
                    {
                        MessageHistoryId = message.MessageHistoryId,
                        FileName = fname,
                        FileUniqueId = fileUniqueId,
                        CreatedDate = DateTime.Now
                    });
                    _entities.SaveChanges();
                }

                return Json(new { data = messageObject, isSuccess = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "File Not Found.", isSuccess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult StarUser(StarUserRequest starUserRequest)
        {
            bool starUserResponse = _chatService.StarUser(starUserRequest);
            return Json(new { data = starUserResponse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UnreadUserMessage(ActionMessageRequest unreadMessageRequest)
        {
            bool unreadMessage = _chatService.UnreadUserMessage(unreadMessageRequest);
            return Json(new { data = unreadMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUserConversation(ActionMessageRequest deleteMessageRequest)
        {
            var path = Server.MapPath("~/Content/ChatFile/");
            bool unreadMessage = _chatService.DeleteUserConversation(deleteMessageRequest, path);
            return Json(new { data = unreadMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportMessage(int messageId)
        {
            bool unreadMessage = _chatService.ReportMessage(messageId, out ReportUserModel reportUserModel);
            string MailContent = System.IO.File.ReadAllText(Server.MapPath(@"~/Content/AbuseMessageComplaint.html"));
            MailContent = MailContent.Replace("{{ProfileId}}", reportUserModel.ProfileId).Replace("{{UserName}}", reportUserModel.UserName).Replace("{{UserEmail}}", reportUserModel.UserEmail).Replace("{{ToUserProfileId}}", reportUserModel.ToUserProfileId).Replace("{{ReportMessage}}", reportUserModel.ReportMessage);
            string Mailsubject = "Abuse Message Complaint";
            Email sEmail = new Email();
            string FromEmailAddress = "registration@wevaha.com";
            int sResult = sEmail.SentEmail(FromEmailAddress, "admin@wevaha.com", Mailsubject, MailContent);
            return Json(new { data = unreadMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddLabel(AddLabelRequest addLabelRequest)
        {
            var labelId = _chatService.AddLabel(addLabelRequest);
            if (labelId == 0)
                return Json(new { data = 0 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = labelId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteLabel(long labelId)
        {
            var response = _chatService.DeleteLabel(labelId);
            return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LabelCheck(AddUserLabelRequest addUserLabelRequest)
        {
            var response = _chatService.LabelCheck(addUserLabelRequest);
            return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteQuickResponse(int resId)
        {
            var response = _chatService.DeleteQuickResponse(resId);
            return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddQuickResponse(AddQuickResponseRequest addQuickResponseRequest)
        {
            var response = _chatService.AddQuickResponse(addQuickResponseRequest);
            if (response == 0)
                return Json(new { data = 0 }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListOfUserQuickResponse(int userId)
        {
            var response = _chatService.ListOfUserQuickResponse(userId);
            return Json(new { data = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendMessage(MessageObject messageObject)
        {
            messageObject.IPAddress = Request.UserHostAddress;
            bool isSuccess = _chatService.SendMessage(messageObject);
            return Json(new { data = isSuccess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ArchiveUser(ArchiveUserRequest archiveUserRequest)
        {
            bool unreadMessage = _chatService.ArchiveUser(archiveUserRequest);
            return Json(new { data = unreadMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BlockMessage(int messageId, bool isBlock)
        {
            bool unreadMessage = _chatService.BlockMessage(messageId, isBlock);
            return Json(new { data = unreadMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}