import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useState,
  type ReactNode,
} from 'react';
import { login as loginApi, logout as logoutApi, register as registerApi } from '../api/auth';
import { getAccessToken } from '../api/client';
import type { AuthUser, LoginRequest, RegisterRequest } from '../types';
import { isAdmin, parseJwt } from '../utils/jwt';

interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  login: (data: LoginRequest) => Promise<string | null>;
  register: (data: RegisterRequest) => Promise<string | null>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

function loadUser(): AuthUser | null {
  const token = getAccessToken();
  return token ? parseJwt(token) : null;
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(() => loadUser());

  const login = useCallback(async (data: LoginRequest) => {
    const result = await loginApi(data);
    if (!result.isSuccess || !result.token) {
      return result.message;
    }
    setUser(parseJwt(result.token.accessToken));
    return null;
  }, []);

  const register = useCallback(async (data: RegisterRequest) => {
    const result = await registerApi(data);
    if (!result.isSuccess) {
      return result.message;
    }
    return null;
  }, []);

  const logout = useCallback(() => {
    logoutApi();
    setUser(null);
  }, []);

  const value = useMemo(
    () => ({
      user,
      isAuthenticated: !!user,
      isAdmin: isAdmin(user),
      login,
      register,
      logout,
    }),
    [user, login, register, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
