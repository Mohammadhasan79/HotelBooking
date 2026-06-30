import { apiRequest } from './client';
import type { ApiResult, CreateRoom, Room, UpdateRoom } from '../types';

export async function getRooms(): Promise<ApiResult<Room[]>> {
  return apiRequest<ApiResult<Room[]>>('/rooms/api/rooms');
}

export async function getRoomsByHotel(hotelId: number): Promise<ApiResult<Room[]>> {
  return apiRequest<ApiResult<Room[]>>(`/rooms/api/rooms/hotel/${hotelId}`);
}

export async function createRoom(payload: CreateRoom): Promise<ApiResult<Room>> {
  return apiRequest<ApiResult<Room>>('/rooms/api/rooms', {
    method: 'POST',
    body: JSON.stringify(payload),
  });
}

export async function updateRoom(
  id: number,
  payload: UpdateRoom,
): Promise<ApiResult<Room>> {
  return apiRequest<ApiResult<Room>>(`/rooms/api/rooms/${id}`, {
    method: 'PUT',
    body: JSON.stringify(payload),
  });
}

export async function deleteRoom(id: number): Promise<ApiResult<unknown>> {
  return apiRequest<ApiResult<unknown>>(`/rooms/api/rooms/${id}`, {
    method: 'DELETE',
  });
}
