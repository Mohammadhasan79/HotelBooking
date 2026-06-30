import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom';
import { Layout } from './components/Layout';
import { ProtectedRoute } from './components/ProtectedRoute';
import { AuthProvider } from './context/AuthContext';
import { HomePage } from './pages/HomePage';
import { HotelDetailPage } from './pages/HotelDetailPage';
import { LoginPage } from './pages/LoginPage';
import { MyBookingsPage } from './pages/MyBookingsPage';
import { RegisterPage } from './pages/RegisterPage';
import { AdminLayout } from './pages/admin/AdminLayout';
import { BookingsAdminPage } from './pages/admin/BookingsAdminPage';
import { HotelsAdminPage } from './pages/admin/HotelsAdminPage';
import { PaymentsAdminPage } from './pages/admin/PaymentsAdminPage';
import { RoomsAdminPage } from './pages/admin/RoomsAdminPage';

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route element={<Layout />}>
            <Route index element={<HomePage />} />
            <Route path="hotels/:id" element={<HotelDetailPage />} />
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />

            <Route element={<ProtectedRoute />}>
              <Route path="my-bookings" element={<MyBookingsPage />} />
            </Route>

            <Route element={<ProtectedRoute adminOnly />}>
              <Route path="admin" element={<AdminLayout />}>
                <Route index element={<Navigate to="hotels" replace />} />
                <Route path="hotels" element={<HotelsAdminPage />} />
                <Route path="rooms" element={<RoomsAdminPage />} />
                <Route path="bookings" element={<BookingsAdminPage />} />
                <Route path="payments" element={<PaymentsAdminPage />} />
              </Route>
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
