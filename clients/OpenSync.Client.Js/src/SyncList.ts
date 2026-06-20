import { OpenSyncClient, SyncEvent } from './OpenSyncClient';

export class SyncList {
  private client: OpenSyncClient;
  private name: string;

  constructor(client: OpenSyncClient, name: string) {
    this.client = client;
    this.name = name;
  }

  async subscribe(): Promise<void> {
    await this.client.subscribe('list', this.name);
  }

  onItemAdded(handler: (data: any) => void): void {
    this.client.on('item_added', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }

  onItemUpdated(handler: (data: any) => void): void {
    this.client.on('item_updated', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }

  onItemRemoved(handler: (data: any) => void): void {
    this.client.on('item_removed', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }
}
