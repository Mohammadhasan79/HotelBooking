import { useEffect, useState, type FormEvent } from 'react';
import {
  createHotel,
  deleteHotel,
  getHotels,
  updateHotel,
} from '../../api/hotels';
import { ErrorMessage } from '../../components/ErrorMessage';
import { LoadingSpinner } from '../../components/LoadingSpinner';
import type { CreateHotel, Hotel } from '../../types';

const emptyForm: CreateHotel = {
  name: '',
  description: '',
  city: '',
  url: '',
  address: '',
};

export function HotelsAdminPage() {
  const [hotels, setHotels] = useState<Hotel[]>([]);
  const [form, setForm] = useState<CreateHotel>(emptyForm);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  async function loadHotels() {
    setLoading(true);
    try {
      const result = await getHotels();
      if (result.success && result.data) {
        setHotels(result.data);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadHotels();
  }, []);

  function startEdit(hotel: Hotel) {
    setEditingId(hotel.id);
    setForm({
      name: hotel.name,
      description: hotel.description,
      city: hotel.city,
      url: hotel.url ?? '',
      address: hotel.address,
    });
  }

  function resetForm() {
    setEditingId(null);
    setForm(emptyForm);
  }

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setSubmitting(true);
    setError('');

    try {
      const result = editingId
        ? await updateHotel(editingId, form)
        : await createHotel(form);

      if (!result.success) {
        setError(result.message);
        return;
      }

      resetForm();
      await loadHotels();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id: number) {
    if (!confirm('این هتل حذف شود؟')) return;
    setError('');
    try {
      const result = await deleteHotel(id);
      if (!result.success) {
        setError(result.message);
        return;
      }
      await loadHotels();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    }
  }

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <ErrorMessage message={error} />

      <section className="card admin-form-card">
        <h2>{editingId ? 'ویرایش هتل' : 'افزودن هتل'}</h2>
        <form onSubmit={handleSubmit} className="admin-form">
          <div className="form-row">
            <div className="form-group">
              <label>نام</label>
              <input
                value={form.name}
                onChange={(e) => setForm({ ...form, name: e.target.value })}
                required
              />
            </div>
            <div className="form-group">
              <label>شهر</label>
              <input
                value={form.city}
                onChange={(e) => setForm({ ...form, city: e.target.value })}
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label>آدرس</label>
            <input
              value={form.address}
              onChange={(e) => setForm({ ...form, address: e.target.value })}
              required
            />
          </div>
          <div className="form-group">
            <label>توضیحات</label>
            <textarea
              value={form.description}
              onChange={(e) => setForm({ ...form, description: e.target.value })}
              rows={3}
            />
          </div>
          <div className="form-group">
            <label>آدرس تصویر (URL)</label>
            <input
              value={form.url}
              onChange={(e) => setForm({ ...form, url: e.target.value })}
            />
          </div>
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
              <th>نام</th>
              <th>شهر</th>
              <th>آدرس</th>
              <th>عملیات</th>
            </tr>
          </thead>
          <tbody>
            {hotels.map((hotel) => (
              <tr key={hotel.id}>
                <td>{hotel.id}</td>
                <td>{hotel.name}</td>
                <td>{hotel.city}</td>
                <td>{hotel.address}</td>
                <td className="actions-cell">
                  <button type="button" className="btn btn-ghost btn-sm" onClick={() => startEdit(hotel)}>
                    ویرایش
                  </button>
                  <button type="button" className="btn btn-danger btn-sm" onClick={() => handleDelete(hotel.id)}>
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
