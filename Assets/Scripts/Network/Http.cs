using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;

public class Http
{
    public static string Get(string host, string path, string cookie)
    {
        string server = host;
        TcpClient client = new TcpClient(server, 443);

        var response = "";

        using (SslStream sslStream = new SslStream(client.GetStream(), false,
                   ValidateServerCertificate, null))
        {
            sslStream.AuthenticateAsClient(server);

            var requestHeaders =
                "GET " + path + " HTTP/1.1\r\n" +
                "Host: " + host + "\r\n" +
                "Cookie: " + cookie + "\r\n" +
                "TE: trailers\r\n\r\n";

            sslStream.Write(Encoding.ASCII.GetBytes(requestHeaders));

            var (status, responseHeaders) = ReadResponseStatusAndHeaders(sslStream);


            var contentLength = int.Parse(responseHeaders["Content-Length"]);

            for (int i = 0; i < contentLength; i++)
            {
                response += (char)sslStream.ReadByte();
            }
        }

        return response;
    }

    
    public static (int, Dictionary<string, string>, string) Post(string host, string path, string cookie)
    {
        string server = host;
        TcpClient client = new TcpClient(server, 443);

        var response = "";

        using (SslStream sslStream = new SslStream(client.GetStream(), false,
                   ValidateServerCertificate, null))
        {
            sslStream.AuthenticateAsClient(server);

            var requestHeaders =
                "POST " + path + " HTTP/1.1\r\n" +
                "Host: " + host + "\r\n" +
                "Cookie: " + cookie + "\r\n" +
                "TE: trailers\r\n\r\n";

            sslStream.Write(Encoding.ASCII.GetBytes(requestHeaders));

            var (status, responseHeaders) = ReadResponseStatusAndHeaders(sslStream);


            var contentLength = int.Parse(responseHeaders["Content-Length"]);

            for (int i = 0; i < contentLength; i++)
            {
                response += (char)sslStream.ReadByte();
            }
            
            return (status, responseHeaders, response);
        }
    }


    public static (int, Dictionary<string, string>) PostMultipart(string host, string path, string cookie,
        Dictionary<string, string> body)
    {
        string server = host;
        TcpClient client = new TcpClient(server, 443);

        using (SslStream sslStream = new SslStream(client.GetStream(), false,
                   ValidateServerCertificate, null))
        {
            sslStream.AuthenticateAsClient(server);

            var loginBody = "";

            foreach (var (name, value) in body)
            {
                loginBody += "-----------------------------13544320162248662364810109147\r\n" +
                             "Content-Disposition: form-data; name=\"" + name + "\"\r\n" +
                             "\r\n" +
                             value + "\r\n";
            }

            loginBody += "-----------------------------13544320162248662364810109147--\r\n";

            var loginHeaders =
                "POST " + path + " HTTP/1.1\r\n" +
                "Host: " + host + "\r\n" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:107.0) Gecko/20100101 Firefox/107.0\r\n" +
                "Accept: */*\r\n" +
                "Accept-Language: en-US,en;q=0.5\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "X-Requested-With: XMLHttpRequest\r\n" +
                "Cookie: " + cookie + "\r\n" +
                "Content-Type: multipart/form-data; boundary=---------------------------13544320162248662364810109147\r\n" +
                "Content-Length: " + loginBody.Length + "\r\n" +
                "Origin: https://" + host + "\r\n" +
                "Connection: keep-alive\r\n" +
                "Sec-Fetch-Dest: empty\r\n" +
                "Sec-Fetch-Mode: cors\r\n" +
                "Sec-Fetch-Site: same-origin\r\n" +
                "Pragma: no-cache\r\n" +
                "Cache-Control: no-cache\r\n" +
                "TE: trailers\r\n\r\n";

            var requestHeadersBytes = Encoding.ASCII.GetBytes(loginHeaders);
            var requestBodyBytes = Encoding.ASCII.GetBytes(loginBody);


            Debug.Log(requestBodyBytes.Length);

            // Debug.Log(string.Join(" ", requestBytes));


            sslStream.Write(requestHeadersBytes);
            sslStream.Write(requestBodyBytes);
            
            var response = ReadResponseStatusAndHeaders(sslStream);
            client.Close();
            return response;
        }
    }


    public static (int, string) PostMultipartAndGetCookie(string host, string path, Dictionary<string, string> body)
    {
        string server = host;
        TcpClient client = new TcpClient(server, 443);

        var cookie = "";

        var status = 0;

        using (SslStream sslStream = new SslStream(client.GetStream(), false,
                   ValidateServerCertificate, null))
        {
            sslStream.AuthenticateAsClient(server);

            var loginBody = "";

            foreach (var (name, value) in body)
            {
                loginBody += "-----------------------------13544320162248662364810109147\r\n" +
                             "Content-Disposition: form-data; name=\"" + name + "\"\r\n" +
                             "\r\n" +
                             value + "\r\n";
            }

            loginBody += "-----------------------------13544320162248662364810109147--\r\n";

            var loginHeaders =
                "POST " + path + " HTTP/1.1\r\n" +
                "Host: " + host + "\r\n" +
                "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:107.0) Gecko/20100101 Firefox/107.0\r\n" +
                "Accept: */*\r\n" +
                "Accept-Language: en-US,en;q=0.5\r\n" +
                "Accept-Encoding: gzip, deflate, br\r\n" +
                "X-Requested-With: XMLHttpRequest\r\n" +
                "Content-Type: multipart/form-data; boundary=---------------------------13544320162248662364810109147\r\n" +
                "Content-Length: " + loginBody.Length + "\r\n" +
                "Origin: https://" + host + "\r\n" +
                "Connection: keep-alive\r\n" +
                "Sec-Fetch-Dest: empty\r\n" +
                "Sec-Fetch-Mode: cors\r\n" +
                "Sec-Fetch-Site: same-origin\r\n" +
                "Pragma: no-cache\r\n" +
                "Cache-Control: no-cache\r\n" +
                "TE: trailers\r\n\r\n";

            var requestHeadersBytes = Encoding.ASCII.GetBytes(loginHeaders);
            var requestBodyBytes = Encoding.ASCII.GetBytes(loginBody);


            Debug.Log(requestBodyBytes.Length);

            // Debug.Log(string.Join(" ", requestBytes));


            sslStream.Write(requestHeadersBytes);
            sslStream.Write(requestBodyBytes);


            var (responseStatus, responseHeaders) = ReadResponseStatusAndHeaders(sslStream);
            status = responseStatus;

            if (status != 200)
            {
                return (status, "");
            }

            var cookieValue = responseHeaders["Set-Cookie"];

            cookie = cookieValue.Split("; Max-Age")[0];
        }

        client.Close();
        return (status, cookie);
    }

    public static (int, Dictionary<string, string>) ReadResponseStatusAndHeaders(SslStream sslStream)
    {
        List<int> response = new List<int>();

        for (;;)
        {
            response.Add(sslStream.ReadByte());
            if (endsWith13101310(response))
            {
                break;
            }
        }

        var responseString = "";

        for (var i = 0; i < response.Count; i++)
        {
            responseString += (char)response[i];
        }
        

        var lines = responseString.Split("\r\n").Where(line => line != "").ToArray();

        var headerLines = lines.Skip(1);

        var headers = new Dictionary<string, string>();

        foreach (var header in headerLines)
        {
            var nameAndValue = header.Split(": ", 2);

            headers[nameAndValue[0]] = nameAndValue[1];
        }

        return (int.Parse(lines[0].Split(" ")[1]), headers);
    }


    private static bool endsWith13101310(List<int> bytes)
    {
        if (bytes.Count > 0 && bytes[^1] == -1) return true;
        return bytes.Count >= 4 &&
               bytes[^4] == 13 &&
               bytes[^3] == 10 &&
               bytes[^2] == 13 &&
               bytes[^1] == 10;
    }

    public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}