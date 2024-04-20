import { useState } from "react";

interface ImagePreviewState {
  imageSrc: string;
  imageFile: File | null;
  isLoading: boolean;
}

const useImagePreview = () => {
  const [imagePreview, setImagePreview] = useState<ImagePreviewState>({
    imageSrc: "",
    imageFile: null,
    isLoading: false,
  });

  const showPreview = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setImagePreview((prevState) => ({ ...prevState, isLoading: true })); // Rozpoczęcie ładowania
      const file = e.target.files[0];
      const reader = new FileReader();

      reader.onload = (x: ProgressEvent<FileReader>) => {
        if (x.target && typeof x.target.result === "string") {
          setImagePreview({
            imageFile: file,
            imageSrc: x.target.result,
            isLoading: false,
          });
        } else {
          setImagePreview({
            imageFile: null,
            imageSrc: "",
            isLoading: false,
          });
        }
      };

      reader.readAsDataURL(file);
    } else {
      setImagePreview({
        imageFile: null,
        imageSrc: "",
        isLoading: false,
      });
    }
  };

  return { showPreview, imagePreview };
};

export default useImagePreview;
