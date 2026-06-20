export enum TransportType {
  WebSocket = 'websocket',
  SSE = 'sse',
  LongPolling = 'longpolling'
}

export interface Transport {
  connect(): Promise<void>;
  disconnect(): Promise<void>;
  send(data: string): Promise<void>;
  onMessage(handler: (data: string) => void): void;
}
