import { Transport } from './Transport';
import { TokenManager } from '../Auth/TokenManager';

export class WebSocketTransport implements Transport {
  private ws: WebSocket | null = null;
  private url: string;
  private tokenManager: TokenManager;
  private messageHandlers: ((data: string) => void)[] = [];

  constructor(baseUrl: string, tokenManager: TokenManager) {
    this.url = baseUrl.replace(/^http/, 'ws') + '/ws';
    this.tokenManager = tokenManager;
  }

  async connect(): Promise<void> {
    const token = await this.tokenManager.getToken();
    return new Promise((resolve, reject) => {
      this.ws = new WebSocket(this.url, [], { headers: { Authorization: `Bearer ${token}` } } as any);

      this.ws.onopen = () => resolve();
      this.ws.onerror = (err) => reject(err);

      this.ws.onmessage = (event: MessageEvent) => {
        this.messageHandlers.forEach(h => h(event.data));
      };

      this.ws.onclose = () => {
        // Auto-reconnect logic could go here
      };
    });
  }

  async disconnect(): Promise<void> {
    this.ws?.close();
    this.ws = null;
  }

  async send(data: string): Promise<void> {
    if (this.ws?.readyState === WebSocket.OPEN) {
      this.ws.send(data);
    }
  }

  onMessage(handler: (data: string) => void): void {
    this.messageHandlers.push(handler);
  }
}
