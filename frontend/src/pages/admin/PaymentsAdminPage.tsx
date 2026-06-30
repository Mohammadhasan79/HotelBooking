import { useEffect, useState } from 'react';
import { completePayment, getAllPayments } from '../../api/payments';
import { ErrorMessage } from '../../components/ErrorMessage';
import { LoadingSpinner } from '../../components/LoadingSpinner';
import type { Payment } from '../../types';
import { PaymentStatus } from '../../types';
import { formatDate, formatPrice, paymentStatusLabel, statusClass } from '../../utils/format';

export function PaymentsAdminPage() {
  const [payments, setPayments] = useState<Payment[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [actionId, setActionId] = useState<number | null>(null);

  async function loadPayments() {
    setLoading(true);
    setError('');
    try {
      const data = await getAllPayments();
      setPayments(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadPayments();
  }, []);

  async function handleComplete(id: number) {
    setActionId(id);
    setError('');
    try {
      await completePayment(id);
      await loadPayments();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'خطا');
    } finally {
      setActionId(null);
    }
  }

  if (loading) return <LoadingSpinner />;

  return (
    <div>
      <ErrorMessage message={error} />

      <section className="card table-wrap">
        <table>
          <thead>
            <tr>
              <th>شناسه</th>
              <th>رزرو</th>
              <th>کاربر</th>
              <th>مبلغ</th>
              <th>وضعیت</th>
              <th>تاریخ</th>
              <th>عملیات</th>
            </tr>
          </thead>
          <tbody>
            {payments.length === 0 ? (
              <tr>
                <td colSpan={7} className="empty-state">
                  پرداختی ثبت نشده است.
                </td>
              </tr>
            ) : (
              payments.map((payment) => (
                <tr key={payment.id}>
                  <td>{payment.id}</td>
                  <td>{payment.bookingId}</td>
                  <td>{payment.userId}</td>
                  <td>{formatPrice(payment.amount)}</td>
                  <td>
                    <span className={statusClass(payment.status)}>
                      {paymentStatusLabel(payment.status)}
                    </span>
                  </td>
                  <td>{formatDate(payment.createdAt)}</td>
                  <td>
                    {payment.status === PaymentStatus.Pending && (
                      <button
                        type="button"
                        className="btn btn-primary btn-sm"
                        disabled={actionId === payment.id}
                        onClick={() => handleComplete(payment.id)}
                      >
                        {actionId === payment.id ? '...' : 'تأیید پرداخت'}
                      </button>
                    )}
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
