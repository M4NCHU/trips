import { FC } from "react";
import { IoSearch } from "react-icons/io5";
import { Link } from "react-router-dom";
import { Button } from "src/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "src/components/ui/dialog";
import { ButtonWithIcon } from "../../ui/Buttons/ButtonWithIcon";
import { Input } from "../../ui/input";
import { UseCategoryList } from "src/api/Category";

interface CategoriesListModalProps {
  action: (categoryId: string) => void;
  categoryId: string;
}

const CategoriesListModal: FC<CategoriesListModalProps> = ({
  action,
  categoryId,
}) => {
  const { data: categories } = UseCategoryList();
  return (
    <Dialog>
      <DialogTrigger asChild>
        <div
          className="p-3 bg-background md:px-2 flex items-center rounded-lg
         cursor-pointer gap-2"
        >
          Choose category
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

        <div className="flex flex-row gap-4 mt-8">
          {categories
            ? categories.map((category, i) => (
                <div
                  key={category.id}
                  className={`flex flex-col w-1/5 h-14 items-center justify-center relative gap-1 ${
                    categoryId === category.id ? "border-2 border-blue-500" : ""
                  }`}
                  onClick={() => action(category.id)}
                >
                  <img
                    src={category.photoUrl}
                    alt={`${category.name}`}
                    className="object-cover"
                  />
                  <div>{category.name}</div>
                </div>
              ))
            : "There is no categories"}
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

export default CategoriesListModal;
