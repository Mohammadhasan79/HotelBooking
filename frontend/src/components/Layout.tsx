import { Outlet } from 'react-router-dom';
import { Navbar } from './Navbar';

export function Layout() {
  return (
    <div className="app-shell">
      <Navbar />
      <main className="container main-content">
        <Outlet />
      </main>
      <footer className="footer">
        <div className="container">
          <p>HotelBooking — پلتفرم رزرو هتل با معماری میکروسرویس</p>
        </div>
      </footer>
    </div>
  );
}
