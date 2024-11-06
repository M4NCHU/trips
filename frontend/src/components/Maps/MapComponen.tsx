import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import { LatLngTuple, icon } from "leaflet";
import img from "../../assets/images/nodata.png";

interface MapComponentProps {
  markers: { position: LatLngTuple; label: string }[];
}

const MapComponent: React.FC<MapComponentProps> = ({ markers }) => {
  const center: LatLngTuple = [51.505, -0.09];

  const customIcon = icon({
    iconUrl: img,
    iconSize: [64, 64],
    iconAnchor: [16, 32],
  });

  return (
    <MapContainer
      className="rounded-lg"
      center={center}
      zoom={13}
      style={{ height: "500px", width: "100%" }}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      />
      {markers.map((marker, index) => (
        <Marker key={index} position={marker.position} icon={customIcon}>
          <Popup>{marker.label}</Popup>
        </Marker>
      ))}
    </MapContainer>
  );
};

export default MapComponent;
