import { useEffect, useState, type ReactNode } from "react";
import { Pagination } from "../../../components/Pagination";
import type { GetPessoaResp } from "../../../types/DTOs/response/pessoas";
import type { APIPaginatedResponse } from "../../../types/APITypedResponse";
import { pessoaService } from "../../../service/pessoas.service";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../../../components/ui/table";
import { toast } from "react-toastify";
import { PencilSimple, Trash } from "phosphor-react";
import { CadastroPessoaDialog } from "../formulario";

const columns = [
  { title: "Nome", name: "nome" },
  { title: "Email", name: "email" },
  { title: "Data de Nascimento", name: "dataNascimento" },
  { title: "Sexo", name: "sexo" },
  { title: "Endereço", name: "endereco" },
  { title: "Nacionalidade", name: "nacionalidade" },
  { title: "Naturalidade", name: "naturalidade" },
];

export default function Listagem() {
  const [data, setData] = useState<APIPaginatedResponse<GetPessoaResp>>({
    total: 0,
    dados: [],
  });
  const [page, setPage] = useState(0);
  const [rowsPerPage] = useState(10);

  function handlePaginated(pageIndex: number) {
    setPage(pageIndex);
  }

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await pessoaService.getPessoasPaginado(
          page,
          rowsPerPage
        );
        setData(response);
      } catch (err) {
        toast.error("Erro ao buscar dados");
      }
    }

    fetchData();
  }, [page]);

  function handleEdit(id: string) {
    alert(`Editar item com ID: ${id}`);
  }

  async function handleDelete(id: string) {
    await pessoaService
      .deletePessoa(id)
      .then(() => toast.success("Pessoa deletada com sucesso"))
      .catch(() => {
        toast.error("Erro ao deletar pessoa");
      });
  }

  return (
    <div className="flex flex-col w-full h-full py-4 px-4 md:px-16 gap-6 mt-20">
      <div className="flex justify-between items-center">
        <div className="flex flex-col gap-2">
          <h1 className="text-2xl font-bold text-zinc-800">
            Listagem de Pessoas
          </h1>
          <p className="text-sm text-zinc-600">
            Aqui você pode visualizar e gerenciar as pessoas cadastradas.
          </p>
        </div>
        <div>
          <CadastroPessoaDialog />
        </div>
      </div>
      <Table className="table-auto bg-background border-collapse w-full">
        <TableHeader className="bg-zinc-300">
          <TableRow>
            {columns.map((column, index) => (
              <TableHead
                key={index}
                style={{ width: "auto" }}
                className="text-zinc-800 text-sm p-3"
              >
                {column.title}
              </TableHead>
            ))}
            <TableHead className="text-zinc-800 text-sm p-3">Ações</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.dados?.map((row, index) => {
            const id = row.id;
            return (
              <TableRow key={index} className="border-b border-zinc-200">
                {columns.map((column, i) => (
                  <TableCell
                    key={i}
                    className="text-zinc-800 text-sm p-3"
                    style={{ width: "auto" }}
                  >
                    {row[column.name as keyof GetPessoaResp] as ReactNode}
                  </TableCell>
                ))}
                <TableCell className="text-zinc-800 text-sm p-3 flex gap-3 justify-center items-center">
                  <PencilSimple
                    size={20}
                    className="text-blue-500 cursor-pointer hover:text-blue-700"
                    onClick={() => handleEdit(id)}
                  />
                  <Trash
                    size={20}
                    className="text-red-500 cursor-pointer hover:text-red-700"
                    onClick={() => handleDelete(id)}
                  />
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
      <Pagination
        pageIndex={page}
        totalCount={data.total}
        rowsPerPage={rowsPerPage}
        onPageChange={handlePaginated}
      />
    </div>
  );
}
