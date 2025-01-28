import { useEffect, useRef } from "react";
import { useMapEvents } from "react-leaflet";

interface CornerLoggerProps {
  onBoundsChange: (bounds: {
    northEastLat: number;
    northEastLng: number;
    southWestLat: number;
    southWestLng: number;
  }) => void;
}

const CornerLogger: React.FC<CornerLoggerProps> = ({ onBoundsChange }) => {
  const debounceTimeout = useRef<NodeJS.Timeout | null>(null);

  const map = useMapEvents({
    moveend: () => {
      const bounds = map.getBounds();
      const northEast = bounds.getNorthEast();
      const southWest = bounds.getSouthWest();

      if (debounceTimeout.current) {
        clearTimeout(debounceTimeout.current);
      }
      debounceTimeout.current = setTimeout(() => {
        onBoundsChange({
          northEastLat: northEast.lat,
          northEastLng: northEast.lng,
          southWestLat: southWest.lat,
          southWestLng: southWest.lng,
        });
      }, 500);
    },
  });

  return null;
};

export default CornerLogger;
