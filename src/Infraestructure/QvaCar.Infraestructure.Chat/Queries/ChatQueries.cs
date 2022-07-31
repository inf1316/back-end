using QvaCar.Domain.Chat;
using QvaCar.Infraestructure.Data.DbContextQuery;
using QvaCar.Seedwork.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Chat.Queries
{
    public class ChatQueries : IChatQueries
    {
        private readonly int ReturnPaginationMessageCount = 100;
        private readonly IQvaCarChatQuery contextQueries;

        public ChatQueries(IQvaCarChatQuery contextQueries) => this.contextQueries = contextQueries;

        public async Task<List<ChannelsByUserItemQueryResponse>> GetUserChannelsAsync(Guid chatUserId, CancellationToken cancellationToken)
        {
            var sql = @$"
                        SELECT 
                            Channel.{nameof(Channel.Id)}                                                                                                               as {nameof(ChannelsByUserSqlQueryResponse.Id)},
                            Channel.{nameof(Channel.AboutAd)}_{nameof(ChannelCarAd.Id)}                                                                                as {nameof(ChannelsByUserSqlQueryResponse.AboutAdId)},
                            Channel.{nameof(Channel.AboutAd)}_{nameof(ChannelCarAd.ModelVersion)}                                                                      as {nameof(ChannelsByUserSqlQueryResponse.AboutAdModelVersion)},
                            Channel.{nameof(Channel.AboutAd)}_{nameof(ChannelCarAd.MainImageFileName)}                                                                 as {nameof(ChannelsByUserSqlQueryResponse.AboutAdMainImageFileName)},
                                                                                                                                                                       
                            Channel.{nameof(Channel.AnotherParticipant)}_{nameof(ChannelUser.Id)}                                                                      as {nameof(ChannelsByUserSqlQueryResponse.AnotherParticipantId)},
                            Channel.{nameof(Channel.AnotherParticipant)}_{nameof(ChannelUser.FirstName)}                                                               as {nameof(ChannelsByUserSqlQueryResponse.AnotherParticipantFirstName)},
                            Channel.{nameof(Channel.AnotherParticipant)}_{nameof(ChannelUser.LastName)}                                                                as {nameof(ChannelsByUserSqlQueryResponse.AnotherParticipantLastName)},
                                                                                                                                                                       
                            Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.Id)}                                                               as {nameof(ChannelsByUserSqlQueryResponse.LastMessageId)},
                            Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.CreatedAtUtc)}                                                     as {nameof(ChannelsByUserSqlQueryResponse.CreatedAtUtc)},
                            Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.Text)}                                                             as {nameof(ChannelsByUserSqlQueryResponse.LastMessageText)},
                            Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.MessageType)}_{nameof(ChannelLastMessagePreview.MessageType.Id)}   as {nameof(ChannelsByUserSqlQueryResponse.LastMessageMessageTypeId)},
                            Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.MessageType)}_{nameof(ChannelLastMessagePreview.MessageType.Name)} as {nameof(ChannelsByUserSqlQueryResponse.LastMessageMessageTypeName)},
                            
                            Channel.{nameof(Channel.MyRole)}_{nameof(ChannelUserRole.Id)}                                                                              as {nameof(ChannelsByUserSqlQueryResponse.UserRoleId)},
                            Channel.{nameof(Channel.MyRole)}_{nameof(ChannelUserRole.Name)}                                                                            as {nameof(ChannelsByUserSqlQueryResponse.UserRoleName)}

                        FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel
                        WHERE Channel.ChatUserId = @UserId
                        Order by Channel.{nameof(Channel.LastMessage)}_{nameof(ChannelLastMessagePreview.Id)} Desc
                        ";
            var rawChannels = await contextQueries.QueryListAsync<ChannelsByUserSqlQueryResponse>(sql, new { UserId = chatUserId }, cancellationToken);

            var items = rawChannels
                .Select(x => new ChannelsByUserItemQueryResponse()
                {
                    Id = x.Id,
                    AboutAd = new ChannelsByUserCarAdQueryResponse()
                    {
                        Id = x.AboutAdId,
                        ModelVersion = x.AboutAdModelVersion,
                        MainImageFileName = x.AboutAdMainImageFileName,
                    },
                    AnotherParticipant = new ChannelsByUserAnotherParticipantQueryResponse()
                    {
                        Id = x.AnotherParticipantId,
                        FirstName = x.AnotherParticipantFirstName,
                        LastName = x.AnotherParticipantLastName,
                    },
                    LastMessagePreview = new ChannelsByUserChannelLastMessagePreviewQueryResponse()
                    {
                        Id = x.LastMessageId,
                        CreatedAtUtc = x.CreatedAtUtc,
                        MessageType = new ChannelsByUserMessageTypeQueryResponse()
                        {
                            Id = x.LastMessageMessageTypeId,
                            Name = x.LastMessageMessageTypeName,
                        },
                        Text = x.LastMessageText,
                    },
                    RoleInChannel = new ChannelsByUserRoleQueryResponse()
                    {
                        Id = x.UserRoleId,
                        Name = x.UserRoleName,
                    }
                })
                .ToList();

            return items;
        }

        public async Task<List<LatestsMessagesByChannelQueryResponse>> GetLatestMessages(Guid chatUserId, Guid channelId, CancellationToken cancellationToken)
        {
            var @params = new { UserId = chatUserId, ChannelId = channelId };
            var sql = @$"
                        Select *                           
                        From 
                        	(
                        		SELECT Top {ReturnPaginationMessageCount}
                        			Message.{nameof(ChatMessage.Id)}                                        as {nameof(LatestsMessagesByChannelSqlQueryResponse.Id)},
                                    Message.{nameof(ChatMessage.CreatedAtUtc)}                              as {nameof(LatestsMessagesByChannelSqlQueryResponse.CreatedAtUtc)},
                        		    Message.{nameof(ChatMessage.Text)}                                      as {nameof(LatestsMessagesByChannelSqlQueryResponse.Text)},
                        		    Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Id)}      as {nameof(LatestsMessagesByChannelSqlQueryResponse.MessageTypeId)},
                        		    Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Name)}    as {nameof(LatestsMessagesByChannelSqlQueryResponse.MessageTypeName)}
                        		FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Messages} as Message
                        			JOIN {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel on Message.ChannelId = Channel.{nameof(Channel.Id)}
                        		Where Channel.ChatUserId = @UserId and Channel.{nameof(Channel.Id)} =  @ChannelId 
                        		ORDER BY Message.{nameof(ChatMessage.Id)} Desc
                        	)	as ReturnMessages
                        ORDER BY ReturnMessages.Id asc
                        ";
            var rawMessages = (await contextQueries.QueryListAsync<LatestsMessagesByChannelSqlQueryResponse>(sql, @params, cancellationToken)).ToList();

            if (!rawMessages.Any())
                throw new EntityNotFoundException($"Channel '{channelId}' not found");

            return rawMessages
                .Select(message => new LatestsMessagesByChannelQueryResponse()
                {
                    Id = message.Id,
                    CreatedAtUtc = message.CreatedAtUtc,
                    Text = message.Text,
                    MessageType = new LatestsMessagesByChannelMessageTypeQueryResponse()
                    {
                        Id = message.MessageTypeId,
                        Name = message.MessageTypeName,
                    }
                })
                .ToList();
        }

        public async Task<List<MessagesByChannelAfterResponse>> GetMessagesAfter(Guid chatUserId, Guid channelId, long referenceMessageId, CancellationToken cancellationToken)
        {
            var @params = new { UserId = chatUserId, ChannelId = channelId, MessageId = referenceMessageId };
            var sql = @$"
                        SELECT 
                        	CASE When EXISTS 
                        		(
                        			Select Message.{nameof(ChatMessage.Id)} 
                        			FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Messages} as Message
                        				JOIN {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel on Message.ChannelId = Channel.{nameof(Channel.Id)}
                        			Where Channel.ChatUserId = @UserId and Channel.{nameof(Channel.Id)} =  @ChannelId and Message.{nameof(ChatMessage.Id)} = @MessageId
                        		) 
                        	THEN 1
                        	ELSE 0 
                        	END as ExistChannelAndMessage;

                        SELECT Top {ReturnPaginationMessageCount}
                        	Message.{nameof(ChatMessage.Id)}                                            as {nameof(GetMessagesAfterByChannelSqlQueryResponse.Id)},
                            Message.{nameof(ChatMessage.CreatedAtUtc)}                                  as {nameof(GetMessagesAfterByChannelSqlQueryResponse.CreatedAtUtc)},
                            Message.{nameof(ChatMessage.Text)}                                          as {nameof(GetMessagesAfterByChannelSqlQueryResponse.Text)},
                            Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Id)}          as {nameof(GetMessagesAfterByChannelSqlQueryResponse.MessageTypeId)},
                            Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Name)}        as {nameof(GetMessagesAfterByChannelSqlQueryResponse.MessageTypeName)}
                        FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Messages} as Message
                        	JOIN {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel on Message.ChannelId = Channel.{nameof(Channel.Id)}
                        Where     Channel.ChatUserId = @UserId and Channel.{nameof(Channel.Id)} =  @ChannelId 
                                    and Message.{nameof(ChatMessage.Id)} > @MessageId
                        ORDER BY Message.{nameof(ChatMessage.Id)} Asc;
                        ";

            (var existChannelOrMessage, var rawMessages) = (await contextQueries.QueryMultipleAsync<bool, GetMessagesAfterByChannelSqlQueryResponse>(sql, @params, cancellationToken));

            if (!existChannelOrMessage)
                throw new EntityNotFoundException($"Channel '{channelId}' or message '{referenceMessageId}' not found");

            return rawMessages
                .Select(message => new MessagesByChannelAfterResponse()
                {
                    Id = message.Id,
                    Text = message.Text,
                    CreatedAtUtc = message.CreatedAtUtc,
                    MessageType = new MessagesByChannelAfterMessageTypeResponse()
                    {
                        Id = message.MessageTypeId,
                        Name = message.MessageTypeName,
                    }
                }).ToList();
        }
        
        public async Task<List<MessagesByChannelBeforeResponse>> GetMessagesBefore(Guid chatUserId, Guid channelId, long referenceMessageId, CancellationToken cancellationToken)
        {
            var @params = new { UserId = chatUserId, ChannelId = channelId, MessageId = referenceMessageId };
            var sql = @$"
                        SELECT 
                        	CASE When EXISTS 
                        		(
                        			Select Message.{nameof(ChatMessage.Id)} 
                        			FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Messages} as Message
                        				JOIN {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel on Message.ChannelId = Channel.{nameof(Channel.Id)}
                        			Where Channel.ChatUserId = @UserId and Channel.{nameof(Channel.Id)} =  @ChannelId and Message.{nameof(ChatMessage.Id)} = @MessageId
                        		) 
                        	THEN 1
                        	ELSE 0 
                        	END as ExistChannelAndMessage;
                        Select *                           
                        From 
                        	(
                                SELECT Top {ReturnPaginationMessageCount}
                                	Message.{nameof(ChatMessage.Id)}                                        as {nameof(GetMessagesBeforeByChannelSqlQueryResponse.Id)},
                                    Message.{nameof(ChatMessage.CreatedAtUtc)}                                      as {nameof(GetMessagesBeforeByChannelSqlQueryResponse.CreatedAtUtc)},
                                    Message.{nameof(ChatMessage.Text)}                                      as {nameof(GetMessagesBeforeByChannelSqlQueryResponse.Text)},
                                    Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Id)}      as {nameof(GetMessagesBeforeByChannelSqlQueryResponse.MessageTypeId)},
                                    Message.{nameof(ChatMessage.MessageType)}_{nameof(MessageType.Name)}    as {nameof(GetMessagesBeforeByChannelSqlQueryResponse.MessageTypeName)}
                                FROM {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Messages} as Message
                                	JOIN {EntityConfigurationConstants.ChatTableSchema}.{EntityConfigurationConstants.Channels} as Channel on Message.ChannelId = Channel.{nameof(Channel.Id)}
                                Where     Channel.ChatUserId = @UserId and Channel.{nameof(Channel.Id)} =  @ChannelId 
                                            and Message.{nameof(ChatMessage.Id)} < @MessageId
                                ORDER BY Message.{nameof(ChatMessage.Id)} Desc
                            ) as ReturnMessages
                        ORDER BY ReturnMessages.Id asc;
                        ";

            (var existChannelOrMessage, var rawMessages) = (await contextQueries.QueryMultipleAsync<bool, GetMessagesBeforeByChannelSqlQueryResponse>(sql, @params, cancellationToken));

            if (!existChannelOrMessage)
                throw new EntityNotFoundException($"Channel '{channelId}' or message '{referenceMessageId}' not found");

            return rawMessages
                .Select(message => new MessagesByChannelBeforeResponse()
                {
                    Id = message.Id,
                    CreatedAtUtc = message.CreatedAtUtc,
                    Text = message.Text,
                    MessageType = new MessagesByChannelBeforeMessageTypeResponse()
                    {
                        Id = message.MessageTypeId,
                        Name = message.MessageTypeName,
                    }
                })
                .ToList();
        }
    }
}
