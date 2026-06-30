import { NavLink, Outlet } from 'react-router-dom';

const links = [
  { to: '/admin/hotels', label: 'هتل‌ها' },
  { to: '/admin/rooms', label: 'اتاق‌ها' },
  { to: '/admin/bookings', label: 'رزروها' },
  { to: '/admin/payments', label: 'پرداخت‌ها' },
];

export function AdminLayout() {
  return (
    <div className="admin-layout">
      <section className="page-header">
        <h1>پنل مدیریت</h1>
        <p className="muted">مدیریت هتل‌ها، اتاق‌ها، رزروها و پرداخت‌ها</p>
      </section>

      <nav className="admin-tabs">
        {links.map((link) => (
          <NavLink key={link.to} to={link.to}>
            {link.label}
          </NavLink>
        ))}
      </nav>

      <Outlet />
    </div>
  );
}
