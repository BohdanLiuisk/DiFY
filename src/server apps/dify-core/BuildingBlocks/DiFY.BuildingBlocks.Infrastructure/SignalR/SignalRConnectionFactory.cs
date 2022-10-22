using System;
using System.Threading.Tasks;
using DiFY.BuildingBlocks.Application.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace DiFY.BuildingBlocks.Infrastructure.SignalR;

public class SignalRConnectionFactory : ISignalRConnectionFactory, IAsyncDisposable
{
    private readonly string _connectionUrl;

    private HubConnection _hubConnection;

    public SignalRConnectionFactory(string connectionUrl)
    {
        _connectionUrl = connectionUrl;
    }
    
    public async Task<ISignalRConnection> GetConnectionAsync(string hubName)
    {
        if (_hubConnection is { State: HubConnectionState.Connected }) return new SignalRConnection(_hubConnection);
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_connectionUrl}/{hubName}")
            .WithAutomaticReconnect()
            .Build();
        await _hubConnection.StartAsync();
        return new SignalRConnection(_hubConnection);
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is { State: HubConnectionState.Connected } or { State: HubConnectionState.Connecting })
        {
            await _hubConnection.DisposeAsync();
        }
    }
}