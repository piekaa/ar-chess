public class WebSocketTextMessage : WebSocketMessage
{
    public readonly string Text;

    public WebSocketTextMessage(string text)
    {
        Text = text;
    }
}
