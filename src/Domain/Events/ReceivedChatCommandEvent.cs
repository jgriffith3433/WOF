namespace WOF.Domain.Events;

public class ReceivedChatCommandEvent : BaseEvent
{
    public ReceivedChatCommandEvent(ChatCommand chatCommand)
    {
        ChatCommand = chatCommand;
    }

    public ChatCommand ChatCommand { get; }
}
