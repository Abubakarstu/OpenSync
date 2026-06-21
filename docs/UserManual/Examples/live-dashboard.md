# Live Dashboard with OpenSync

```csharp
// Server pushes metrics via Document updates
var doc = await client.Documents.GetOrCreateAsync("dashboard-metrics");

// Update metrics in real-time
await doc.SetAsync(new
{
    activeUsers = 1423,
    requestsPerSecond = 89,
    errorRate = 0.02,
    uptime = "99.97%"
});
```

```javascript
// JavaScript client receives updates
const client = new OpenSyncClient('https://localhost:5001');
await client.connect(token);

const metrics = client.document('dashboard-metrics');
metrics.onUpdated = (data) => {
    document.getElementById('active-users').textContent = data.activeUsers;
    document.getElementById('rps').textContent = data.requestsPerSecond;
    document.getElementById('uptime').textContent = data.uptime;
};
```
