
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Channels;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TeleSharp.TL.Users;
using TLSharp.Core;
using TLSharp.Core.Utils;

namespace TestTlWin
{
    class Program
    {
        static void Main(string[] args)
        {
            TestTl();
            Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
        static async void TestTl()
        {
            //Socks5ProxyClient socks5ProxyClient = new Socks5ProxyClient("127.0.0.1", 1080);
            //var tcpClient=await socks5ProxyClient.CreateConnection("149.154.167.50", 443);
            //TelegramClient client = new TelegramClient(1807643, "d02110ac671efefee34d368491809de9");
            //
            var phone = "+8617136679087";
            //var phone = "+8615111385412";
            TelegramClient client = new TelegramClient(17349, "344583e45741c457fe1862106095a5eb", null, phone);
            await client.ConnectAsync();

            //await client.SendPingAsync();

            TLUser user = null;
            #region 登录
            if (client.Session.TLUser == null)
            {
                var tLSentCode = await client.SendCodeRequestAsync(phone);
                var code = "41927";
                if (await client.IsPhoneRegisteredAsync(phone))
                {
                    try
                    {
                        user = await client.MakeAuthAsync(phone, tLSentCode.PhoneCodeHash, code);
                    }
                    catch (TLSharp.Core.Exceptions.CloudPasswordNeededException)
                    {
                        var tLPassword = await client.GetPasswordSetting();
                        user = await client.MakeAuthWithPasswordAsync(tLPassword,"123456");
                    }

                }
                else
                {
                    user = await client.SignUpAsync(phone, tLSentCode.PhoneCodeHash, code, "test", "test");
                }
            }
            #endregion
            #region 获得会话列表
            //var userDialogs = await client.GetUserDialogsAsync() as TLDialogs;

            #endregion
            #region 获得消息
            //for (int i = 0; i < userDialogs.Dialogs.Count; i++)
            //{
            //    var dialogs = userDialogs.Dialogs[i];
            //    if (dialogs.UnreadCount > 0)
            //    {

            //        if (dialogs.Peer is TLPeerChannel dialChannel)
            //        {
            //            var getMessageChannel = userDialogs.Chats.FirstOrDefault(c => (c is TLChannel tlc) && tlc.Id == dialChannel.ChannelId) as TLChannel;
            //            var peer = new TLInputPeerChannel
            //            {
            //                ChannelId = getMessageChannel.Id,
            //                AccessHash = getMessageChannel.AccessHash ?? 0
            //            };
            //            var getMessagesMessages = await client.GetHistoryAsync(peer, limit: dialogs.UnreadCount, maxId: dialogs.TopMessage + 1);//
            //            var success = await client.SendRequestAsync<bool>(new TeleSharp.TL.Channels.TLRequestReadHistory
            //            {
            //                Channel = new TLInputChannel
            //                {
            //                    ChannelId = getMessageChannel.Id,
            //                    AccessHash = getMessageChannel.AccessHash ?? 0
            //                },
            //                MaxId = dialogs.TopMessage
            //            });
            //        }
            //        else
            //        {
            //            TLAbsInputPeer peer = null;
            //            if (dialogs.Peer is TLPeerChat dialChat)
            //            {
            //                var getMessageUser = userDialogs.Chats.FirstOrDefault(u => (u is TLChat tlu) && tlu.Id == dialChat.ChatId) as TLChat;
            //                peer = new TLInputPeerChat { ChatId = getMessageUser.Id };
            //            }
            //            else if (dialogs.Peer is TLPeerUser dialUser)
            //            {
            //                var getMessageUser = userDialogs.Users.FirstOrDefault(u => (u is TLUser tlu) && tlu.Id == dialUser.UserId) as TLUser;
            //                peer = new TLInputPeerUser { UserId = getMessageUser.Id, AccessHash = getMessageUser.AccessHash ?? 0 };
            //            }
            //            var getMessagesMessages = await client.GetHistoryAsync(peer, limit: dialogs.UnreadCount, maxId: dialogs.TopMessage + 1);
            //            var receivedMessagesList = await client.SendRequestAsync<TLAffectedMessages>(new TeleSharp.TL.Messages.TLRequestReadHistory
            //            {
            //                Peer = peer,
            //                MaxId = dialogs.TopMessage
            //            });
            //        }
            //    }
            //}
            //var message = userDialogs.Messages[0] as TLMessage;
            //var channel = userDialogs.Chats[0] as TLChannel;
            //var getPeerDialogs = await client.SendRequestAsync<TLPeerDialogs>(new TLRequestGetPeerDialogs
            //{
            //    Peers = new TLVector<TLAbsInputPeer>(new[] {new TLInputPeerChannel{
            //  ChannelId=channel.Id,
            //   AccessHash=channel.AccessHash??0
            // } })
            //});

            #endregion
            #region 根据名称寻找对应用户
            var resolvedPeer = await client.SendRequestAsync<TLResolvedPeer>(new TLRequestResolveUsername()
            {
                Username = "fanxing001"//"rocky_yu"//
            });
            var addUser = resolvedPeer.Users[0] as TLUser;
            #endregion
            #region 获取当前所有群组,频道
            //var chats = await client.SendRequestAsync<TLAbsChats>(new TLRequestGetAllChats { ExceptIds = new TLVector<int>() });
            //var chatList = chats as TLChats;
            #endregion
            #region 群组加人
            //var chat1 = chatList.Chats[1] as TLChat;
            //var addChatUserUpdates = await client.SendRequestAsync<TLAbsUpdates>(new TLRequestAddChatUser
            //{
            //    ChatId = chat1.Id,
            //    UserId = new TLInputUser
            //    {
            //        UserId = addUser.Id,
            //        AccessHash = addUser.AccessHash ?? 0
            //    },
            //    FwdLimit = 5
            //});
            #endregion
            #region 群组踢人
            //var chat1 = chatList.Chats[1] as TLChat;
            //var deleteChatUserUpdates = await client.SendRequestAsync<TLAbsUpdates>(new TLRequestDeleteChatUser
            //{
            //    ChatId = chat1.Id,
            //    UserId = new TLInputUser
            //    {
            //        UserId = addUser.Id,
            //        AccessHash = addUser.AccessHash ?? 0
            //    },
            //});

            #endregion
            #region 获取群组成员
            //TLAbsChat tblAbsChat = chatList.Chats[3];
            //if (tblAbsChat is TLChat chat)
            //{
            //    var tLChatFull = await client.SendRequestAsync<TeleSharp.TL.Messages.TLChatFull>(new TLRequestGetFullChat { ChatId = chat.Id });
            //}
            //else if (tblAbsChat is TLChannel channel)
            //{
            //    var tLChatFull = await client.SendRequestAsync<TeleSharp.TL.Messages.TLChatFull>(new TLRequestGetFullChannel
            //    {
            //        Channel = new TLInputChannel
            //        {
            //            ChannelId = channel.Id,
            //            AccessHash = channel.AccessHash ?? 0
            //        }
            //    });
            //    var channelParticipant = await client.SendRequestAsync<TeleSharp.TL.Channels.TLChannelParticipants>(new TLRequestGetParticipants
            //    {
            //        Channel = new TLInputChannel
            //        {
            //            ChannelId = channel.Id,
            //            AccessHash = channel.AccessHash ?? 0
            //        },
            //        Limit = 100,
            //        Filter = new TLChannelParticipantsRecent
            //        {

            //        },
            //        Filter = new TLChannelParticipantsSearch
            //        {

            //        }
            //        Filter = new TLChannelParticipantsKicked
            //        {

            //        }
            //        Filter = new TLChannelParticipantsBanned
            //        {

            //        }
            //    });
            //}
            #endregion
            #region 更新群组名称,关于
            //if (chats is TLChats)
            //{
            //var chatList = chats as TLChats;
            //var chat = chatList.Chats[0] as TLChat;
            //var user2 = await client.SendRequestAsync<TLAbsUpdates>(new TLRequestEditChatTitle
            //{
            //    ChatId = chat.Id,
            //    Title = "UpdateSuccess"
            //});
            //var success = await client.SendRequestAsync<bool>(new TLRequestEditChatAbout
            //{
            //    Peer = new TLInputPeerChat
            //    {
            //        ChatId = chat.Id,
            //    },
            //    About = "TEST ABOUT"
            //});
            //}

            #endregion
            #region 创建群组
            //var tLAbsUpdates = await client.SendRequestAsync<TLAbsUpdates>(new TLRequestCreateChat
            //{
            //    Title = "Test12323123",
            //    Users = new TLVector<TLAbsInputUser>(new[] { new TLInputUser {
            //     UserId=addUser.Id,
            //      AccessHash=addUser.AccessHash??0
            //    } })
            //});
            #endregion
            #region  创建管道，超群组
            //var tLAbsUpdates = await client.SendRequestAsync<TLAbsUpdates>(new TLRequestCreateChannel
            //{
            //    Title = "Test123",
            //    About = "This is Test1231241",
            //    Megagroup = true,
            //});
            #endregion
            #region 获得聊天记录
            //var messagesList=await client.GetHistoryAsync(new TLInputPeerUser
            //{
            //    UserId = addUser.Id,
            //    AccessHash = addUser.AccessHash??0
            //});
            #endregion
            #region 发送表情
            //var emojiKeywordsDifference=await client.SendRequestAsync<TLEmojiKeywordsDifference>(new TLRequestGetEmojiKeywords
            //{
            //    LangCode = "en"
            //});
            #endregion
            #region 发送图片
            //var fileResult = await client.UploadFile("image.jpg", new StreamReader("image.jpg"));
            //var updates2 = await client.SendUploadedPhoto(new TLInputPeerUser() { UserId = addUser.Id }, fileResult, "kitty");
            #endregion
            #region 上传文档
            //await client.SendUploadedDocument(
            //               new TLInputPeerUser() { UserId = addUser.Id },
            //               fileResult,
            //               "some zips", //caption
            //               "application/zip", //mime-type
            //               new TLVector<TLAbsDocumentAttribute>()); //document attributes, such as file name 
            #endregion
            #region 获取用户所有信息
            var userFull = await client.SendRequestAsync<TLUserFull>(new TLRequestGetFullUser()
            {
                Id = new TLInputUser { UserId = addUser.Id, AccessHash = addUser.AccessHash ?? 0 },
            });
            #endregion
            #region 添加修改好友
            //var updates = await client.SendRequestAsync<TLUpdates>(new TLRequestAddContact()
            //{
            //    Id = new TLInputUser { UserId = addUser.Id, AccessHash = addUser.AccessHash ?? 0 },
            //    FirstName = addUser.FirstName + "Test",
            //    LastName = addUser.LastName,
            //    Phone = addUser.Phone
            //});
            #endregion
            #region 获得所有联系人
            //var result = await client.GetContactsAsync();
            #endregion
            #region 发送文本消息
            //var user2 = await client.SendMessageAsync(new TLInputPeerUser() { UserId = addUser.Id }, "🌎"); 
            #endregion
            Console.Read();
        }
    }
}
