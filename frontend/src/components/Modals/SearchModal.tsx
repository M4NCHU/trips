import React, { FC, useEffect, useState } from "react";
import { IoSearch } from "react-icons/io5";
import { Link } from "react-router-dom";
import { debounce } from "lodash";
import { useSearchDestinations } from "src/api/Destinations";
import { Input } from "../ui/input";
import { Button } from "src/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogDescription,
  DialogFooter,
  DialogTrigger,
  DialogClose,
} from "src/components/ui/dialog";
import { ButtonWithIcon } from "../ui/Buttons/ButtonWithIcon";

interface SearchModalProps {}

const SearchModal: FC<SearchModalProps> = () => {
  const [inputValue, setInputValue] = useState("");
  const [searchTerm, setSearchTerm] = useState("");

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
    <Dialog>
      <DialogTrigger asChild>
        <div className="px-0 md:px-2 flex items-center rounded-full h-full cursor-pointer gap-2 grow max-w-[24rem]">
          <input
            type="text"
            className="hidden md:block rounded-full text-base placeholder:text-sm w-full h-full px-[2rem] bg-secondary"
            placeholder="search for something ..."
          />
        </div>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>Search for something:</DialogTitle>
          <DialogDescription>
            Start typing to search for destinations. Click on a destination to
            navigate.
          </DialogDescription>
        </DialogHeader>
        <div className="py-4">
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
              className="block"
            >
              {destination.name}
            </Link>
          ))}
        </div>
        <DialogFooter>
          <DialogClose asChild>
            <Button type="button" variant="secondary">
              Close
            </Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default SearchModal;
