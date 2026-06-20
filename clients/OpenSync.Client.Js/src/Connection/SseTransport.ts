import { Transport } from './Transport';
import { TokenManager } from '../Auth/TokenManager';

export class SseTransport implements Transport {
  private eventSource: EventSource | null = null;
  private baseUrl: string;
  private tokenManager: TokenManager;
  private messageHandlers: ((data: string) => void)[] = [];

  constructor(baseUrl: string, tokenManager: TokenManager) {
    this.baseUrl = baseUrl;
    this.tokenManager = tokenManager;
  }

  async connect(): Promise<void> {
    const token = await this.tokenManager.getToken();
    this.eventSource = new EventSource(`${this.baseUrl}/api/v1/sync/events`, {
      withCredentials: true
    } as any);

    this.eventSource.onmessage = (event: MessageEvent) => {
      this.messageHandlers.forEach(h => h(event.data));
    };

    this.eventSource.onerror = () => {
      this.eventSource?.close();
    };
  }

  async disconnect(): Promise<void> {
    this.eventSource?.close();
    this.eventSource = null;
  }

  async send(_data: string): Promise<void> {
    // SSE is unidirectional - no client-to-server messages
  }

  onMessage(handler: (data: string) => void): void {
    this.messageHandlers.push(handler);
  }
}
