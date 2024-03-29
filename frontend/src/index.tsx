import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { ThemeProvider } from "./context/ThemeContext";
import "./index.css";
import reportWebVitals from "./reportWebVitals";
import { Toaster } from "react-hot-toast";
import { UserProvider } from "./context/UserContext";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

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
