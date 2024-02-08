import React, { ReactNode } from "react";

interface ModalContentProps {
  children: ReactNode;
}

export const ModalContent: React.FC<ModalContentProps> = ({ children }) => {
  return <div className="space-y-4">{children}</div>;
};
