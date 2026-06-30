const API_BASE = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:7068';
const TOKEN_KEY = 'hb_access_token';
const REFRESH_KEY = 'hb_refresh_token';

export function getAccessToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

export function getRefreshToken(): string | null {
  return localStorage.getItem(REFRESH_KEY);
}

export function setTokens(accessToken: string, refreshToken: string): void {
  localStorage.setItem(TOKEN_KEY, accessToken);
  localStorage.setItem(REFRESH_KEY, refreshToken);
}

export function clearTokens(): void {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(REFRESH_KEY);
}

async function refreshAccessToken(): Promise<string | null> {
  const refreshToken = getRefreshToken();
  if (!refreshToken) return null;

  const response = await fetch(`${API_BASE}/identity/api/auth/RefreshToken?refreshToken=${encodeURIComponent(refreshToken)}`, {
    method: 'PUT',
  });

  if (!response.ok) {
    clearTokens();
    return null;
  }

  const data = await response.json();
  if (!data.isSuccess || !data.token) {
    clearTokens();
    return null;
  }

  setTokens(data.token.accessToken, data.token.refreshToken);
  return data.token.accessToken;
}

export async function apiRequest<T>(
  path: string,
  options: RequestInit = {},
  retry = true,
): Promise<T> {
  const headers = new Headers(options.headers);
  if (!headers.has('Content-Type') && options.body) {
    headers.set('Content-Type', 'application/json');
  }

  const token = getAccessToken();
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers,
  });

  if (response.status === 401 && retry) {
    const newToken = await refreshAccessToken();
    if (newToken) {
      return apiRequest<T>(path, options, false);
    }
  }

  if (!response.ok) {
    let message = 'خطایی رخ داد';
    try {
      const errorBody = await response.json();
      message = errorBody.message ?? errorBody.title ?? message;
    } catch {
      message = response.statusText || message;
    }
    throw new Error(message);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json() as Promise<T>;
}
