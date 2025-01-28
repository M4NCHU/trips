import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { LatLngTuple, icon, Map as LeafletMap, DivIcon } from "leaflet";
import { useEffect, useRef } from "react";
import CornerLogger from "./CornerLogger";

interface MapComponentProps {
  markers: {
    position: LatLngTuple;
    label: string;
    photoUrl: string | null;
    description: string | null;
  }[];
  onBoundsChange: (bounds: {
    northEastLat: number;
    northEastLng: number;
    southWestLat: number;
    southWestLng: number;
  }) => void;
}

const MapComponent: React.FC<MapComponentProps> = ({
  markers,
  onBoundsChange,
}) => {
  const center: LatLngTuple = [51.505, -0.09];

  const mapRef = useRef<LeafletMap | null>(null);

  useEffect(() => {
    if (mapRef.current) {
      const bounds = mapRef.current.getBounds();
      const northEast = bounds.getNorthEast();
      const southWest = bounds.getSouthWest();

      onBoundsChange({
        northEastLat: northEast.lat,
        northEastLng: northEast.lng,
        southWestLat: southWest.lat,
        southWestLng: southWest.lng,
      });
    }
  }, []);

  const createCustomIcon = (photoUrl: string | null): DivIcon => {
    const imageSrc = photoUrl || "default-image-url.png";
    return new DivIcon({
      className: "custom-icon",
      html: `<div style="width: 50px; height: 50px; border-radius: 50%; overflow: hidden; border: 2px solid white; box-shadow: 0 0 5px rgba(0, 0, 0, 0.5);">
               <img src="${imageSrc}" alt="marker" style="width: 100%; height: 100%; object-fit: cover;" />
             </div>`,
      iconSize: [50, 50],
      iconAnchor: [25, 25],
    });
  };

  return (
    <MapContainer
      className="rounded-lg"
      center={center}
      zoom={13}
      style={{ height: "100%", width: "100%" }}
      ref={(mapInstance) => {
        if (mapInstance && !mapRef.current) {
          mapRef.current = mapInstance;
        }
      }}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      />

      <CornerLogger onBoundsChange={onBoundsChange} />

      {markers.map((marker, index) => (
        <Marker
          key={index}
          position={marker.position}
          icon={createCustomIcon(marker.photoUrl)}
        >
          <Popup>
            <div className="text-center">
              <h3 className="font-bold">{marker.label}</h3>
              {marker.photoUrl && (
                <img
                  src={marker.photoUrl}
                  alt={marker.label}
                  className="w-full h-auto rounded-md my-2"
                />
              )}
              {marker.description && (
                <p className="text-sm text-gray-700">{marker.description}</p>
              )}
            </div>
          </Popup>
        </Marker>
      ))}
    </MapContainer>
  );
};

export default MapComponent;
