# Deployment

## Docker Compose

```bash
docker compose -f docker/docker-compose.yml up -d
```

This starts:
- OpenSync API on port 5000
- PostgreSQL on port 5432
- Redis on port 6379 (optional, for multi-instance)

## Standalone

```bash
cd src/OpenSync.Api
dotnet run --urls "http://0.0.0.0:5000"
```

## Docker

```bash
docker build -f docker/Dockerfile -t opensync:latest .
docker run -p 5000:8080 opensync:latest
```

## Multi-Instance Scaling

1. Set `OpenSync:Backplane:Type` to `"Redis"` in config
2. Deploy behind a load balancer
3. All instances receive and forward events via Redis pub/sub

## Configuration

See [CONFIGURATION.md](CONFIGURATION.md) for full options.
