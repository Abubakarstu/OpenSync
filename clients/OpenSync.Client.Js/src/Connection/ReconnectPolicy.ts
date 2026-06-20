export class ReconnectPolicy {
  maxRetries: number;
  baseDelayMs: number;
  currentRetry: number = 0;

  constructor(maxRetries: number = 5, baseDelayMs: number = 1000) {
    this.maxRetries = maxRetries;
    this.baseDelayMs = baseDelayMs;
  }

  getDelay(): number {
    const delay = Math.min(this.baseDelayMs * Math.pow(2, this.currentRetry), 30000);
    this.currentRetry++;
    return delay;
  }

  reset(): void {
    this.currentRetry = 0;
  }

  canRetry(): boolean {
    return this.currentRetry < this.maxRetries;
  }
}
