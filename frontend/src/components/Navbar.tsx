import { Link, NavLink } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export function Navbar() {
  const { isAuthenticated, isAdmin, logout, user } = useAuth();

  return (
    <header className="navbar">
      <div className="container navbar-inner">
        <Link to="/" className="brand">
          <span className="brand-icon">🏨</span>
          <span>رزرو هتل</span>
        </Link>

        <nav className="nav-links">
          <NavLink to="/" end>
            جستجو
          </NavLink>
          {isAuthenticated && (
            <NavLink to="/my-bookings">رزروهای من</NavLink>
          )}
          {isAdmin && (
            <NavLink to="/admin">پنل مدیریت</NavLink>
          )}
        </nav>

        <div className="nav-actions">
          {isAuthenticated ? (
            <>
              <span className="user-chip">{user?.email}</span>
              <button type="button" className="btn btn-ghost" onClick={logout}>
                خروج
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="btn btn-ghost">
                ورود
              </Link>
              <Link to="/register" className="btn btn-primary">
                ثبت‌نام
              </Link>
            </>
          )}
        </div>
      </div>
    </header>
  );
}
