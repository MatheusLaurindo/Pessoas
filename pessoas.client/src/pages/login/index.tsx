import { z } from "zod";
import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginService } from "../../service/login.service";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

const loginSchema = z.object({
  email: z.string().email("Email inválido"),
  senha: z.string(),
});

export type LoginForm = z.infer<typeof loginSchema>;

export default function Login() {
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      senha: "",
    },
  });

  async function onSubmit(data: LoginForm) {
    await loginService
      .login(data)
      .then(() => {
        toast.success("Login realizado com sucesso!");
        navigate("/");
      })
      .catch(() => toast.error("Erro ao realizar login!"));
  }
  return (
    <div className="flex min-h-screen flex-col md:flex-row">
      <div className="hidden md:flex bg-primary text-white items-center justify-center p-8 md:w-1/2">
        <div className="max-w-md text-center">
          <h1 className="text-4xl font-bold mb-4">SEJA BEM VINDO!</h1>
          <p className="text-lg">Faça o login para continuar</p>
        </div>
      </div>
      <div className="flex items-center justify-center p-4 md:w-1/2 mt-16 md:mt-0">
        <Card className="w-full max-w-md">
          <CardHeader className="space-y-1">
            <CardTitle className="text-2xl font-bold text-center">
              Login
            </CardTitle>
            <CardDescription className="text-center">
              Entre com seu email e senha para acessar sua conta.
            </CardDescription>
          </CardHeader>
          <form onSubmit={handleSubmit(onSubmit)}>
            <CardContent>
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    type="email"
                    {...register("email")}
                    required
                  />
                  <span className="p-1 text-red-600">
                    {errors.email?.message}
                  </span>
                </div>
                <div className="space-y-2">
                  <div className="flex items-center justify-between">
                    <Label htmlFor="password">Senha</Label>
                  </div>
                  <Input
                    id="senha"
                    type="password"
                    {...register("senha")}
                    required
                  />
                  <span className="p-1 text-red-600">
                    {errors.senha?.message}
                  </span>
                </div>
              </div>
            </CardContent>
            <CardFooter>
              <Button type="submit" className="w-full bg-primary -mt-5">
                Entrar
              </Button>
            </CardFooter>
          </form>
        </Card>
      </div>
    </div>
  );
}
