﻿using Starksoft.Aspen.Proxy;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TLSharp.Core.MTProto.Crypto;

namespace TLSharp.Core.Network
{
    public delegate TcpClient TcpClientConnectionHandler(string address, int port);

    public class TcpTransport : IDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;
        private int sendCounter = 0;

        public TcpTransport(string address, int port, TcpClientConnectionHandler handler = null)
        {
            if (handler == null)
            {
                var ipAddress = IPAddress.Parse(address);
                var endpoint = new IPEndPoint(ipAddress, port);
                Socks5ProxyClient socks5ProxyClient = new Socks5ProxyClient("127.0.0.1",1080);
                //HttpProxyClient httpProxyClient = new HttpProxyClient("127.0.0.1",8888);
                //tcpClient = new TcpClient(ipAddress.AddressFamily);
                try {
                    //tcpClient.Connect (endpoint);
                    tcpClient= socks5ProxyClient.CreateConnection(address, port);
                    //tcpClient = httpProxyClient.CreateConnection(address,port);
                } catch (Exception ex) {
                    throw new Exception ($"Problem when trying to connect to {endpoint}; either there's no internet connection or the IP address version is not compatible (if the latter, consider using DataCenterIPVersion enum)",
                                         ex);
                }
            }
            else
                tcpClient = handler(address, port);

            if (tcpClient.Connected)
            {
                stream = tcpClient.GetStream();
            }
        }

        public async Task Send(byte[] packet, CancellationToken token = default(CancellationToken))
        {
            if (!tcpClient.Connected)
                throw new InvalidOperationException("Client not connected to server.");

            var tcpMessage = new TcpMessage(sendCounter, packet);

            await stream.WriteAsync(tcpMessage.Encode(), 0, tcpMessage.Encode().Length, token).ConfigureAwait(false);
            sendCounter++;
        }

        public async Task<TcpMessage> Receive(CancellationToken token = default(CancellationToken))
        {
            var packetLengthBytes = new byte[4];
            if (await stream.ReadAsync(packetLengthBytes, 0, 4, token).ConfigureAwait(false) != 4)
                throw new InvalidOperationException("Couldn't read the packet length");
            int packetLength = BitConverter.ToInt32(packetLengthBytes, 0);

            var seqBytes = new byte[4];
            if (await stream.ReadAsync(seqBytes, 0, 4, token).ConfigureAwait(false) != 4)
                throw new InvalidOperationException("Couldn't read the sequence");
            int seq = BitConverter.ToInt32(seqBytes, 0);

            int readBytes = 0;
            var body = new byte[packetLength - 12];
            int neededToRead = packetLength - 12;

            do
            {
                var bodyByte = new byte[packetLength - 12];
                var availableBytes = await stream.ReadAsync(bodyByte, 0, neededToRead, token).ConfigureAwait(false);
                neededToRead -= availableBytes;
                Buffer.BlockCopy(bodyByte, 0, body, readBytes, availableBytes);
                readBytes += availableBytes;
            }
            while (readBytes != packetLength - 12);

            var crcBytes = new byte[4];
            if (await stream.ReadAsync(crcBytes, 0, 4, token).ConfigureAwait(false) != 4)
                throw new InvalidOperationException("Couldn't read the crc");

            byte[] rv = new byte[packetLengthBytes.Length + seqBytes.Length + body.Length];

            Buffer.BlockCopy(packetLengthBytes, 0, rv, 0, packetLengthBytes.Length);
            Buffer.BlockCopy(seqBytes, 0, rv, packetLengthBytes.Length, seqBytes.Length);
            Buffer.BlockCopy(body, 0, rv, packetLengthBytes.Length + seqBytes.Length, body.Length);
            var crc32 = new Crc32();
            var computedChecksum = crc32.ComputeHash(rv).Reverse();

            if (!crcBytes.SequenceEqual(computedChecksum))
            {
                throw new InvalidOperationException("invalid checksum! skip");
            }

            return new TcpMessage(seq, body);
        }

        public bool IsConnected
        {
            get
            {
                return this.tcpClient.Connected;
            }
        }


        public void Dispose()
        {
            if (tcpClient.Connected)
            {
                stream.Close();
                tcpClient.Close();
            }
        }
    }
}
