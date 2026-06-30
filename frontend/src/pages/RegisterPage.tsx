import { useState, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ErrorMessage } from '../components/ErrorMessage';
import { useAuth } from '../context/AuthContext';

export function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  async function handleSubmit(event: FormEvent) {
    event.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    const message = await register({ firstName, lastName, email, password });
    setLoading(false);

    if (message) {
      setError(message);
      return;
    }

    setSuccess('ثبت‌نام موفق بود. اکنون وارد شوید.');
    setTimeout(() => navigate('/login'), 1500);
  }

  return (
    <div className="auth-page">
      <section className="card auth-card">
        <h1>ثبت‌نام</h1>
        <p className="muted">حساب کاربری جدید بسازید</p>

        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="firstName">نام</label>
              <input
                id="firstName"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="lastName">نام خانوادگی</label>
              <input
                id="lastName"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label htmlFor="registerEmail">ایمیل</label>
            <input
              id="registerEmail"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="form-group">
            <label htmlFor="registerPassword">رمز عبور</label>
            <input
              id="registerPassword"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <ErrorMessage message={error} />
          {success && <div className="alert alert-success">{success}</div>}
          <button type="submit" className="btn btn-primary btn-block" disabled={loading}>
            {loading ? 'در حال ثبت...' : 'ثبت‌نام'}
          </button>
        </form>

        <p className="auth-footer">
          قبلاً ثبت‌نام کرده‌اید؟ <Link to="/login">ورود</Link>
        </p>
      </section>
    </div>
  );
}
