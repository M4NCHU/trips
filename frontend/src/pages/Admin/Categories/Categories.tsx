import { FC, useState } from "react";
import { useDeleteCategory } from "src/api/Category";
import CreateCategoryModal from "src/components/Categories/Modals/CreateCategoryModal";
import DynamicIcon from "src/components/Icons/DynamicIcon";
import Table from "src/components/ui/Table/Table";
import { Category } from "src/types/Category";
import * as Icons from "react-icons/fa";
import DeleteModal from "src/components/Modals/DeleteModal";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import Pagination from "src/components/Pagination";
import useURLParams from "src/hooks/useURLParams";
import { UseCategoryList } from "src/api/Category";

interface CategoryAdminProps {}
type IconName = keyof typeof Icons;

const CategoryAdmin: FC<CategoryAdminProps> = () => {
  const [selectedCategoryId, setSelectedCategoryId] = useState<string | null>(
    null
  );
  const [isEditMode, setIsEditMode] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  const { mutate: deleteCategory } = useDeleteCategory();

  const navigate = useNavigate();
  const { getParam, addParam } = useURLParams();
  const currentPage = Number(getParam("page")) || 1;
  const itemsPerPage = 2;

  const {
    data: paginatedCategories,
    isLoading,
    totalItems,
    isError: CategoriesError,
    page,
    setPage,
  } = UseCategoryList({}, itemsPerPage);

  console.log(paginatedCategories);

  if (CategoriesError) return <div>Error loading categories...</div>;

  const handleCategoryDelete = (categoryId: string) => {
    setSelectedCategoryId(categoryId);
    setIsDeleteModalOpen(true);
  };

  const handleCategoryEdit = (categoryId: string) => {
    setSelectedCategoryId(categoryId);
    setIsEditMode(true);
    setIsModalOpen(true);
  };

  const handleAddCategory = () => {
    setSelectedCategoryId(null);
    setIsEditMode(false);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  const handleConfirmDelete = async () => {
    if (!selectedCategoryId) return;

    try {
      deleteCategory(
        { id: selectedCategoryId },
        {
          onSuccess: () => {
            toast.success("Category deleted successfully!");
            setIsDeleteModalOpen(false);
            navigate(0);
          },
          onError: (error: any) => {
            console.error("Error deleting category:", error);
            toast.error("Failed to delete category.");
          },
        }
      );
    } catch (error) {
      console.error("Error deleting category:", error);
      toast.error("Failed to delete category.");
    }
  };

  const handleCancelDelete = () => {
    setIsDeleteModalOpen(false);
  };

  return (
    <div className="p-0 md:p-4">
      <h1 className="text-xl font-bold mb-4">Manage Categories</h1>
      {paginatedCategories && paginatedCategories.length > 0 ? (
        <>
          <Table
            data={paginatedCategories}
            createModal={
              <button onClick={handleAddCategory}>Add Category</button>
            }
            tableTitle="Categories list"
            tableDescription="Categories list description"
            columns={["name", "description", "icon"]}
            onEdit={(category) => handleCategoryEdit(category.id)}
            onDelete={(category) => handleCategoryDelete(category.id)}
            renderCell={(item, column) => {
              const cellData = item[column];
              if (column === "icon" && typeof cellData === "string") {
                return (
                  <div className="">
                    <DynamicIcon iconName={cellData as IconName} />
                  </div>
                );
              }
              return cellData as React.ReactNode;
            }}
          />

          <Pagination
            currentPage={currentPage}
            totalPages={Math.ceil(totalItems / itemsPerPage)}
            onPageChange={(newPage) => setPage(newPage)}
          />

          <CreateCategoryModal
            isOpen={isModalOpen}
            categoryId={selectedCategoryId}
            isEditMode={isEditMode}
            onClose={handleCloseModal}
          />

          <DeleteModal
            isOpen={isDeleteModalOpen}
            onClose={handleCancelDelete}
            onConfirm={handleConfirmDelete}
            message="Are you sure you want to delete this category?"
          />
        </>
      ) : (
        <div>No data</div>
      )}
    </div>
  );
};

export default CategoryAdmin;
