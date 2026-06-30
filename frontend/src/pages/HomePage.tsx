import { useEffect, useState, type FormEvent } from 'react';
import { Link } from 'react-router-dom';
import { getHotels, searchHotels } from '../api/hotels';
import { ErrorMessage } from '../components/ErrorMessage';
import { LoadingSpinner } from '../components/LoadingSpinner';
import type { Hotel, HotelWithRooms } from '../types';
import { formatPrice } from '../utils/format';

export function HomePage() {
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [searchResults, setSearchResults] = useState<HotelWithRooms[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [searching, setSearching] = useState(false);
  const [error, setError] = useState('');
  const [city, setCity] = useState('');
  const [checkIn, setCheckIn] = useState('');
  const [checkOut, setCheckOut] = useState('');

  useEffect(() => {
    getHotels()
      .then((result) => {
        if (result.success && result.data) {
          setHotels(result.data);
        } else {
          setError(result.message || 'خطا در دریافت هتل‌ها');
        }
      })
      .catch((err: Error) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  async function handleSearch(event: FormEvent) {
    event.preventDefault();
    setError('');
    setSearching(true);

    try {
      const result = await searchHotels({
        city,
        checkInTime: new Date(checkIn).toISOString(),
        checkOutTime: new Date(checkOut).toISOString(),
      });

      if (!result.success) {
        setError(result.message);
        setSearchResults([]);
        return;
      }

      setSearchResults(result.data ?? []);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا در جستجو');
    } finally {
      setSearching(false);
    }
  }

  function resetSearch() {
    setSearchResults(null);
    setCity('');
    setCheckIn('');
    setCheckOut('');
  }

  if (loading) return <LoadingSpinner />;

  const displayList = searchResults ?? hotels.map((hotel) => ({
    hotelId: hotel.id,
    name: hotel.name,
    description: hotel.description,
    url: hotel.url,
    address: hotel.address,
    rooms: [],
  }));

  return (
    <div>
      <section className="hero">
        <h1>بهترین هتل‌ها را پیدا کنید</h1>
        <p>جستجو بر اساس شهر و تاریخ ورود و خروج</p>
      </section>

      <section className="card search-card">
        <form className="search-form" onSubmit={handleSearch}>
          <div className="form-group">
            <label htmlFor="city">شهر</label>
            <input
              id="city"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              placeholder="مثلاً تهران"
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="checkIn">تاریخ ورود</label>
            <input
              id="checkIn"
              type="date"
              value={checkIn}
              onChange={(e) => setCheckIn(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="checkOut">تاریخ خروج</label>
            <input
              id="checkOut"
              type="date"
              value={checkOut}
              onChange={(e) => setCheckOut(e.target.value)}
              required
            />
          </div>
          <div className="form-actions">
            <button type="submit" className="btn btn-primary" disabled={searching}>
              {searching ? 'در حال جستجو...' : 'جستجو'}
            </button>
            {searchResults && (
              <button type="button" className="btn btn-ghost" onClick={resetSearch}>
                نمایش همه
              </button>
            )}
          </div>
        </form>
      </section>

      <ErrorMessage message={error} />

      <section className="hotel-grid">
        {displayList.length === 0 ? (
          <div className="empty-state">هتلی یافت نشد.</div>
        ) : (
          displayList.map((hotel) => (
            <article key={hotel.hotelId} className="card hotel-card">
              <div
                className="hotel-image"
                style={{
                  backgroundImage: hotel.url
                    ? `url(${hotel.url})`
                    : 'linear-gradient(135deg, #0f766e, #134e4a)',
                }}
              />
              <div className="hotel-card-body">
                <h2>{hotel.name}</h2>
                <p className="muted">{hotel.address}</p>
                <p>{hotel.description}</p>
                {searchResults && hotel.rooms.length > 0 && (
                  <p className="room-count">
                    {hotel.rooms.length} اتاق موجود از{' '}
                    {formatPrice(Math.min(...hotel.rooms.map((r) => r.pricePernight)))}
                  </p>
                )}
                <Link
                  to={`/hotels/${hotel.hotelId}`}
                  state={{ checkIn, checkOut }}
                  className="btn btn-primary"
                >
                  مشاهده جزئیات
                </Link>
              </div>
            </article>
          ))
        )}
      </section>
    </div>
  );
}
