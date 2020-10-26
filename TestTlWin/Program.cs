
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TLSharp.Core;

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
            TelegramClient client = new TelegramClient(1807643, "d02110ac671efefee34d368491809de9");
            //
            var phone = "+8617136679087";
            //var phone = "+8615111385412";
            //TelegramClient client = new TelegramClient(17349, "344583e45741c457fe1862106095a5eb",null,phone);
            await client.ConnectAsync();

            //await client.SendPingAsync();

            TLUser user;
            if (client.Session.TLUser == null)
            {
                var tLSentCode = await client.SendCodeRequestAsync(phone);
                var code = "24182";
                if (tLSentCode.PhoneRegistered)
                {
                    user = await client.MakeAuthAsync(phone, tLSentCode.PhoneCodeHash, code);
                }
                else
                {
                    user = await client.SignUpAsync(phone, tLSentCode.PhoneCodeHash, code, "test", "test");
                }
            }
            var result = await client.GetContactsAsync();
            var user2 = await client.SendMessageAsync(new TLInputPeerUser() { UserId = ((TLUser)result.Users[0]).Id }, "OUR_MESSAGE");
            Console.Read();
        }
    }
}
