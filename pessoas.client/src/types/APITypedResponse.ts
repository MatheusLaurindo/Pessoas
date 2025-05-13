export type APITypedResponse<T> = {
  sucess: boolean;
  data: T;
  mensagem: string;
};
