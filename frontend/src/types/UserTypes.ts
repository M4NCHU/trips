export type User = {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  token?: string;
  id: string;
  roles: string[];
} | null;

export type RegisterUser = {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  user: User;
};

type UserContextType = {
  user: User | null;
  setUser: (user: User) => void;
};
