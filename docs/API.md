# OpenSync REST API

Base URL: `/api/v1/sync/services/{serviceId}`

## Authentication

### Generate Token

```
POST /api/v1/sync/tokens
```

```json
{
  "identity": "user-123",
  "ttl_seconds": 3600,
  "permissions": {
    "documents": ["read", "write"],
    "lists": ["read"],
    "maps": ["read", "write"],
    "channels": ["read", "write"],
    "streams": ["publish"]
  }
}
```

Response:

```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "expires_at": "2024-01-15T11:30:00Z"
  }
}
```

## Documents

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/documents` | Create document |
| GET | `/documents` | List documents |
| GET | `/documents/{id}` | Get document |
| PATCH | `/documents/{id}` | Partial update (merge JSON) |
| PUT | `/documents/{id}` | Full replace |
| DELETE | `/documents/{id}` | Delete |

### Create Document

```
POST /api/v1/sync/services/{serviceId}/documents
```

```json
{
  "unique_name": "user-settings",
  "data": { "theme": "dark", "language": "en" },
  "ttl_seconds": null
}
```

## Lists

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/lists` | Create list |
| GET | `/lists` | List lists |
| GET | `/lists/{id}` | Get list |
| DELETE | `/lists/{id}` | Delete list |

### List Items

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/lists/{id}/items` | Append item |
| GET | `/lists/{id}/items` | List items (paginated) |
| GET | `/lists/{id}/items/{itemId}` | Get item |
| PUT | `/lists/{id}/items/{itemId}` | Update item |
| DELETE | `/lists/{id}/items/{itemId}` | Remove item |

Query params for listing: `?page=0&page_size=50`

## Maps

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/maps` | Create map |
| GET | `/maps` | List maps |
| GET | `/maps/{id}` | Get map |
| DELETE | `/maps/{id}` | Delete map |

### Map Items

| Method | Endpoint | Description |
|--------|----------|-------------|
| PUT | `/maps/{id}/items/{key}` | Set item (upsert) |
| GET | `/maps/{id}/items` | List items |
| GET | `/maps/{id}/items/{key}` | Get item by key |
| DELETE | `/maps/{id}/items/{key}` | Remove item |

## Streams

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/streams` | Create stream |
| GET | `/streams` | List streams |
| GET | `/streams/{id}` | Get stream |
| DELETE | `/streams/{id}` | Delete stream |
| POST | `/streams/{id}/messages` | Publish message |

## Channels

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/channels` | Create channel |
| GET | `/channels` | List channels |
| GET | `/channels/{id}` | Get channel |
| DELETE | `/channels/{id}` | Delete channel |

### Channel Members

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/channels/{id}/members` | Join channel |
| GET | `/channels/{id}/members` | List members |
| PUT | `/channels/{id}/members/{identity}` | Update presence |
| DELETE | `/channels/{id}/members/{identity}` | Leave channel |

### Channel Messages

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/channels/{id}/messages` | Broadcast message |

## Health

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/sync/health` | Health check |
| GET | `/api/v1/sync/stats` | Server statistics |

## Response Format

Success:

```json
{
  "success": true,
  "data": { ... }
}
```

Paginated:

```json
{
  "success": true,
  "data": [ ... ],
  "meta": {
    "page": 0,
    "page_size": 50,
    "total_count": 230,
    "has_next": true
  }
}
```

Error:

```json
{
  "success": false,
  "error": {
    "code": "REVISION_CONFLICT",
    "message": "Expected revision 5, found 6",
    "details": { "expected_revision": 5, "current_revision": 6 }
  }
}
```
