import { createBrowserRouter } from "react-router-dom";
import Layout from "../Layout";
import { login } from "./login";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [...login],
  },
]);
