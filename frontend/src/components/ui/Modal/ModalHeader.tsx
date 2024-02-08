import React, { ReactNode } from "react";

interface ModalHeaderProps {
  children: ReactNode;
}

export const ModalHeader: React.FC<ModalHeaderProps> = ({ children }) => {
  return (
    <div className="px-4 py-2 border-b border-gray-200">
      <h3 className="text-lg font-semibold text-gray-900">{children}</h3>
    </div>
  );
};
