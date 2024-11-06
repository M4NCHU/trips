import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { ThemeProvider } from "./context/ThemeContext";
import "./index.css";
import reportWebVitals from "./reportWebVitals";
import { Toaster } from "react-hot-toast";
import { UserProvider } from "./context/UserContext";
import axios from "axios";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

axios.defaults.baseURL =
  process.env.REACT_APP_API_BASE_URL || "http://localhost:5000";

axios.defaults.withCredentials = true;

const queryClient = new QueryClient();
root.render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <UserProvider>
        <ThemeProvider>
          <Toaster />
          <App />
        </ThemeProvider>
      </UserProvider>
    </QueryClientProvider>
  </React.StrictMode>
);

reportWebVitals();
