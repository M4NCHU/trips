import { useState } from "react";

const useImagePreview = () => {
  const [imagePreview, setImagePreview] = useState({
    imageSrc: "",
    imageFile: null,
  });

  const showPreview = (e: any) => {
    if (e.target.files && e.target.files[0]) {
      const file = e.target.files[0];
      const reader = new FileReader();

      reader.onload = (x) => {
        if (x.target && typeof x.target.result === "string") {
          setImagePreview({
            imageFile: file,
            imageSrc: x.target.result,
          });
        }
      };

      reader.readAsDataURL(file);
    } else {
      setImagePreview({
        imageFile: null,
        imageSrc: "",
      });
    }
  };

  return { showPreview, imagePreview };
};

export default useImagePreview;
