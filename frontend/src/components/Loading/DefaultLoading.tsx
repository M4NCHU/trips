import { FC } from "react";

interface DefaultLoadingProps {
  // Props here if needed, such as size or color
}

const DefaultLoading: FC<DefaultLoadingProps> = () => {
  return (
    <div className="flex justify-center items-center min-h-[90vh]">
      <div className="animate-spin rounded-full h-32 w-32 border-t-4 border-b-4 border-blue-500"></div>
    </div>
  );
};

export default DefaultLoading;
