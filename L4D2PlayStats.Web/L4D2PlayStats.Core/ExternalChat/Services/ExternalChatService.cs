using System.Collections.ObjectModel;
using L4D2PlayStats.Core.ExternalChat.Commands;
using L4D2PlayStats.Core.ExternalChat.Models;
using L4D2PlayStats.Core.ExternalChat.Results;

namespace L4D2PlayStats.Core.ExternalChat.Services;

public class ExternalChatService : IExternalChatService
{
    private static readonly List<Message> Messages = [];
    private static readonly Lock Lock = new();

    public ReadOnlyCollection<Message> GetMessages()
    {
        lock (Lock)
        {
            RemoveOldMessages();

            return Messages.AsReadOnly();
        }
    }

    public SendMessageResult SendMessage(User? user, MessageCommand? command)
    {
        if (user == null)
            return SendMessageResult.FailureResult("User cannot be null.");

        if (command == null)
            return SendMessageResult.FailureResult("Command cannot be null.");

        if (string.IsNullOrEmpty(user.SteamId))
            return SendMessageResult.FailureResult("User SteamId cannot be null or empty.");

        if (string.IsNullOrEmpty(user.Name))
            return SendMessageResult.FailureResult("User Name cannot be null or empty.");

        if (string.IsNullOrEmpty(command.Message))
            return SendMessageResult.FailureResult("Message cannot be null or empty.");

        if (command.Message.Length > 200)
            return SendMessageResult.FailureResult("Message cannot be longer than 200 characters.");

        lock (Lock)
        {
            RemoveOldMessages();

            var lastMessage = Messages.LastOrDefault();
            if (lastMessage is { Age.TotalSeconds: < 20 })
                return SendMessageResult.FailureResult("Max message rate exceeded. Please wait.");

            var lastUserMessage = Messages.LastOrDefault(m => m.SteamId == user.SteamId);
            if (lastUserMessage is { Age.TotalMinutes: < 5 })
                return SendMessageResult.FailureResult("Max message rate exceeded. Please wait.");

            var message = new Message(user, command);

            Messages.Add(message);
        }

        return SendMessageResult.SuccessResult();
    }

    private static void RemoveOldMessages()
    {
        Messages.RemoveAll(m => m.Age.TotalMinutes >= 15);
    }
}