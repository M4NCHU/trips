import { FC } from "react";
import { UseCategoryList } from "src/api/Category";
import CreateCategoryModal from "src/components/Categories/Modals/CreateCategoryModal";
import Table from "src/components/ui/Table/Table";
import { Category } from "src/types/Category";

interface CategoryAdminProps {}

const CategoryAdmin: FC<CategoryAdminProps> = () => {
  const columns: (keyof Category)[] = ["name", "description"];
  const {
    data: categories,
    isLoading,
    isError: CategoriesError,
  } = UseCategoryList();

  if (CategoriesError) return <div>Error loading categories...</div>;

  return (
    <div className="p-0 md:p-4">
      <h1 className="text-xl font-bold mb-4">Manage Categories</h1>
      {categories ? (
        <Table
          data={categories}
          createModal={<CreateCategoryModal />}
          tableTitle="Categories list"
          tableDescription="Categories list description"
          columns={columns}
          renderCell={(item, column) => {
            const cellData = item[column];

            if (Array.isArray(cellData)) {
              return (
                <ul>
                  {cellData.map((place, index) => (
                    <li key={index}>{place.name}</li>
                  ))}
                </ul>
              );
            }
            return cellData as React.ReactNode;
          }}
        />
      ) : (
        <div>No data</div>
      )}
    </div>
  );
};

export default CategoryAdmin;
