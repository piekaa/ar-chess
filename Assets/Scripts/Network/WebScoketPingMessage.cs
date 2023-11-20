public class WebScoketPingMessage : WebSocketMessage
{
    public readonly byte[] Payload;

    public WebScoketPingMessage(byte[] payload)
    {
        Payload = payload;
    }
}
