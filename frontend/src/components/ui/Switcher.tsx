import { FiMoon } from "react-icons/fi";
import { MdOutlineWbSunny } from "react-icons/md";
import { useTheme } from "../../../src/context/ThemeContext";

export default function Switcher() {
  const { isDarkMode, toggleTheme } = useTheme();

  return (
    <button className="cursor-pointer p-1" onClick={toggleTheme}>
      {isDarkMode ? <MdOutlineWbSunny /> : <FiMoon />}
    </button>
  );
}
