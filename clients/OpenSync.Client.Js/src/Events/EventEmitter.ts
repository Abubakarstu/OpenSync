export class EventEmitter {
  private listeners: Map<string, ((...args: any[]) => void)[]> = new Map();

  on(event: string, handler: (...args: any[]) => void): void {
    if (!this.listeners.has(event)) {
      this.listeners.set(event, []);
    }
    this.listeners.get(event)!.push(handler);
  }

  off(event: string, handler: (...args: any[]) => void): void {
    const handlers = this.listeners.get(event);
    if (handlers) {
      const idx = handlers.indexOf(handler);
      if (idx >= 0) handlers.splice(idx, 1);
    }
  }

  emit(event: string, ...args: any[]): void {
    const handlers = this.listeners.get(event);
    if (handlers) {
      handlers.forEach(h => h(...args));
    }
  }

  removeAll(): void {
    this.listeners.clear();
  }
}
