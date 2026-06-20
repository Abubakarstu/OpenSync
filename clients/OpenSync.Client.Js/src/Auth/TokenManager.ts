export class TokenManager {
  private apiKey: string;
  private token: string | null = null;
  private expiresAt: number = 0;
  private baseUrl: string;

  constructor(baseUrl: string, apiKey: string) {
    this.baseUrl = baseUrl;
    this.apiKey = apiKey;
  }

  async getToken(): Promise<string> {
    if (this.token && Date.now() < this.expiresAt) {
      return this.token;
    }

    const response = await fetch(`${this.baseUrl}/api/v1/sync/tokens`, {
      method: 'POST',
      headers: {
        'X-Api-Key': this.apiKey,
        'Content-Type': 'application/json'
      },
      body: '{}'
    });

    const data = await response.json();
    this.token = data.data.token;
    this.expiresAt = Date.now() + 55 * 60 * 1000;
    return this.token!;
  }
}
