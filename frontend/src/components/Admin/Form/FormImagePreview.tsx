import React, { FC, ReactElement } from "react";

interface FormImagePreviewProps {
  imagePreview: string;
  isLoading: boolean;
}

const FormImagePreview: FC<FormImagePreviewProps> = ({
  imagePreview,
  isLoading,
}): ReactElement | null => {
  if (isLoading) {
    return (
      <div className="flex justify-center items-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (!imagePreview) {
    return null;
  }

  return (
    <div className="img-preview">
      <p>Image Preview</p>
      <img src={imagePreview} alt="Preview" className="max-w-full max-h-96" />
    </div>
  );
};

export default FormImagePreview;
