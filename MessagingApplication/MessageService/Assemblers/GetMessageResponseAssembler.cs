using MessageService.DTOs;
using MessageService.Exceptions;
using MessageService.Models;
using MessageService.Repositories;
using System;
using System.Collections;

namespace MessageService.Assemblers
{
    public class GetMessageResponseAssembler
    {
        public List<GetMessageResponse> AssembleMany(
            IEnumerable<Message> messages,
            IEnumerable<Chat> relatedChats,
            IEnumerable<User> relatedUsers,
            IEnumerable<Message> relatedMessages
            )
        {
            var relatedChatsDict = relatedChats.ToDictionary(c => c.Id);
            var relatedUsersDict = relatedUsers.ToDictionary(u => u.UniqueName);
            var relatedMessagesDict = relatedMessages.ToDictionary(m => m.Id);

            List<GetMessageResponse> responses = new List<GetMessageResponse>();

            foreach (Message message in messages)
            {
                relatedMessagesDict.TryGetValue(message.QuotedId ?? string.Empty, out var quotedMessage);
                responses.Add(Assemble(message, relatedChatsDict, relatedUsersDict, quotedMessage));
            }

            return responses;
        }

        public GetMessageResponse Assemble(
            Message message,
            IEnumerable<Chat> relatedChats,
            IEnumerable<User> relatedUsers,
            Message? quotedMessage
            )
        {
            return Assemble(message, relatedChats.ToDictionary(c => c.Id), relatedUsers.ToDictionary(u => u.UniqueName), quotedMessage);
        }

        public GetMessageResponse Assemble(
            Message message,
            Dictionary<string, Chat> relatedChats,
            Dictionary<string, User> relatedUsers,
            Message? quotedMessage
            )
        {
            var sourceChat = relatedChats[message.ChatId];
            relatedUsers.TryGetValue(message.SenderUniqueName, out var sourceUser);

            GetMessageResponse response = sourceUser == null ? new GetMessageResponse(message, sourceChat) : new GetMessageResponse(message, sourceUser, sourceChat);

            if(message.QuotedId != null && quotedMessage != null)
            {
                Chat quotedChat = relatedChats[quotedMessage.ChatId];
                relatedUsers.TryGetValue(quotedMessage.SenderUniqueName, out var quotedUser);

                response.QuotedMessage = quotedUser == null ? new GetMessageResponse(quotedMessage, quotedChat) : new GetMessageResponse(quotedMessage, quotedUser, quotedChat);
            }

            foreach (KeyValuePair<string, List<string>> kvp in message.Reactions)
            {
                List<GetMessageResponseUser> sources = new List<GetMessageResponseUser>();
                foreach (string uniqueName in kvp.Value)
                {
                    if (relatedUsers.TryGetValue(uniqueName, out var source))
                        sources.Add(new GetMessageResponseUser(source.UniqueName, source.DisplayName));
                }
                response.Reactions.Add(kvp.Key, sources);
            }

            response.ImageUrls = message.ImageUrls;

            return response;
        }
    }
}
