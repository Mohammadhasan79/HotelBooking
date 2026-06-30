export interface ApiResult<T> {
  success: boolean;
  message: string;
  data?: T;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
}

export interface LoginResponse {
  isSuccess: boolean;
  message: string;
  token?: TokenResponse;
}

export interface RegisterResponse {
  isSuccess: boolean;
  message: string;
}

export interface Hotel {
  id: number;
  name: string;
  description: string;
  city: string;
  url?: string;
  address: string;
}

export interface Room {
  id: number;
  hotelId: number;
  title: string;
  url?: string;
  pricePernight: number;
  capacity: number;
  isAvailable: boolean;
}

export interface HotelWithRooms {
  hotelId: number;
  name: string;
  description: string;
  url?: string;
  address: string;
  rooms: Room[];
}

export interface SearchByCityAndDate {
  city: string;
  checkInTime: string;
  checkOutTime: string;
}

export interface CreateHotel {
  name: string;
  description: string;
  city: string;
  url?: string;
  address: string;
}

export interface UpdateHotel extends CreateHotel {}

export interface CreateRoom {
  hotelId: number;
  title: string;
  url?: string;
  pricePernight: number;
  capacity: number;
  isAvailable: boolean;
}

export interface UpdateRoom {
  title: string;
  url?: string;
  pricePernight: number;
  capacity: number;
  isAvailable: boolean;
}

export const BookingStatus = {
  Pending: 1,
  Confirmed: 2,
  Cancelled: 3,
  Completed: 4,
} as const;

export type BookingStatus = (typeof BookingStatus)[keyof typeof BookingStatus];

export const PaymentStatus = {
  Pending: 1,
  Completed: 2,
  Failed: 3,
} as const;

export type PaymentStatus = (typeof PaymentStatus)[keyof typeof PaymentStatus];

export interface Booking {
  id: number;
  userId: string;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: BookingStatus;
}

export interface CreateBooking {
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
}

export interface Payment {
  id: number;
  bookingId: number;
  userId: string;
  amount: number;
  status: PaymentStatus;
  createdAt: string;
}

export interface AuthUser {
  id: string;
  email: string;
  roles: string[];
}
