using MediatR;
using OpenSync.Application.Common.Abstractions;
using OpenSync.Application.Common.Models;
using OpenSync.Core.Entities;

namespace OpenSync.Application.Channels.Queries.ListChannelMembers;

public record ListChannelMembersQuery(Guid ChannelId, PageRequest PageRequest) : IQuery<Common.Result<PagedResult<ChannelMember>>>;
