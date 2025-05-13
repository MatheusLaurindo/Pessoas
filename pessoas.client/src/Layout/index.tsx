import { Outlet } from "react-router-dom";
import { ToastContainer } from 'react-toastify';

export default function Layout() {
  return (
    <div className="relative min-h-screen flex flex-col">
      <header className="fixed w-full bg-secondary shadow px-6 py-4 flex justify-between items-center">
        <h1 className="text-xl font-semibold">Gerenciamento de Pessoas</h1>
      </header>

      <main className="flex-1 flex items-center justify-center">
        <ToastContainer />
        <div className="w-full"><Outlet /></div>
      </main>
    </div>
  );
}
