export type AdicionarPessoaRequest = {
    nome: string;
    cpf: string;
    dataNascimento: string;
    email?: string;
    endereco?: string;
    naturalidade?: string;
    nacionalidade?: number;
    sexo?: number;
}