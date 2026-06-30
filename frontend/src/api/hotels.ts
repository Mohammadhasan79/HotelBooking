import { apiRequest } from './client';
import type {
  ApiResult,
  CreateHotel,
  Hotel,
  HotelWithRooms,
  SearchByCityAndDate,
  UpdateHotel,
} from '../types';

export async function getHotels(): Promise<ApiResult<Hotel[]>> {
  return apiRequest<ApiResult<Hotel[]>>('/hotels/api/hotels');
}

export async function getHotelById(id: number): Promise<ApiResult<Hotel>> {
  return apiRequest<ApiResult<Hotel>>(`/hotels/api/hotels/${id}`);
}

export async function searchHotels(
  payload: SearchByCityAndDate,
): Promise<ApiResult<HotelWithRooms[]>> {
  return apiRequest<ApiResult<HotelWithRooms[]>>('/hotels/api/hotels/SearchByCityAndDate', {
    method: 'POST',
    body: JSON.stringify(payload),
  });
}

export async function createHotel(payload: CreateHotel): Promise<ApiResult<Hotel>> {
  return apiRequest<ApiResult<Hotel>>('/hotels/api/hotels', {
    method: 'POST',
    body: JSON.stringify(payload),
  });
}

export async function updateHotel(
  id: number,
  payload: UpdateHotel,
): Promise<ApiResult<Hotel>> {
  return apiRequest<ApiResult<Hotel>>(`/hotels/api/hotels/${id}`, {
    method: 'PUT',
    body: JSON.stringify(payload),
  });
}

export async function deleteHotel(id: number): Promise<ApiResult<unknown>> {
  return apiRequest<ApiResult<unknown>>(`/hotels/api/hotels/${id}`, {
    method: 'DELETE',
  });
}
