import { OpenSyncClient, SyncEvent } from './OpenSyncClient';

export class SyncDocument {
  private client: OpenSyncClient;
  private name: string;

  constructor(client: OpenSyncClient, name: string) {
    this.client = client;
    this.name = name;
  }

  async subscribe(): Promise<void> {
    await this.client.subscribe('document', this.name);
  }

  async set(data: any, expectedRevision?: number): Promise<void> {
    await this.client.httpPost(`/documents/${this.name}`, { data, expected_revision: expectedRevision });
  }

  onUpdated(handler: (data: any) => void): void {
    this.client.on('object_updated', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }

  onRemoved(handler: () => void): void {
    this.client.on('object_removed', (event: SyncEvent) => {
      if (event.object_id === this.name) handler();
    });
  }
}
