import { FC, useState } from "react";
import { Button } from "../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../ui/dialog";
import { Input } from "../ui/input";

interface CreateTripModalProps {
  userId: string;
  onTripCreated: (newTripId: string) => void;
  onSubmit: (e: React.FormEvent) => Promise<void>;
  message: string;
}

const CreateTripModal: FC<CreateTripModalProps> = ({
  userId,
  onTripCreated,
  onSubmit,
  message,
}) => {
  const [title, setTitle] = useState("");

  const handleFormSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    onTripCreated(title);
    await onSubmit(e);
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <button
          className="p-2 w-full bg-red-500 rounded-lg
        "
        >
          open
        </button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <DialogHeader>
          <DialogTitle>New trip plan scheme</DialogTitle>
          <DialogDescription>{message}</DialogDescription>
        </DialogHeader>
        <div className="py-4">
          <Input
            placeholder="Enter name of new trip scheme"
            name="title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
        </div>
        <DialogFooter>
          <DialogClose asChild>
            <Button
              type="button"
              variant="secondary"
              //   onClick={(e) => handleFormSubmit(e)}
            >
              Close
            </Button>
          </DialogClose>

          <Button
            type="button"
            variant="secondary"
            className="bg-green-500"
            onClick={(e) => {
              handleFormSubmit(e);
            }}
          >
            Create
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default CreateTripModal;
