import { FC } from "react";
import NoDataImg from "../../../assets/images/nodata.png";

interface NoDataProps {}

const NoData: FC<NoDataProps> = ({}) => {
  return (
    <div className="flex flex-col justify-center items-center min-h-[80vh]">
      <button className="bg-green-500 px-6 py-4 rounded-lg font-semibold">
        Create new Scheme +
      </button>
      <div className="w-full h-full md:h-[24rem] md:w-[24rem]">
        <img src={NoDataImg} alt="" />
      </div>
    </div>
  );
};

export default NoData;
