export type VisitPlace = {
  id: string;
  name: string;
  description: string;
  photoUrl: string;
  destinationId: string;
  price: number;
};

export type CreateVisitPlace = {
  name: string;
  description: string;
  photoUrl: string;
  destinationId: string;
  price: number;
};
