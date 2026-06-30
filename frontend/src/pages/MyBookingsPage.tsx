import { useEffect, useState } from 'react';
import { cancelBooking, getMyBookings } from '../api/bookings';
import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingSpinner } from '../components/LoadingSpinner';
import type { Booking } from '../types';
import { BookingStatus } from '../types';
import { bookingStatusLabel, formatDate, formatPrice, statusClass } from '../utils/format';

export function MyBookingsPage() {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [actionId, setActionId] = useState<number | null>(null);

  async function loadBookings() {
    setLoading(true);
    setError('');
    try {
      const result = await getMyBookings();
      if (result.success && result.data) {
        setBookings(result.data);
      } else {
        setError(result.message || 'خطا در دریافت رزروها');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا در دریافت رزروها');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadBookings();
  }, []);

  async function handleCancel(id: number) {
    setActionId(id);
    setError('');
    try {
      const result = await cancelBooking(id);
      if (!result.success) {
        setError(result.message);
        return;
      }
      await loadBookings();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا در لغو رزرو');
    } finally {
      setActionId(null);
    }
  }

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <section className="page-header">
        <h1>رزروهای من</h1>
        <p className="muted">لیست رزروها و وضعیت آن‌ها</p>
      </section>

      <ErrorMessage message={error} />

      {bookings.length === 0 ? (
        <div className="empty-state card">هنوز رزروی ثبت نکرده‌اید.</div>
      ) : (
        <div className="table-wrap card">
          <table>
            <thead>
              <tr>
                <th>شناسه</th>
                <th>اتاق</th>
                <th>ورود</th>
                <th>خروج</th>
                <th>مبلغ</th>
                <th>وضعیت</th>
                <th>عملیات</th>
              </tr>
            </thead>
            <tbody>
              {bookings.map((booking) => (
                <tr key={booking.id}>
                  <td>{booking.id}</td>
                  <td>{booking.roomId}</td>
                  <td>{formatDate(booking.checkInDate)}</td>
                  <td>{formatDate(booking.checkOutDate)}</td>
                  <td>{formatPrice(booking.totalPrice)}</td>
                  <td>
                    <span className={statusClass(booking.status)}>
                      {bookingStatusLabel(booking.status)}
                    </span>
                  </td>
                  <td>
                    {booking.status !== BookingStatus.Cancelled &&
                      booking.status !== BookingStatus.Completed && (
                        <button
                          type="button"
                          className="btn btn-danger btn-sm"
                          disabled={actionId === booking.id}
                          onClick={() => handleCancel(booking.id)}
                        >
                          {actionId === booking.id ? '...' : 'لغو'}
                        </button>
                      )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
