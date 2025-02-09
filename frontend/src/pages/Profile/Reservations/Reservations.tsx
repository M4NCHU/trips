import { FC, useState } from "react";
import { useQuery } from "@tanstack/react-query";

import useFilter from "src/hooks/UseFilter";
import { Button } from "src/components/ui/button";
import Table from "src/components/ui/Table/Table";
import FilterModal from "src/components/Modals/FilterModal";
import ProfileNav from "src/components/Profile/ProfileNav/ProfileNav";
import { ReservationDTO } from "src/types/Resume/ReservationDTO";
import { useGetUserReservations } from "src/api/ResumeAPI";
import { Navigate, useNavigate } from "react-router-dom";

interface ReservationsProps {}

const Reservations: FC<ReservationsProps> = () => {
  const { filters, updateFilter, resetFilters } = useFilter();

  const columns: (keyof ReservationDTO | "status")[] = [
    "id",
    "totalPrice",
    "createdAt",
    "status",
  ];

  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
  const [minPrice, setMinPrice] = useState<number | null>(null);
  const [maxPrice, setMaxPrice] = useState<number | null>(null);

  const applyFilters = () => {
    updateFilter("categoryId", selectedCategory ?? undefined);
    updateFilter("minPrice", minPrice ?? undefined);
    updateFilter("maxPrice", maxPrice ?? undefined);
  };

  const navigate = useNavigate();

  const { data: reservations, isLoading, isError } = useGetUserReservations();
  const filtersList = [
    {
      name: "Category",
      type: "select",
      value: selectedCategory ?? "",
      setValue: setSelectedCategory,
      options: ["Category 1", "Category 2"],
    },
    {
      name: "Min Price",
      type: "number",
      value: minPrice ?? "",
      setValue: setMinPrice,
      minValue: 0,
      maxValue: maxPrice ?? undefined,
    },
    {
      name: "Max Price",
      type: "number",
      value: maxPrice ?? "",
      setValue: setMaxPrice,
      minValue: minPrice ?? undefined,
    },
  ];
  return (
    <>
      <ProfileNav />
      <div className="p-4">
        <h1 className="text-xl font-bold mb-4">Manage Reservations</h1>

        <div className="p-0 md:p-4">
          <h1 className="text-xl font-bold mb-4">Reservations List</h1>

          {isLoading ? (
            <p>Loading reservations...</p>
          ) : isError ? (
            <p>Failed to load reservations.</p>
          ) : (
            <Table
              data={reservations || []}
              tableTitle="Reservations list"
              tableDescription="Manage your reservations here."
              columns={columns}
              renderCell={(item, column) => {
                if (column === "status") {
                  return typeof item.status === "number"
                    ? item.status === 1
                      ? "Pending"
                      : "Completed"
                    : "Unknown";
                }

                if (column === "totalPrice") {
                  return typeof item.totalPrice === "number"
                    ? `${item.totalPrice.toFixed(2)} zł`
                    : "N/A";
                }

                if (column === "createdAt") {
                  return item.createdAt
                    ? new Date(item.createdAt).toLocaleString()
                    : "N/A";
                }

                if (column === "reservationItems") {
                  return item.reservationItems &&
                    item.reservationItems.length > 0 ? (
                    <ul>
                      {item.reservationItems.map((reservationItem, index) => (
                        <li key={index}>
                          {reservationItem.itemId} - {reservationItem.price} zł
                          x {reservationItem.quantity}
                        </li>
                      ))}
                    </ul>
                  ) : (
                    "No Items"
                  );
                }

                return (
                  item[column as keyof ReservationDTO]?.toString() ?? "N/A"
                );
              }}
              renderActions={(item) => (
                <div className="flex gap-2">
                  <Button
                    onClick={() => navigate(`/profile/reservations/${item.id}`)}
                    className="bg-blue-500 text-white px-2 py-1 rounded"
                  >
                    View Details
                  </Button>
                </div>
              )}
            />
          )}
        </div>
      </div>
    </>
  );
};

export default Reservations;
