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
import { IoCreate } from "react-icons/io5";
import CreateCategoryForm from "../Forms/CreateCategoryForm";

interface CreateCategoryModalProps {}

const CreateCategoryModal: FC<CreateCategoryModalProps> = () => {
  const [title, setTitle] = useState("");

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button className="bg-purple-500 text-foreground">
          <IoCreate />
          <span className="hidden md:block">Add category</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="">
        <DialogHeader>
          <DialogTitle>Create Destination</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col">
          <CreateCategoryForm />
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

export default CreateCategoryModal;
