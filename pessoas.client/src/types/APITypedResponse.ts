export type APITypedResponse<T> = {
  sucess: boolean;
  data: T;
  mensagem: string;
};

export type APIPaginatedResponse<T> = {
  total: number;
  dados: T[];
};
