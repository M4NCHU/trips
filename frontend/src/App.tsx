import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Contact from "./pages/Contact/Contact";
import Home from "./pages/Home/Home";
import Layout from "./pages/Layout";
import Create from "./pages/Destinations/Create";
import Login from "./pages/Authentication/Login/Login";
import Register from "./pages/Authentication/Register/Register";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="contact" element={<Contact />} />
          <Route path="/destinations/create" element={<Create />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
