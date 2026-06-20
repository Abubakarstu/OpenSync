import { OpenSyncClient, SyncEvent } from './OpenSyncClient';

export class SyncChannel {
  private client: OpenSyncClient;
  private name: string;

  constructor(client: OpenSyncClient, name: string) {
    this.client = client;
    this.name = name;
  }

  async subscribe(): Promise<void> {
    await this.client.subscribe('channel', this.name);
  }

  async join(metadata?: any): Promise<void> {
    await this.client.httpPost(`/channels/${this.name}/members`, { identity: 'default', metadata });
  }

  async leave(): Promise<void> {
    await this.client.httpPost(`/channels/${this.name}/leave`, {});
  }

  async sendMessage(data: any): Promise<void> {
    await this.client.httpPost(`/channels/${this.name}/messages`, { data });
  }

  onMemberJoined(handler: (data: any) => void): void {
    this.client.on('member_joined', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }

  onMemberLeft(handler: (data: any) => void): void {
    this.client.on('member_left', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }

  onMessage(handler: (data: any) => void): void {
    this.client.on('message_broadcast', (event: SyncEvent) => {
      if (event.object_id === this.name) handler(event.data);
    });
  }
}
