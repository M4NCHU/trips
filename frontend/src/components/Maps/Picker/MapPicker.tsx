import { FC, useState } from "react";
import { MapContainer, TileLayer, Marker, useMapEvents } from "react-leaflet";
import { LatLngTuple, Icon } from "leaflet";
import { FaMapMarkerAlt } from "react-icons/fa";
import { renderToString } from "react-dom/server";
import { Button } from "../../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../ui/dialog";

interface MapPickerProps {
  onPointSelected: (point: LatLngTuple) => void;
}

const MapPicker: FC<MapPickerProps> = ({ onPointSelected }) => {
  const [markerPosition, setMarkerPosition] = useState<LatLngTuple | null>(
    null
  );

  const MapClickHandler = () => {
    useMapEvents({
      click(e) {
        const { lat, lng } = e.latlng;
        const position: LatLngTuple = [lat, lng];
        setMarkerPosition(position);
        onPointSelected(position);
      },
    });

    return null;
  };

  const customIcon = new Icon({
    iconUrl: `data:image/svg+xml;base64,${btoa(
      renderToString(<FaMapMarkerAlt size={32} color="red" />)
    )}`,
    iconSize: [32, 32],
    iconAnchor: [16, 32],
  });

  return (
    <MapContainer
      center={[51.505, -0.09]}
      zoom={13}
      style={{ height: "400px", width: "100%" }}
      className="rounded-lg"
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      />
      <MapClickHandler />
      {markerPosition && <Marker position={markerPosition} icon={customIcon} />}
    </MapContainer>
  );
};

const MapModal: FC<{ onConfirm: (point: LatLngTuple) => void }> = ({
  onConfirm,
}) => {
  const [selectedPoint, setSelectedPoint] = useState<LatLngTuple | null>(null);
  const [isOpen, setIsOpen] = useState(false);

  const handlePointSelected = (point: LatLngTuple) => {
    setSelectedPoint(point);
  };

  const handleConfirm = () => {
    if (selectedPoint) {
      onConfirm(selectedPoint);
      setIsOpen(false);
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={setIsOpen}>
      <DialogTrigger asChild>
        <Button
          className="bg-blue-500 text-foreground"
          onClick={() => setIsOpen(true)} // Otwórz modal
        >
          Pick a location
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Select Location</DialogTitle>
          <DialogDescription>
            Click on the map to select a location.
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col gap-4">
          <MapPicker onPointSelected={handlePointSelected} />
          {selectedPoint && (
            <div>
              Selected Point: Latitude: {selectedPoint[0]}, Longitude:{" "}
              {selectedPoint[1]}
            </div>
          )}
        </div>
        <DialogFooter>
          <DialogClose asChild>
            <Button variant="secondary">Close</Button>
          </DialogClose>
          <Button onClick={handleConfirm} disabled={!selectedPoint}>
            Confirm
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default MapModal;
