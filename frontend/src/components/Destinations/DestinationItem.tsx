import { FC } from "react";
import Logo from "../../assets/images/test.webp";

interface DestinationItemProps {}

const DestinationItem: FC<DestinationItemProps> = ({}) => {
  return (
    <div className="category-item flex flex-col gap-4 p-4 cursor-pointer  border-b-2 border-transparent hover:border-black">
      <div className="category-item__img  flex items-center justify-center relative">
        <img
          src={Logo}
          alt=""
          className="w-full md:w-72 h-72 object-cover rounded-xl"
        />
      </div>
      <div className="destination-description flex flex-col">
        <h2 className="font-bold">Miejsce</h2>
        <p className="">Krótki opis</p>
        <p className="">data</p>
        <p className="">
          <span className="font-bold">529 zł</span> osoba
        </p>
      </div>
    </div>
  );
};

export default DestinationItem;
