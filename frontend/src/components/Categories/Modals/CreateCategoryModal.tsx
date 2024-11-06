import { FC, useEffect, useState } from "react";
import { Button } from "../../ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../../ui/dialog";
import CreateCategoryForm from "../Forms/CreateCategoryForm";
import { useGetCategoryById } from "../../../api/Category";

interface CreateCategoryModalProps {
  categoryId?: string | null;
  isEditMode?: boolean;
  isOpen: boolean;
  onClose: () => void;
}

const CreateCategoryModal: FC<CreateCategoryModalProps> = ({
  categoryId,
  isEditMode = false,
  isOpen,
  onClose,
}) => {
  const [title, setTitle] = useState(
    isEditMode ? "Edit Category" : "Create Category"
  );
  const { data: categoryData, isLoading } = useGetCategoryById(
    categoryId || ""
  );

  useEffect(() => {
    if (isEditMode && categoryData) {
      setTitle("Edit Category");
    } else {
      setTitle("Create Category");
    }
  }, [isEditMode, categoryData]);

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{title}</DialogTitle>
        </DialogHeader>
        <div className="flex flex-col">
          {isLoading ? (
            <p>Loading...</p>
          ) : (
            <CreateCategoryForm
              category={categoryData}
              isEditMode={isEditMode}
            />
          )}
        </div>

        <DialogFooter>
          <DialogClose asChild>
            <Button type="button" variant="secondary" onClick={onClose}>
              Close
            </Button>
          </DialogClose>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default CreateCategoryModal;
