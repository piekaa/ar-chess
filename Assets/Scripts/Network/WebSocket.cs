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


        Debug.Log(webSocketUpgradeRequest);
        
        var webSocketUpgradeRequestBytes = Encoding.ASCII.GetBytes(webSocketUpgradeRequest);

        sslStream.Write(webSocketUpgradeRequestBytes);

        Debug.Log(Http.ReadResponseHeaders(sslStream));

        new Thread(() =>
        {
            for (;;)
            {
                var opcode = sslStream.ReadByte() & 0b1111;

                incomingMessages.Enqueue(opcode switch
                {
                    1 => ReadTextMessage(),
                    9 => ReadPingMessage(),
                    _ => throw new Exception("Opcode different than 1 and 9 -> " + opcode)
                });
            }
        }).Start();
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

    private WebSocketMessage ReadPingMessage()
    {
        return new WebScoketPingMessage(ReadPayload());
    }

    private byte[] ReadPayload()
    {
        var payloadLength = ReadPayloadLength();
        byte[] payload = new byte[payloadLength];

        for (int i = 0; i < payloadLength; i++)
        {
            payload[i] = (byte)sslStream.ReadByte();
        }

        return payload;
    }

    private int ReadPayloadLength()
    {
        var payloadLengthFirstByte = sslStream.ReadByte();

        if ((payloadLengthFirstByte & 0b10000000) != 0)
        {
            throw new Exception("Mask is set");
        }

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

        return payloadLength;
    }


    public void Send(string message)
    {
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

    public void Send(byte[] message, byte opcode)
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
}