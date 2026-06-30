import { useEffect, useState, type FormEvent } from 'react';
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';
import { createBooking } from '../api/bookings';
import { getHotelById } from '../api/hotels';
import { getRoomsByHotel } from '../api/rooms';
import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingSpinner } from '../components/LoadingSpinner';
import { useAuth } from '../context/AuthContext';
import type { Hotel, Room } from '../types';
import { formatPrice } from '../utils/format';

interface LocationState {
  checkIn?: string;
  checkOut?: string;
}

export function HotelDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const state = (location.state as LocationState) ?? {};
  const { isAuthenticated } = useAuth();

  const [hotel, setHotel] = useState<Hotel | null>(null);
  const [rooms, setRooms] = useState<Room[]>([]);
  const [loading, setLoading] = useState(true);
  const [bookingRoomId, setBookingRoomId] = useState<number | null>(null);
  const [checkIn, setCheckIn] = useState(state.checkIn ?? '');
  const [checkOut, setCheckOut] = useState(state.checkOut ?? '');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    const hotelId = Number(id);
    if (!hotelId) return;

    Promise.all([getHotelById(hotelId), getRoomsByHotel(hotelId)])
      .then(([hotelResult, roomsResult]) => {
        if (hotelResult.success && hotelResult.data) {
          setHotel(hotelResult.data);
        } else {
          setError(hotelResult.message || 'هتل یافت نشد');
        }

        if (roomsResult.success && roomsResult.data) {
          setRooms(roomsResult.data.filter((room) => room.isAvailable));
        }
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, [id]);

  async function handleBooking(event: FormEvent) {
    event.preventDefault();
    setError('');
    setSuccess('');

    if (!isAuthenticated) {
      navigate('/login', { state: { from: location.pathname } });
      return;
    }

    if (!bookingRoomId) {
      setError('لطفاً یک اتاق انتخاب کنید');
      return;
    }

    setSubmitting(true);
    try {
      const result = await createBooking({
        roomId: bookingRoomId,
        checkInDate: new Date(checkIn).toISOString(),
        checkOutDate: new Date(checkOut).toISOString(),
      });

      if (!result.success) {
        setError(result.message);
        return;
      }

      setSuccess('رزرو با موفقیت ثبت شد. وضعیت پرداخت به‌صورت خودکار پردازش می‌شود.');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا در ثبت رزرو');
    } finally {
      setSubmitting(false);
    }
  }

  if (loading) return <LoadingSpinner />;
  if (!hotel) {
    return (
      <div className="empty-state">
        <p>هتل یافت نشد.</p>
        <Link to="/" className="btn btn-primary">
          بازگشت
        </Link>
      </div>
    );
  }

  return (
    <div className="hotel-detail">
      <Link to="/" className="back-link">
        ← بازگشت به لیست
      </Link>

      <section className="card detail-header">
        <div
          className="detail-image"
          style={{
            backgroundImage: hotel.url
              ? `url(${hotel.url})`
              : 'linear-gradient(135deg, #0f766e, #134e4a)',
          }}
        />
        <div>
          <h1>{hotel.name}</h1>
          <p className="muted">{hotel.city} — {hotel.address}</p>
          <p>{hotel.description}</p>
        </div>
      </section>

      <ErrorMessage message={error} />
      {success && <div className="alert alert-success">{success}</div>}

      <section className="card">
        <h2>اتاق‌های موجود</h2>
        {rooms.length === 0 ? (
          <p className="empty-state">اتاقی برای این هتل ثبت نشده است.</p>
        ) : (
          <div className="room-list">
            {rooms.map((room) => (
                <label
                    key={room.id}
                    className={`room-item ${bookingRoomId === room.id ? 'selected' : ''}`}
                >
                    <div className="room-image-wrapper">
                        {room.url ? (
                            <img src={room.url} alt={room.title} className="room-image" />
                        ) : (
                            <div className="room-image-placeholder" />
                        )}
                    </div>
                    <div>
                        <strong>{room.title}</strong>
                        <p className="muted">ظرفیت {room.capacity} نفر</p>
                        <p>{formatPrice(room.pricePernight)} / شب</p>
                    </div>
                    <input
                        type="radio"
                        name="room"
                        checked={bookingRoomId === room.id}
                        onChange={() => setBookingRoomId(room.id)}
                    />
                </label>
            ))}
          </div>
        )}
      </section>

      <section className="card">
        <h2>رزرو اتاق</h2>
        <form className="booking-form" onSubmit={handleBooking}>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="detailCheckIn">تاریخ ورود</label>
              <input
                id="detailCheckIn"
                type="date"
                value={checkIn}
                onChange={(e) => setCheckIn(e.target.value)}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="detailCheckOut">تاریخ خروج</label>
              <input
                id="detailCheckOut"
                type="date"
                value={checkOut}
                onChange={(e) => setCheckOut(e.target.value)}
                required
              />
            </div>
          </div>
          <button type="submit" className="btn btn-primary" disabled={submitting}>
            {submitting ? 'در حال ثبت...' : 'ثبت رزرو'}
          </button>
        </form>
      </section>
    </div>
  );
}
