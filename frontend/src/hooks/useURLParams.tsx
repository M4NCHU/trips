import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";

function useURLParams() {
  const navigate = useNavigate();
  const location = useLocation();
  const [params, setParams] = useState(new URLSearchParams(location.search));

  useEffect(() => {
    setParams(new URLSearchParams(location.search));
  }, [location.search]);

  const addParam = (key: string, value: string | number | boolean) => {
    if (typeof key !== "string" || key === "") return;

    const newParams = new URLSearchParams(params);
    newParams.set(key, String(value));

    navigate({
      pathname: location.pathname,
      search: newParams.toString(),
    });
  };

  const removeParam = (key: string) => {
    if (typeof key !== "string" || key === "") return;

    const newParams = new URLSearchParams(params);
    newParams.delete(key);

    navigate({
      pathname: location.pathname,
      search: newParams.toString(),
    });
  };

  const getParam = (key: string): string | null => {
    return params.get(key);
  };

  const clearAllParams = () => {
    navigate({
      pathname: location.pathname,
      search: "",
    });
  };

  return { addParam, removeParam, getParam, clearAllParams, params };
}

export default useURLParams;
