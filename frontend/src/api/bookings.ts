import { apiRequest } from './client';
import type { ApiResult, Booking, CreateBooking } from '../types';

export async function getMyBookings(): Promise<ApiResult<Booking[]>> {
  return apiRequest<ApiResult<Booking[]>>('/bookings/api/bookings/my');
}

export async function getAllBookings(): Promise<ApiResult<Booking[]>> {
  return apiRequest<ApiResult<Booking[]>>('/bookings/api/bookings');
}

export async function createBooking(payload: CreateBooking): Promise<ApiResult<Booking>> {
  return apiRequest<ApiResult<Booking>>('/bookings/api/bookings', {
    method: 'POST',
    body: JSON.stringify(payload),
  });
}

export async function cancelBooking(id: number): Promise<ApiResult<unknown>> {
  return apiRequest<ApiResult<unknown>>(`/bookings/api/bookings/${id}/cancel`, {
    method: 'DELETE',
  });
}
