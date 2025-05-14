import api from "../lib/axios";
import type { APIPaginatedResponse, APITypedResponse } from "../types/APITypedResponse";
import type { AdicionarPessoaRequest, EditarPessoaRequest } from "../types/DTOs/request/pessoa";
import type { GetPessoaResp } from "../types/DTOs/response/pessoas";

const baseUrl = "/api/v1/pessoa";

export const pessoaService = {
  async getPessoaById(id?: string): Promise<APITypedResponse<GetPessoaResp>> {
    return await api.get(baseUrl + `/${id}`)
    .then((resp) => resp.data);
  },
  
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

  async postPessoa(payload: AdicionarPessoaRequest): Promise<GetPessoaResp> {
    return await api.post(baseUrl, payload).then((resp) => resp.data);
  },

  async putPessoa(payload: EditarPessoaRequest): Promise<GetPessoaResp> {
    return await api.put(baseUrl, payload).then((resp) => resp.data);
  },

  async deletePessoa(id: string): Promise<GetPessoaResp> {
    return await api.delete(baseUrl + `/${id}`).then((resp) => resp.data);
  },
};
