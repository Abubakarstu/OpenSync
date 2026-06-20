import { OpenSyncClient, SyncEvent } from './OpenSyncClient';

export class SyncStream {
  private client: OpenSyncClient;
  private name: string;

  constructor(client: OpenSyncClient, name: string) {
    this.client = client;
    this.name = name;
  }

  async subscribe(): Promise<void> {
    await this.client.subscribe('stream', this.name);
  }

  async publish(data: any): Promise<void> {
    await this.client.httpPost(`/streams/${this.name}/messages`, { data });
  }

  onMessage(handler: (data: any) => void): void {
    this.client.on('stream_message', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }
}
