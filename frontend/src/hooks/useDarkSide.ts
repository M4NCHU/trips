import { useEffect } from "react";
import { useLocalStorage, useMediaQuery, useUpdateEffect } from "usehooks-ts";

const COLOR_SCHEME_QUERY = "(prefers-color-scheme: dark)";

interface UseDarkModeOutput {
  isDarkMode: boolean;
  toggle: () => void;
  enable: () => void;
  disable: () => void;
}

export function useDarkMode(defaultValue?: boolean): UseDarkModeOutput {
  const isDarkOS = useMediaQuery(COLOR_SCHEME_QUERY);
  const [isDarkMode, setDarkMode] = useLocalStorage<boolean>(
    "usehooks-ts-dark-mode",
    defaultValue ?? isDarkOS ?? false
  );

  useUpdateEffect(() => {
    setDarkMode(isDarkOS);
  }, [isDarkOS]);

  useEffect(() => {
    if (isDarkMode) {
      document.documentElement.classList.add("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
  }, [isDarkMode]);

  const toggle = () => setDarkMode((prev) => !prev);
  const enable = () => setDarkMode(true);
  const disable = () => setDarkMode(false);

  return {
    isDarkMode,
    toggle,
    enable,
    disable,
  };
}
