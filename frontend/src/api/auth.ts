import { apiRequest, setTokens, clearTokens } from './client';
import type { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../types';

export async function login(data: LoginRequest): Promise<LoginResponse> {
  const result = await apiRequest<LoginResponse>('/identity/api/auth/login', {
    method: 'POST',
    body: JSON.stringify(data),
  });

  if (result.isSuccess && result.token) {
    setTokens(result.token.accessToken, result.token.refreshToken);
  }

  return result;
}

export async function register(data: RegisterRequest): Promise<RegisterResponse> {
  return apiRequest<RegisterResponse>('/identity/api/auth/register', {
    method: 'POST',
    body: JSON.stringify(data),
  });
}

export function logout(): void {
  clearTokens();
}
