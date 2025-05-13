import api from "../lib/axios";
import type { APIPaginatedResponse } from "../types/APITypedResponse";
import type { GetPessoaResp } from "../types/DTOs/response/pessoas";

const baseUrl = "/api/v1/pessoa";

export const pessoaService = {
  async getPessoasPaginado(
    pagina: number,
    linhasPorPagina: number
  ): Promise<APIPaginatedResponse<GetPessoaResp>> {
    return await api
      .get(
        baseUrl +
          `/paginado?pagina=${pagina}&linhasPorPagina=${linhasPorPagina}`
      )
      .then((resp) => ({
        total: resp.data.total,
        dados: resp.data.data,
      }));
  },
};
