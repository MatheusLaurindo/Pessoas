import axios from "axios";
import { toast } from "react-toastify";

const api = axios.create({
  baseURL: "https://desafio-pessoas-fjbmggemepfwdefd.brazilsouth-01.azurewebsites.net",
  withCredentials: true,
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      window.location.href = "/login";
    }

    toast.error("Token expirado ou inválido! Realize o login novamente.");

    return Promise.reject(error);
  }
);

export default api;
