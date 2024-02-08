import React, { ReactNode } from "react";

interface ModalBodyProps {
  children: ReactNode;
}

export const ModalBody: React.FC<ModalBodyProps> = ({ children }) => {
  return <div className="px-4 py-2">{children}</div>;
};
