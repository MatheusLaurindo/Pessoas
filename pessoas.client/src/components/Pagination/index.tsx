import { Button } from "../ui/button";
import { CaretDoubleLeft, CaretDoubleRight, CaretLeft, CaretRight } from "phosphor-react";

export interface PaginationProps {
  pageIndex: number;
  totalCount?: number;
  rowsPerPage: number;
  onPageChange: (pageIndex: number) => Promise<void> | void;
}

export function Pagination({
  pageIndex,
  totalCount,
  rowsPerPage,
  onPageChange,
}: PaginationProps) {
  const pages = Math.ceil((totalCount ?? 0) / rowsPerPage) || 1;

  return (
    <div className="flex items-center justify-between">
      <span className="text-sm text-muted-foreground">
        Total de {totalCount} item(s)
      </span>
      <div className="flex items-center gap-6 lg:gap-8">
        <div className="text-sm font-medium">
          Página {pageIndex + 1} de {pages}
        </div>
        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            className="hidden h-8 w-8 p-0 lg:flex"
            onClick={() => onPageChange(0)}
            disabled={pageIndex === 0}
          >
            <CaretDoubleLeft className="h-3 w-3" />
          </Button>
          <span>Primeira página</span>
          <Button
            variant="outline"
            className="h-8 w-8 p-0"
            onClick={() => onPageChange(pageIndex - 1)}
            disabled={pageIndex === 0}
          >
            <CaretLeft className="h-3 w-3" />
          </Button>
          <span>Página anterior</span>
          <Button
            variant="outline"
            className="h-8 w-8 p-0"
            onClick={() => onPageChange(pageIndex + 1)}
            disabled={pages <= pageIndex + 1}
          >
            <CaretRight className="h-3 w-3" />
          </Button>
          <span>Próxima página</span>

          <Button
            variant="outline"
            className="hidden h-8 w-8 p-0 lg:flex"
            onClick={() => onPageChange(pages - 1)}
            disabled={pages <= pageIndex + 1}
          >
            <CaretDoubleRight className="h-3 w-3" />
          </Button>
          <span>Última página</span>
        </div>
      </div>
    </div>
  );
}