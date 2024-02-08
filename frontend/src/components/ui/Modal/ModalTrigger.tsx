import React, { ReactNode } from "react";
import { useModal } from "../../../context/ModalContext";

interface ModalTriggerProps {
  children: ReactNode;
}

export const ModalTrigger: React.FC<ModalTriggerProps> = ({ children }) => {
  const { openModal } = useModal();

  return <div onClick={openModal}>{children}</div>;
};
