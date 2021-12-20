using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WE.DataAccess;

namespace WEvaha.Hubs
{
    public class ChatHub : Hub
    {
        static ConcurrentDictionary<int, string> dic = new ConcurrentDictionary<int, string>();
        static ConcurrentDictionary<int, string> dicActive = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// When a user comes online for first time, or reloads page. 
        /// Add the user into dictionary and call relevant functions
        /// </summary>
        public void Notify(int profileId, string id)
        {
            if (dic.ContainsKey(profileId))
            {
                //do some stuff if client is already in dictionary
            }
            else
            {
                using (WEvahaEntities db = new WEvahaEntities())
                {
                    var user = db.TBL_USER_PROFILE.Where(x => x.ProfileId == profileId).FirstOrDefault();
                    if (user != null)
                    {
                        user.LastSeenDate = null;
                        db.SaveChanges();
                    }
                }

                dic.TryAdd(profileId, id);
                dicActive.TryAdd(profileId, null);
                foreach (KeyValuePair<int, string> entry in dic)
                {
                    Clients.Caller.online(entry.Key);
                }
                Clients.Others.enters(profileId);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var connId = Context.ConnectionId;
            var name = dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
            if (!string.IsNullOrWhiteSpace(name.Key.ToString()))
            {
                using (WEvahaEntities db = new WEvahaEntities())
                {
                    var user = db.TBL_USER_PROFILE.Where(x => x.ProfileId == name.Key).FirstOrDefault();
                    if (user != null)
                    {
                        user.LastSeenDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }

                dic.TryRemove(name.Key, out string s);
                dicActive.TryRemove(name.Key, out string s1);
                return Clients.All.disconnected(name.Key);
            }
            else
            {
                return base.OnDisconnected(stopCalled);
            }
        }

        public void Send(int touserId, int fromUserId, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            var read = false;
            if (dicActive.ContainsKey(touserId) && dicActive[touserId] == touserId.ToString())
            {
                read = true;
            }
            var senderUsername = string.Empty;
            var senderProfileURL = string.Empty;
            //Add in DB
            using (WEvahaEntities db = new WEvahaEntities())
            {
                db.TBL_MESSAGEHISTORY.Add(new TBL_MESSAGEHISTORY()
                {
                    MessageDetail = message,
                    FromUserId = fromUserId,
                    ToUserId = touserId,
                    CreatedDate = DateTime.Now,
                    IPAddress = Context.Request.Environment["server.RemoteIpAddress"].ToString(),
                    IsMessageRead = read,
                    IsReported = false,
                    IsSpam = false
                });

                db.SaveChanges();

                senderUsername = "Me";//db.USP_WE_GET_PROFILE_PERSONALDETAILS_BY_ID(fromUserId).FirstOrDefault()?.ProfileName;
                senderProfileURL = db.USP_WE_GET_PROFILE_PIC_BY_ID(fromUserId).FirstOrDefault()?.ProfilePicURL;
            }


            if (dic.ContainsKey(touserId))
            {
                Clients.Client(dic[touserId]).addNewMessageToPage(fromUserId, touserId, message, senderUsername, senderProfileURL, DateTime.Now.ToString("dd MMMM, yyyy mm:ss tt"), read);
            }
        }

        public void UnreadMessageRead(int touserId, int fromUserId)
        {
            using (WEvahaEntities db = new WEvahaEntities())
            {
                var unreadMessage = db.TBL_MESSAGEHISTORY.Where(x => x.FromUserId == touserId && x.ToUserId == fromUserId && !x.IsMessageRead).ToList();

                foreach (var item in unreadMessage)
                {
                    item.IsMessageRead = true;
                }
                db.SaveChanges();
            }
        }

        public void FileSendMessage(int touserId, int fromUserId, string message, string fileName, string filePath)
        {
            var senderUsername = string.Empty;
            var senderProfileURL = string.Empty;
            using (WEvahaEntities db = new WEvahaEntities())
            {
                senderUsername = "Me"; //db.USP_WE_GET_PROFILE_PERSONALDETAILS_BY_ID(fromUserId).FirstOrDefault()?.ProfileName;
                senderProfileURL = db.USP_WE_GET_PROFILE_PIC_BY_ID(fromUserId).FirstOrDefault()?.ProfilePicURL;
            }

            if (dic.ContainsKey(touserId))
            {
                Clients.Client(dic[touserId]).fileSendMessage(fromUserId, touserId, message, senderUsername, senderProfileURL, DateTime.Now.ToString("dd MMMM, yyyy mm:ss tt"), false, fileName, filePath);
            }
        }
    }
}