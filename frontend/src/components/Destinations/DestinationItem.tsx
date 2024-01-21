import { FC } from "react";
import { Link } from "react-router-dom";
import { Destination } from "../../types/Destination";

interface DestinationItemProps {
  data: Destination;
}

const DestinationItem: FC<DestinationItemProps> = ({ data }) => {
  return (
    <Link to={`/destination/${data.id}`}>
      <div className="category-item flex flex-col gap-4 p-2 cursor-pointer  border-b-2 border-transparent hover:border-secondary">
        <div className="category-item__img  flex items-center justify-center relative">
          <img
            src={data.photoUrl}
            alt={`${data.name}` + ` image`}
            className="w-full md:w-72 h-72 object-cover rounded-xl"
          />
        </div>
        <div className="destination-description flex flex-col">
          <h2 className="font-bold">{data.name}</h2>
          {/* <p className="">{data.description}</p> */}
          <p className="">{data.location}</p>
          <p className="">
            <span className="font-bold">529 zł</span> osoba
          </p>
        </div>
      </div>
    </Link>
  );
};

export default DestinationItem;
