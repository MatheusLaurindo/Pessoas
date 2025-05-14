import { useState, type ReactNode } from "react";
import { Pagination } from "../../../components/Pagination";
import type { GetPessoaResp } from "../../../types/DTOs/response/pessoas";
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
import { useQuery } from "@tanstack/react-query";
import { queryClient } from "../../../main";
import { AlertDialog } from "@radix-ui/react-alert-dialog";
import {
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "../../../components/ui/alert-dialog";
import { Formulario } from "../formulario";
import { Button } from "../../../components/ui/button";

const columns = [
  { title: "Nome", name: "nome" },
  { title: "Email", name: "email" },
  { title: "Data de Nascimento", name: "dataNascimento" },
  { title: "Sexo", name: "sexo" },
  { title: "Nacionalidade", name: "nacionalidade" },
  { title: "Naturalidade", name: "naturalidade" },
];

export default function Listagem() {
  const [page, setPage] = useState(0);
  const [rowsPerPage] = useState(10);

  const [id, setId] = useState("");

  const [openCadastro, setOpenCadastro] = useState(false);
  const [openEdicao, setOpenEdicao] = useState(false);

  const [openModalExcluir, setOpenModalExcluir] = useState(false);

  function handlePaginated(pageIndex: number) {
    setPage(pageIndex);
  }

  const { data } = useQuery({
    queryKey: ["get-pessoas-paginado", page, rowsPerPage],
    queryFn: async () =>
      await pessoaService.getPessoasPaginado(page, rowsPerPage),
  });

  const handleCloseModal = () => {
    setOpenModalExcluir(false);
    setId("");
  };

  async function handleDelete(id: string) {
    await pessoaService
      .deletePessoa(id)
      .then(() => toast.success("Pessoa deletada com sucesso"))
      .catch(() => {
        toast.error("Erro ao deletar pessoa");
      });

    handleCloseModal();

    queryClient.refetchQueries({ queryKey: ["get-pessoas-paginado"] });
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
          <Button
            onClick={() => setOpenCadastro(!openCadastro)}
            className="bg-zinc-800 text-white"
          >
            Adicionar Pessoa
          </Button>
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
                    onClick={() => {
                      setId(id);
                      setOpenEdicao(true);
                    }}
                  />
                  <Trash
                    size={20}
                    className="text-red-500 cursor-pointer hover:text-red-700"
                    onClick={() => {
                      setId(id);
                      setOpenModalExcluir(true);
                    }}
                  />
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
      <Pagination
        pageIndex={page}
        totalCount={data?.total}
        rowsPerPage={rowsPerPage}
        onPageChange={handlePaginated}
      />

      {openCadastro && (
        <Formulario
          open={openCadastro}
          onOpenChange={() => setOpenCadastro(!openCadastro)}
        />
      )}

      {openEdicao && id && (
        <Formulario
          id={id}
          open={openEdicao}
          onOpenChange={() => setOpenEdicao(!openEdicao)}
        />
      )}

      {openModalExcluir && (
        <AlertDialog open={openModalExcluir} onOpenChange={handleCloseModal}>
          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>Tem certeza?</AlertDialogTitle>
              <AlertDialogDescription>
                Está ação não pode ser desfeita. Você tem certeza que deseja
                excluir essa pessoa?
              </AlertDialogDescription>
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel>Cancelar</AlertDialogCancel>
              <AlertDialogAction onClick={() => handleDelete(id)}>
                Excluir
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
      )}
    </div>
  );
}
