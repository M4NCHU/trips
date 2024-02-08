import React from "react";
import { useModal } from "../../../context/ModalContext";

interface ModalProps {
  children: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({ children }) => {
  const { isOpen, closeModal } = useModal();

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 z-40 flex justify-center items-center">
      <div className="bg-white p-5 rounded-lg shadow-lg relative">
        <button className="absolute top-3 right-3 text-lg" onClick={closeModal}>
          &times;
        </button>
        {children}
      </div>
    </div>
  );
};
