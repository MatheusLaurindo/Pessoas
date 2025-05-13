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

const columns = [
  { title: "Nome", name: "nome" },
  { title: "Email", name: "email" },
  { title: "Data de Nascimento", name: "dataNascimento" },
  { title: "Sexo", name: "sexo" },
  { title: "Endereço", name: "endereco" },
  { title: "Nacionalidade", name: "nacionalidade" },
  { title: "Naturalidade", name: "naturalidade" },
  { title: "Ações", name: "acoes" },
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
        console.log("📦 Dados recebidos:", response);
        setData(response);
      } catch (err) {
        toast.error("Erro ao buscar dados");
      }
    }

    fetchData();
  }, [page]);

  return (
    <div className="flex flex-col w-full h-full py-4 px-16 gap-6">
      <Table className="table-auto bg-background border-collapse">
        <TableHeader className="bg-zinc-300">
          <TableRow>
            {columns.map((column, index) => (
              <TableHead
                key={index}
                style={{ width: "w-auto" }}
                className="text-zinc-800 text-sm p-3"
              >
                {column.title}
              </TableHead>
            ))}
          </TableRow>
        </TableHeader>
        <TableBody>
          {data?.dados?.map((row, index) => {
            return (
              <TableRow key={index} className="border-b border-zinc-200">
                {columns.map((column, index) => (
                  <TableCell
                    key={index}
                    className="text-zinc-800 text-sm p-3"
                    style={{ width: "w-auto" }}
                  >
                    {row[column.name as keyof GetPessoaResp] as ReactNode}
                  </TableCell>
                ))}
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
