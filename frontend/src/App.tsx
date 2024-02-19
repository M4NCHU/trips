import { BrowserRouter } from "react-router-dom";
import "./App.css";
import Router from "./pages/router";

function App() {
  return (
    <>
      <BrowserRouter>
        <div className="min-h-screen">
          <Router />
        </div>
      </BrowserRouter>
    </>
  );
}

export default App;
