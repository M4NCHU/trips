import { FC, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";

import { Button } from "src/components/ui/button";
import ProfileNav from "src/components/Profile/ProfileNav/ProfileNav";
import { useGetReservationById } from "src/api/ResumeAPI";
import { useDestinationById } from "src/api/Destinations";

interface ReservationDetailsProps {}

const ReservationDetails: FC<ReservationDetailsProps> = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const {
    data: reservation,
    isLoading,
    isError,
  } = useGetReservationById(id || "");

  useEffect(() => {
    if (!id) {
      navigate("/reservations");
    }
  }, [id, navigate]);

  if (!id) {
    return <p>Invalid reservation ID.</p>;
  }

  if (isLoading) return <p>Loading reservation details...</p>;
  if (isError || !reservation) {
    return <p>Failed to load reservation details. Please try again.</p>;
  }

  return (
    <div>
      <ProfileNav />
      <div className="p-4">
        <h1 className="text-xl font-bold mb-4">Reservation Details</h1>
        <div className="bg-background shadow-md rounded-lg p-4">
          <p>
            <strong>ID:</strong> {reservation.id}
          </p>
          <p>
            <strong>User ID:</strong> {reservation.userId}
          </p>
          <p>
            <strong>Total Price:</strong> {reservation.totalPrice.toFixed(2)} zł
          </p>
          <p>
            <strong>Status:</strong> {reservation.status}
          </p>
          <p>
            <strong>Created At:</strong>{" "}
            {new Date(reservation.createdAt).toLocaleString()}
          </p>
          <h2 className="text-lg font-bold mt-4">Items</h2>
          <ul className="list-disc list-inside">
            {reservation.reservationItems?.map((item, index) => {
              return <ReservationItem key={item.itemId} item={item} />;
            })}
          </ul>
          <Button
            onClick={() => navigate("/profile/reservations")}
            className="bg-gray-500 text-white mt-4"
          >
            Back to Reservations List
          </Button>
        </div>
      </div>
    </div>
  );
};

// 🔥 WYDZIELONY KOMPONENT, ABY MÓC UŻYWAĆ HOOKÓW POZA PĘTLĄ
const ReservationItem: FC<{
  item: { itemId: string; price: number; quantity: number };
}> = ({ item }) => {
  const {
    data: destination,
    isLoading,
    isError,
  } = useDestinationById(item.itemId);

  return (
    <li className="mt-2">
      <p>
        <strong>Item ID:</strong> {item.itemId}
      </p>
      <p>
        <strong>Price:</strong> {item.price} zł
      </p>
      <p>
        <strong>Quantity:</strong> {item.quantity}
      </p>
      {isLoading ? (
        <p>Loading destination...</p>
      ) : isError || !destination ? (
        <p>Failed to load destination details.</p>
      ) : (
        <>
          <p>
            <strong>Destination Name:</strong> {destination.name}
          </p>
          <p>
            <strong>Destination Price:</strong> {destination.price.toFixed(2)}{" "}
            zł
          </p>
        </>
      )}
    </li>
  );
};

export default ReservationDetails;
