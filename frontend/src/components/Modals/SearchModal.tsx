import { debounce } from "lodash";
import React, { FC, useEffect, useState } from "react";
import { IoSearch } from "react-icons/io5";
import { Link } from "react-router-dom";
import { useSearchDestinations } from "src/api/Destinations";
import { useModal } from "src/context/ModalContext";
import { Modal } from "../ui/Modal/Modal";
import { ModalBody } from "../ui/Modal/ModalBody";
import { ModalFooter } from "../ui/Modal/ModalFooter";
import { Input } from "../ui/input";

interface SearchModalProps {}

const SearchModal: FC<SearchModalProps> = ({}) => {
  const [inputValue, setInputValue] = useState("");
  const [searchTerm, setSearchTerm] = useState("");
  const { isOpen, openModal, closeModal } = useModal();

  const {
    data: destinations,
    isError,
    isLoading,
  } = useSearchDestinations(inputValue.length >= 3 ? searchTerm : "");

  useEffect(() => {
    const debouncedSearch = debounce(() => {
      if (inputValue.length >= 3) {
        setSearchTerm(inputValue);
      }
    }, 1000);
    debouncedSearch();

    return () => {
      debouncedSearch.cancel();
    };
  }, [inputValue]);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInputValue(event.target.value);
  };

  return (
    <>
      <IoSearch onClick={openModal} className="cursor-pointer" />

      <Modal>
        <ModalBody>
          <h1 className="font-semibold">Search for something:</h1>
          <Input
            placeholder="Search..."
            value={inputValue}
            onChange={handleSearchChange}
          />
          {isLoading && <p>Loading...</p>}
          {isError && <p>Error fetching destinations.</p>}
          {destinations?.map((destination) => (
            <Link
              to={`/destination/${destination.id}`}
              key={destination.id}
              onClick={closeModal}
            >
              {destination.name}
            </Link>
          ))}
        </ModalBody>
        <ModalFooter>
          <button
            className="px-4 py-2 text-white bg-blue-500 rounded hover:bg-blue-700"
            onClick={closeModal}
          >
            Close
          </button>
        </ModalFooter>
      </Modal>
    </>
  );
};

export default SearchModal;
