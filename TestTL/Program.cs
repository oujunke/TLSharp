using System;
using TLSharp.Core;

namespace TestTL
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
            TelegramClient telegramClient = new TelegramClient(1974560, "9559517a588cf1912bd58df8511d0625");
            await telegramClient.ConnectAsync();
            await telegramClient.SendPingAsync();
        }
    }
}
