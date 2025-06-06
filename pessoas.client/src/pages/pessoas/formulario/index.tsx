import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import * as z from "zod";
import { Check, ChevronsUpDown } from "lucide-react";

import { Button } from "../../../components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../../../components/ui/dialog";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../../../components/ui/form";
import { Input } from "../../../components/ui/input";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "../../../components/ui/popover";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "../../../components/ui/command";
import { cn } from "../../../lib/utils";
import { pessoaService } from "../../../service/pessoas.service";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { queryClient } from "../../../main";
import { useQuery } from "@tanstack/react-query";
import { useEffect } from "react";

const formSchema = z.object({
  id: z.string().nullable(),
  nome: z.string().min(2, { message: "Nome deve ter pelo menos 2 caracteres" }),
  email: z.string().email({ message: "Email inválido" }).nullable(),
  dataNascimento: z
    .string()
    .min(10, { message: "Data de nascimento inválida" })
    .max(10),
  cpf: z.string().min(14, { message: "CPF inválido" }).max(14),
  endereco: z.string().max(255, { message: "Endereço inválido" }).nullable(),
  sexo: z.number().optional(),
  nacionalidade: z.number().optional(),
  naturalidade: z
    .string()
    .min(2, { message: "Naturalidade deve ter pelo menos 2 caracteres" })
    .nullable(),
});

type FormData = z.infer<typeof formSchema>;

const sexos = [
  { label: "Masculino", value: 1 },
  { label: "Feminino", value: 2 },
  { label: "Outro", value: 3 },
];

const nacionalidades = [
  { label: "Brasileira", value: 1 },
  { label: "Argentina", value: 2 },
  { label: "Uruguaia", value: 3 },
  { label: "Chilena", value: 4 },
  { label: "Colombiana", value: 5 },
  { label: "Venezuelana", value: 6 },
  { label: "Paraguai", value: 7 },
  { label: "Boliviana", value: 8 },
  { label: "Peruana", value: 9 },
  { label: "Equatoriana", value: 10 },
  { label: "Mexicana", value: 11 },
  { label: "Americana", value: 12 },
  { label: "Canadense", value: 13 },
  { label: "Espanhola", value: 14 },
  { label: "Portuguesa", value: 15 },
  { label: "Italiana", value: 16 },
  { label: "Alemã", value: 17 },
  { label: "Francesa", value: 18 },
  { label: "Inglesa", value: 19 },
  { label: "Japonesa", value: 20 },
  { label: "Chinesa", value: 21 },
  { label: "Coreana", value: 22 },
  { label: "Indiana", value: 23 },
  { label: "Australiana", value: 24 },
  { label: "Moçambicana", value: 25 },
  { label: "Angolana", value: 26 },
];

export type FormularioCadatro = {
  id?: string;
  open: boolean;
  onOpenChange: (open: boolean) => void;
};

export function Formulario({ id, open, onOpenChange }: FormularioCadatro) {
  const navigate = useNavigate();

  const { data } = useQuery({
    queryKey: ["get-pessoas-by-id", id],
    queryFn: async () => await pessoaService.getPessoaById(id),
    enabled: !!id,
  });

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      id: null,
      nome: "",
      email: null,
      cpf: "",
      endereco: null,
      naturalidade: null,
      nacionalidade: 0,
      sexo: 0,
      dataNascimento: "",
    },
  });

  useEffect(() => {
    if (data) {
      form.reset({
        id: data.data.id ?? null,
        nome: data.data.nome ?? "",
        email: data.data.email ?? null,
        cpf: formatCPF(data.data.cpf ?? ""),
        endereco: data.data.endereco ?? null,
        naturalidade: data.data.naturalidade ?? null,
        nacionalidade:
          nacionalidades.find((n) => n.label === data.data.nacionalidade)
            ?.value ?? 0,
        sexo: sexos.find((s) => s.label === data.data.sexo)?.value ?? 0,
        dataNascimento: formatDate(data.data.dataNascimento ?? ""),
      });
    }
  }, [data, form]);

  async function onSubmit(data: FormData) {
    if (id && data.id) {
      await pessoaService
        .putPessoa(data)
        .then(() => {
          toast.success("Pessoa editada com sucesso!");
          queryClient.refetchQueries({ queryKey: ["get-pessoas-paginado"] });
          navigate("/");
        })
        .catch(() => toast.error("Erro ao editar a pessoa"));
    } else {
      await pessoaService
        .postPessoa({
          ...data,
          email: data.email ?? undefined,
          endereco: data.endereco ?? undefined,
          naturalidade: data.naturalidade ?? undefined,
        })
        .then(() => {
          toast.success("Pessoa cadastrada com sucesso!");
          queryClient.refetchQueries({ queryKey: ["get-pessoas-paginado"] });
          navigate("/");
        })
        .catch(() => toast.error("Erro ao cadastrar a pessoa"));
    }

    onOpenChange(false);
    form.reset();
  }

  // Função para aplicar máscara ao CPF
  const formatCPF = (value: string) => {
    const cpfDigits = value.replace(/\D/g, "");
    let formattedCPF = "";

    if (cpfDigits.length <= 3) {
      formattedCPF = cpfDigits;
    } else if (cpfDigits.length <= 6) {
      formattedCPF = `${cpfDigits.slice(0, 3)}.${cpfDigits.slice(3)}`;
    } else if (cpfDigits.length <= 9) {
      formattedCPF = `${cpfDigits.slice(0, 3)}.${cpfDigits.slice(
        3,
        6
      )}.${cpfDigits.slice(6)}`;
    } else {
      formattedCPF = `${cpfDigits.slice(0, 3)}.${cpfDigits.slice(
        3,
        6
      )}.${cpfDigits.slice(6, 9)}-${cpfDigits.slice(9, 11)}`;
    }

    return formattedCPF;
  };

  // Função para aplicar máscara à data
  const formatDate = (value: string) => {
    const dateDigits = value.replace(/\D/g, "");
    let formattedDate = "";

    if (dateDigits.length <= 2) {
      formattedDate = dateDigits;
    } else if (dateDigits.length <= 4) {
      formattedDate = `${dateDigits.slice(0, 2)}/${dateDigits.slice(2)}`;
    } else {
      formattedDate = `${dateDigits.slice(0, 2)}/${dateDigits.slice(
        2,
        4
      )}/${dateDigits.slice(4, 8)}`;
    }

    return formattedDate;
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px]">
        <DialogHeader>
          <DialogTitle>{id ? "Edição de Dados" : "Cadastro de Pessoa"}</DialogTitle>
          <DialogDescription>
            Preencha os dados da pessoa para continuar.
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="nome"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Nome</FormLabel>
                    <FormControl>
                      <Input placeholder="Nome completo" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Email</FormLabel>
                    <FormControl>
                      <Input {...field} value={field.value ?? ""} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="dataNascimento"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Data de Nascimento</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="DD/MM/AAAA"
                        {...field}
                        onChange={(e) => {
                          const formattedValue = formatDate(e.target.value);
                          field.onChange(formattedValue);
                        }}
                        maxLength={10}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="cpf"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>CPF</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="000.000.000-00"
                        {...field}
                        onChange={(e) => {
                          const formattedValue = formatCPF(e.target.value);
                          field.onChange(formattedValue);
                        }}
                        maxLength={14}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <FormField
              control={form.control}
              name="endereco"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Endereço</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="Rua, número, bairro, cidade, estado"
                      {...field}
                      value={field.value ?? ""}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <div className="grid grid-cols-2 gap-4">
              <FormField
                control={form.control}
                name="sexo"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <FormLabel>Sexo</FormLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <FormControl>
                          <Button
                            variant="outline"
                            role="combobox"
                            className={cn(
                              "w-full justify-between",
                              !field.value && "text-muted-foreground"
                            )}
                          >
                            {field.value
                              ? sexos.find((sexo) => sexo.value === field.value)
                                  ?.label
                              : "Selecione o sexo"}
                            <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                      <PopoverContent className="w-full p-0">
                        <Command>
                          <CommandInput placeholder="Buscar sexo..." />
                          <CommandList>
                            <CommandEmpty>Nenhum sexo encontrado.</CommandEmpty>
                            <CommandGroup>
                              {sexos.map((sexo) => (
                                <CommandItem
                                  key={sexo.value}
                                  value={sexo.value.toString()}
                                  onSelect={() => {
                                    form.setValue("sexo", sexo.value);
                                  }}
                                >
                                  <Check
                                    className={cn(
                                      "mr-2 h-4 w-4",
                                      sexo.value === field.value
                                        ? "opacity-100"
                                        : "opacity-0"
                                    )}
                                  />
                                  {sexo.label}
                                </CommandItem>
                              ))}
                            </CommandGroup>
                          </CommandList>
                        </Command>
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="nacionalidade"
                render={({ field }) => (
                  <FormItem className="flex flex-col">
                    <FormLabel>Nacionalidade</FormLabel>
                    <Popover>
                      <PopoverTrigger asChild>
                        <FormControl>
                          <Button
                            variant="outline"
                            role="combobox"
                            className={cn(
                              "w-full justify-between",
                              !field.value && "text-muted-foreground"
                            )}
                          >
                            {field.value
                              ? nacionalidades.find(
                                  (nacionalidade) =>
                                    nacionalidade.value === field.value
                                )?.label
                              : "Selecione a nacionalidade"}
                            <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                          </Button>
                        </FormControl>
                      </PopoverTrigger>
                      <PopoverContent className="w-full p-0">
                        <Command>
                          <CommandInput placeholder="Buscar nacionalidade..." />
                          <CommandList>
                            <CommandEmpty>
                              Nenhuma nacionalidade encontrada.
                            </CommandEmpty>
                            <CommandGroup>
                              {nacionalidades.map((nacionalidade) => (
                                <CommandItem
                                  key={nacionalidade.value}
                                  value={nacionalidade.value.toString()}
                                  onSelect={() => {
                                    form.setValue(
                                      "nacionalidade",
                                      nacionalidade.value
                                    );
                                  }}
                                >
                                  <Check
                                    className={cn(
                                      "mr-2 h-4 w-4",
                                      nacionalidade.value === field.value
                                        ? "opacity-100"
                                        : "opacity-0"
                                    )}
                                  />
                                  {nacionalidade.label}
                                </CommandItem>
                              ))}
                            </CommandGroup>
                          </CommandList>
                        </Command>
                      </PopoverContent>
                    </Popover>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <FormField
              control={form.control}
              name="naturalidade"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Naturalidade</FormLabel>
                  <FormControl>
                    <Input
                      placeholder="Cidade/Estado de nascimento"
                      {...field}
                      value={field.value ?? ""}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={() => onOpenChange(false)}
              >
                Cancelar
              </Button>
              <Button type="submit">Salvar</Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
