import type { AuthUser } from '../types';

const ROLE_CLAIM =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export function parseJwt(token: string): AuthUser | null {
  try {
    const payload = token.split('.')[1];
    const decoded = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));

    const rolesRaw = decoded.role ?? decoded[ROLE_CLAIM] ?? [];
    const roles = Array.isArray(rolesRaw) ? rolesRaw : [rolesRaw].filter(Boolean);

    return {
      id:
        decoded.sub ??
        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ??
        '',
      email:
        decoded.email ??
        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ??
        '',
      roles,
    };
  } catch {
    return null;
  }
}

export function isAdmin(user: AuthUser | null): boolean {
  return user?.roles.some((role) => role.toLowerCase() === 'admin') ?? false;
}
