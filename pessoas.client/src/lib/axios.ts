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
      toast.error("Token expirado ou inválido! Realize o login novamente.");
      setInterval(() => {
        window.location.href = "/login";
      }, 2000);
      return Promise.reject(error);
    }
    if (error.response?.status === 403) {
      window.location.href = "/forbidden";
      toast.error(
        "Acesso negado! Você não tem permissão para acessar esta página."
      );
      return Promise.reject(error);
    }
    toast.error(
      error.response?.data?.mensagem || "Ocorreu um erro inesperado."
    );

    return Promise.reject(error);
  }
);

export default api;
