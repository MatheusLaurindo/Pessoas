import type { LoginRequest } from "../types/DTOs/request/login";
import type { APITypedResponse } from "../types/APITypedResponse.ts";
import api from "../lib/axios.ts";

const baseUrl = '/api/v1/auth';

export const loginService = {
    async login(payload: LoginRequest): Promise<APITypedResponse<any>> {
        return await api.post(`${baseUrl}`, payload, {
            withCredentials: true,
        }).then(resp => resp.data)
    }
}