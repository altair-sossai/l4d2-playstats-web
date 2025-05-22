using System.Collections.ObjectModel;
using L4D2PlayStats.Core.ExternalChat.Commands;
using L4D2PlayStats.Core.ExternalChat.Models;
using L4D2PlayStats.Core.ExternalChat.Results;

namespace L4D2PlayStats.Core.ExternalChat.Services;

public interface IExternalChatService
{
    ReadOnlyCollection<Message> GetMessages();
    SendMessageResult SendMessage(User? user, MessageCommand? command);
}