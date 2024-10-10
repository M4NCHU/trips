import { FC, useState } from "react";
import { Button } from "../../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../ui/dialog";
import { Input } from "../../ui/input";
import CreateDestinationForm from "../Forms/CreateDestinationForm";
import { IoCreate } from "react-icons/io5";

interface CreateTripModalProps {}

const CreateDestinationModal: FC<CreateTripModalProps> = () => {
  const [title, setTitle] = useState("");

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button className="bg-purple-500 text-foreground">
          <IoCreate />
          <span className="hidden md:block">Add destination</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="">
        <DialogHeader>
          <DialogTitle>Create Destination</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col">
          <CreateDestinationForm />
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
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default CreateDestinationModal;
