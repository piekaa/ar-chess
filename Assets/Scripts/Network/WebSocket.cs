using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebSocket
{
    private SslStream sslStream;
    private ConcurrentQueue<WebSocketMessage> incomingMessages = new();
    private Thread readThread;

    private bool connected;
    
    private Queue<string> messageQueue = new();

    public WebSocket(string host, string origin, string path, string cookie)
    {
        var client = new TcpClient(host, 443);

        sslStream = new SslStream(client.GetStream(), false, Http.ValidateServerCertificate, null);
        sslStream.AuthenticateAsClient(host);

        var webSocketUpgradeRequest =
            "GET " + path + " HTTP/1.1\r\n" +
            "Host: " + host + "\r\n" +
            "Connection: Upgrade\r\n" +
            "Cookie: " + cookie + "\r\n" +
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36\r\n" +
            "Upgrade: websocket\r\n" +
            "Origin: https://" + origin + "\r\n" +
            "Sec-WebSocket-Version: 13\r\n" +
            "Sec-WebSocket-Key: LkYUL9m088X1bROLkTh9hA==\r\n" +
            "Sec-WebSocket-Extensions: permessage-deflate; client_max_window_bits\r\n\r\n";


        var webSocketUpgradeRequestBytes = Encoding.ASCII.GetBytes(webSocketUpgradeRequest);
        sslStream.Write(webSocketUpgradeRequestBytes);


        readThread = new Thread(() =>
        {
            Http.ReadResponseLineAndHeaders(sslStream);
            connected = true;
            
            foreach (var message in messageQueue)
            {
                Send(message);
            }

            for (;;)
            {
                try
                {
                    var opcode = sslStream.ReadByte() & 0b1111;
                    if (opcode == 1)
                    {
                        incomingMessages.Enqueue(ReadTextMessage());
                    }

                    else if (opcode == 9)
                    {
                        var ping = ReadPingMessage();
                        Send(ping.Payload, 10);
                    }
                    else
                    {
                        throw new Exception("Opcode different than 1 and 9 -> " + opcode);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    break;
                }
            }
        });
        readThread.Start();
    }

    public WebSocketMessage DequeueMessageOrNull()
    {
        WebSocketMessage message;
        var messageAvailable = incomingMessages.TryDequeue(out message);
        if (messageAvailable)
        {
            return message;
        }

        return null;
    }


    private WebSocketMessage ReadTextMessage()
    {
        return new WebSocketTextMessage(ByteArrayToString(ReadPayload()));
    }

    private string ByteArrayToString(byte[] bytes)
    {
        var responseString = "";

        for (var i = 0; i < bytes.Length; i++)
        {
            responseString += (char)bytes[i];
        }

        return responseString;
    }

    private WebScoketPingMessage ReadPingMessage()
    {
        return new WebScoketPingMessage(ReadPayload());
    }

    private byte[] ReadPayload()
    {
        var (payloadLength, mask) = ReadPayloadLengthAndMask();
        byte[] payload = new byte[payloadLength];

        var maskIndex = 0;

        for (int i = 0; i < payloadLength; i++)
        {
            payload[i] = (byte)(sslStream.ReadByte() ^ mask[maskIndex]);
            maskIndex++;
            maskIndex %= 4;
        }

        return payload;
    }

    private (int, byte[]) ReadPayloadLengthAndMask()
    {
        var payloadLengthFirstByte = sslStream.ReadByte();

        bool withMask = (payloadLengthFirstByte & 0b10000000) != 0;

        var payloadLength = 0;

        if (payloadLengthFirstByte < 126)
        {
            payloadLength = payloadLengthFirstByte;
        }
        else
        {
            if (payloadLengthFirstByte == 127)
            {
                throw new Exception("Payload length is too large");
            }

            payloadLength = sslStream.ReadByte() << 8 | sslStream.ReadByte();
        }

        if (withMask)
        {
            var mask = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                mask[i] = (byte)sslStream.ReadByte();
            }

            return (payloadLength, mask);
        }

        return (payloadLength, new byte[] { 0, 0, 0, 0 });
    }


    

    public void Send(string message)
    {
        if (!connected)
        {
            messageQueue.Enqueue(message);
            return;
        }

        sslStream.Write(new byte[] { 0b10000001 });

        var bytes = Encoding.ASCII.GetBytes(message);

        sslStream.Write(new[] { (byte)(0b10000000 | bytes.Length) });

        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });

        for (int i = 0; i < bytes.Length; i++)
        {
            sslStream.Write(new[] { (byte)(bytes[i] ^ 0b11111111) });
        }
    }

    private void Send(byte[] message, byte opcode)
    {
        sslStream.Write(new[] { (byte)(0b10000000 | opcode) });


        sslStream.Write(new[] { (byte)(0b10000000 | message.Length) });

        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });
        sslStream.Write(new byte[] { 0b11111111 });

        for (int i = 0; i < message.Length; i++)
        {
            sslStream.Write(new[] { (byte)(message[i] ^ 0b11111111) });
        }
    }

    public void Disconnect()
    {
        connected = false;
        sslStream.Close();
    }
}