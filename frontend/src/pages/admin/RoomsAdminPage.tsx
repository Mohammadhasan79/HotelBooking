import { useEffect, useState, type FormEvent } from 'react';
import { getHotels } from '../../api/hotels';
import { createRoom, deleteRoom, getRooms, updateRoom } from '../../api/rooms';
import { ErrorMessage } from '../../components/ErrorMessage';
import { LoadingSpinner } from '../../components/LoadingSpinner';
import type { CreateRoom, Hotel, Room } from '../../types';
import { formatPrice } from '../../utils/format';

const emptyForm: CreateRoom = {
  hotelId: 0,
  title: '',
  url: '',
  pricePernight: 0,
  capacity: 1,
  isAvailable: true,
};

export function RoomsAdminPage() {
  const [rooms, setRooms] = useState<Room[]>([]);
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [form, setForm] = useState<CreateRoom>(emptyForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  async function loadData() {
    setLoading(true);
    try {
      const [roomsResult, hotelsResult] = await Promise.all([getRooms(), getHotels()]);
      if (roomsResult.success && roomsResult.data) setRooms(roomsResult.data);
      if (hotelsResult.success && hotelsResult.data) {
        setHotels(hotelsResult.data);
        if (!form.hotelId && hotelsResult.data[0]) {
          setForm((prev) => ({ ...prev, hotelId: hotelsResult.data![0].id }));
        }
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadData();
  }, []);

  function startEdit(room: Room) {
    setEditingId(room.id);
    setForm({
      hotelId: room.hotelId,
      title: room.title,
      url: room.url ?? '',
      pricePernight: room.pricePernight,
      capacity: room.capacity,
      isAvailable: room.isAvailable,
    });
  }

  function resetForm() {
    setEditingId(null);
    setForm({ ...emptyForm, hotelId: hotels[0]?.id ?? 0 });
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setError('');

    try {
      const payload = {
        title: form.title,
        url: form.url,
        pricePernight: Number(form.pricePernight),
        capacity: Number(form.capacity),
        isAvailable: form.isAvailable,
      };

      const result = editingId
        ? await updateRoom(editingId, payload)
        : await createRoom({ ...form, ...payload, hotelId: Number(form.hotelId) });

      if (!result.success) {
        setError(result.message);
        return;
      }

      resetForm();
      await loadData();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id: number) {
    if (!confirm('این اتاق حذف شود؟')) return;
    try {
      const result = await deleteRoom(id);
      if (!result.success) {
        setError(result.message);
        return;
      }
      await loadData();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    }
  }

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <ErrorMessage message={error} />

      <section className="card admin-form-card">
        <h2>{editingId ? 'ویرایش اتاق' : 'افزودن اتاق'}</h2>
        <form onSubmit={handleSubmit} className="admin-form">
          {!editingId && (
            <div className="form-group">
              <label>هتل</label>
              <select
                value={form.hotelId}
                onChange={(e) => setForm({ ...form, hotelId: Number(e.target.value) })}
                required
              >
                {hotels.map((hotel) => (
                  <option key={hotel.id} value={hotel.id}>
                    {hotel.name}
                  </option>
                ))}
              </select>
            </div>
          )}
          <div className="form-row">
            <div className="form-group">
              <label>عنوان</label>
              <input
                value={form.title}
                onChange={(e) => setForm({ ...form, title: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label>قیمت هر شب</label>
              <input
                type="number"
                min="0"
                value={form.pricePernight}
                onChange={(e) => setForm({ ...form, pricePernight: Number(e.target.value) })}
                required
              />
            </div>
            <div className="form-group">
              <label>ظرفیت</label>
              <input
                type="number"
                min="1"
                value={form.capacity}
                onChange={(e) => setForm({ ...form, capacity: Number(e.target.value) })}
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label>آدرس تصویر (URL)</label>
            <input
              value={form.url}
              onChange={(e) => setForm({ ...form, url: e.target.value })}
            />
          </div>
          <label className="checkbox-label">
            <input
              type="checkbox"
              checked={form.isAvailable}
              onChange={(e) => setForm({ ...form, isAvailable: e.target.checked })}
            />
            موجود
          </label>
          <div className="form-actions">
            <button type="submit" className="btn btn-primary" disabled={submitting}>
              {submitting ? '...' : editingId ? 'ذخیره' : 'افزودن'}
            </button>
            {editingId && (
              <button type="button" className="btn btn-ghost" onClick={resetForm}>
                انصراف
              </button>
            )}
          </div>
        </form>
      </section>

      <section className="card table-wrap">
        <table>
          <thead>
            <tr>
              <th>شناسه</th>
              <th>هتل</th>
              <th>عنوان</th>
              <th>قیمت</th>
              <th>ظرفیت</th>
              <th>موجود</th>
              <th>عملیات</th>
            </tr>
          </thead>
          <tbody>
            {rooms.map((room) => (
              <tr key={room.id}>
                <td>{room.id}</td>
                <td>{room.hotelId}</td>
                <td>{room.title}</td>
                <td>{formatPrice(room.pricePernight)}</td>
                <td>{room.capacity}</td>
                <td>{room.isAvailable ? 'بله' : 'خیر'}</td>
                <td className="actions-cell">
                  <button type="button" className="btn btn-ghost btn-sm" onClick={() => startEdit(room)}>
                    ویرایش
                  </button>
                  <button type="button" className="btn btn-danger btn-sm" onClick={() => handleDelete(room.id)}>
                    حذف
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </section>
    </div>
  );
}
