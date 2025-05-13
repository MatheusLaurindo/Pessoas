import { createBrowserRouter } from "react-router-dom";
import Layout from "../Layout";
import { login } from "./login";
import { pessoas } from "./pessoas";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    children: [...login, ...pessoas],
  },
]);
