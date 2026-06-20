import { Transport, TransportType, WebSocketTransport } from './Connection/Transport';
import { SseTransport } from './Connection/SseTransport';
import { EventEmitter } from './Events/EventEmitter';
import { TokenManager } from './Auth/TokenManager';
import { SyncDocument } from './SyncDocument';
import { SyncList } from './SyncList';
import { SyncMap } from './SyncMap';
import { SyncStream } from './SyncStream';
import { SyncChannel } from './SyncChannel';

export interface SyncEvent {
  event: string;
  object_type: string;
  object_id: string;
  data?: any;
  timestamp: string;
}

export class OpenSyncClient {
  private transport: Transport;
  private emitter: EventEmitter;
  private tokenManager: TokenManager;
  private baseUrl: string;
  private connected: boolean = false;

  constructor(baseUrl: string, tokenManager: TokenManager, transportType: TransportType = TransportType.WebSocket) {
    this.baseUrl = baseUrl;
    this.tokenManager = tokenManager;
    this.emitter = new EventEmitter();

    switch (transportType) {
      case TransportType.SSE:
        this.transport = new SseTransport(baseUrl, tokenManager);
        break;
      default:
        this.transport = new WebSocketTransport(baseUrl, tokenManager);
    }

    this.transport.onMessage((data: string) => {
      try {
        const event: SyncEvent = JSON.parse(data);
        this.emitter.emit(event.event, event);
        this.emitter.emit('*', event);
      } catch (e) {
        console.error('Failed to parse event:', e);
      }
    });
  }

  async connect(): Promise<void> {
    await this.transport.connect();
    this.connected = true;
  }

  async disconnect(): Promise<void> {
    await this.transport.disconnect();
    this.connected = false;
  }

  async subscribe(objectType: string, objectId: string): Promise<void> {
    await this.send({ action: 'subscribe', object_type: objectType, object_id: objectId });
  }

  async unsubscribe(objectType: string, objectId: string): Promise<void> {
    await this.send({ action: 'unsubscribe', object_type: objectType, object_id: objectId });
  }

  async send(message: any): Promise<void> {
    await this.transport.send(JSON.stringify(message));
  }

  on(event: string, handler: (event: SyncEvent) => void): void {
    this.emitter.on(event, handler);
  }

  off(event: string, handler: (event: SyncEvent) => void): void {
    this.emitter.off(event, handler);
  }

  document(name: string): SyncDocument {
    return new SyncDocument(this, name);
  }

  list(name: string): SyncList {
    return new SyncList(this, name);
  }

  map(name: string): SyncMap {
    return new SyncMap(this, name);
  }

  stream(name: string): SyncStream {
    return new SyncStream(this, name);
  }

  channel(name: string): SyncChannel {
    return new SyncChannel(this, name);
  }

  async httpGet<T>(path: string): Promise<T> {
    const token = await this.tokenManager.getToken();
    const response = await fetch(`${this.baseUrl}/api/v1/sync${path}`, {
      headers: { Authorization: `Bearer ${token}` }
    });
    return response.json();
  }

  async httpPost<T>(path: string, body: any): Promise<T> {
    const token = await this.tokenManager.getToken();
    const response = await fetch(`${this.baseUrl}/api/v1/sync${path}`, {
      method: 'POST',
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(body)
    });
    return response.json();
  }
}
