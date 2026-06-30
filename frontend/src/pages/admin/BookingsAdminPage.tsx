import { useEffect, useState } from 'react';
import { getAllBookings } from '../../api/bookings';
import { ErrorMessage } from '../../components/ErrorMessage';
import { LoadingSpinner } from '../../components/LoadingSpinner';
import type { Booking } from '../../types';
import { bookingStatusLabel, formatDate, formatPrice, statusClass } from '../../utils/format';

export function BookingsAdminPage() {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    getAllBookings()
      .then((result) => {
        if (result.success && result.data) {
          setBookings(result.data);
        } else {
          setError(result.message);
        }
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <ErrorMessage message={error} />

      <section className="card table-wrap">
        <table>
          <thead>
            <tr>
              <th>شناسه</th>
              <th>کاربر</th>
              <th>اتاق</th>
              <th>ورود</th>
              <th>خروج</th>
              <th>مبلغ</th>
              <th>وضعیت</th>
            </tr>
          </thead>
          <tbody>
            {bookings.length === 0 ? (
              <tr>
                <td colSpan={7} className="empty-state">
                  رزروی ثبت نشده است.
                </td>
              </tr>
            ) : (
              bookings.map((booking) => (
                <tr key={booking.id}>
                  <td>{booking.id}</td>
                  <td>{booking.userId}</td>
                  <td>{booking.roomId}</td>
                  <td>{formatDate(booking.checkInDate)}</td>
                  <td>{formatDate(booking.checkOutDate)}</td>
                  <td>{formatPrice(booking.totalPrice)}</td>
                  <td>
                    <span className={statusClass(booking.status)}>
                      {bookingStatusLabel(booking.status)}
                    </span>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </section>
    </div>
  );
}
