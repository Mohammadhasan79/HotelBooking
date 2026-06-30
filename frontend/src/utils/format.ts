import { BookingStatus, PaymentStatus } from '../types';

export function formatPrice(value: number): string {
  return `${value.toLocaleString('fa-IR')} تومان`;
}

export function formatDate(value: string): string {
  return new Date(value).toLocaleDateString('fa-IR');
}

export function bookingStatusLabel(status: BookingStatus): string {
  switch (status) {
    case BookingStatus.Pending:
      return 'در انتظار';
    case BookingStatus.Confirmed:
      return 'تأیید شده';
    case BookingStatus.Cancelled:
      return 'لغو شده';
    case BookingStatus.Completed:
      return 'تکمیل شده';
    default:
      return 'نامشخص';
  }
}

export function paymentStatusLabel(status: PaymentStatus): string {
  switch (status) {
    case PaymentStatus.Pending:
      return 'در انتظار پرداخت';
    case PaymentStatus.Completed:
      return 'پرداخت شده';
    case PaymentStatus.Failed:
      return 'ناموفق';
    default:
      return 'نامشخص';
  }
}

export function statusClass(status: number): string {
  if (status === BookingStatus.Confirmed || status === PaymentStatus.Completed) {
    return 'badge badge-success';
  }
  if (status === BookingStatus.Cancelled || status === PaymentStatus.Failed) {
    return 'badge badge-danger';
  }
  return 'badge badge-warning';
}
