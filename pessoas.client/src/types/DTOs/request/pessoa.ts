export type AdicionarPessoaRequest = {
  nome: string;
  cpf: string;
  dataNascimento: string;
  email?: string;
  endereco?: string;
  naturalidade?: string;
  nacionalidade?: number;
  sexo?: number;
};

export type EditarPessoaRequest = {
  id: string | null;
  nome: string;
  cpf: string;
  dataNascimento: string;
  email: string | null;
  endereco: string | null;
  naturalidade: string | null;
  nacionalidade?: number;
  sexo?: number;
};
