using Microsoft.AspNetCore.Mvc;
using OpenSync.Api.Models.Responses;
using OpenSync.Core.Interfaces.Repositories;
using OpenSync.Infrastructure.Realtime.Connection;
using OpenSync.Infrastructure.Realtime.Subscriptions;

namespace OpenSync.Api.Controllers.V1;

public class StatsController : BaseController
{
    private readonly IConnectionManager _connectionManager;
    private readonly ISubscriptionManager _subscriptionManager;
    private readonly IDocumentRepository _documentRepository;
    private readonly IListRepository _listRepository;
    private readonly IMapRepository _mapRepository;
    private readonly IStreamRepository _streamRepository;
    private readonly IChannelRepository _channelRepository;

    public StatsController(
        IConnectionManager connectionManager,
        ISubscriptionManager subscriptionManager,
        IDocumentRepository documentRepository,
        IListRepository listRepository,
        IMapRepository mapRepository,
        IStreamRepository streamRepository,
        IChannelRepository channelRepository)
    {
        _connectionManager = connectionManager;
        _subscriptionManager = subscriptionManager;
        _documentRepository = documentRepository;
        _listRepository = listRepository;
        _mapRepository = mapRepository;
        _streamRepository = streamRepository;
        _channelRepository = channelRepository;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> Get()
    {
        var stats = new StatsResponse
        {
            ConnectionCount = _connectionManager.ConnectionCount,
            SubscriptionCount = _subscriptionManager.TotalSubscriptions,
            DocumentCount = await _documentRepository.CountAsync(),
            ListCount = await _listRepository.CountAsync(),
            MapCount = await _mapRepository.CountAsync(),
            StreamCount = await _streamRepository.CountAsync(),
            ChannelCount = await _channelRepository.CountAsync(),
        };

        stats.TotalObjectCount = stats.DocumentCount + stats.ListCount + stats.MapCount + stats.StreamCount + stats.ChannelCount;

        return Ok(new ApiResponse<StatsResponse> { Success = true, Data = stats });
    }
}
