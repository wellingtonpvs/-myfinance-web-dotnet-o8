-- =====================================================
-- MyFinance Web - Script de Criação do Banco de Dados
-- Sistema de Controle Financeiro Pessoal
-- Desenvolvido em ASP.NET Core 6.0 com SQLite
-- =====================================================

-- Este script é gerado automaticamente pelo Entity Framework Core
-- Para SQLite, o banco é criado automaticamente através das migrations

-- =====================================================
-- ESTRUTURA DAS TABELAS
-- =====================================================

-- Tabela: PlanoContas
-- Armazena as categorias de receitas e despesas
CREATE TABLE IF NOT EXISTS "PlanoContas" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PlanoContas" PRIMARY KEY AUTOINCREMENT,
    "Codigo" INTEGER NOT NULL,
    "Descricao" TEXT NOT NULL COLLATE NOCASE,
    "Tipo" INTEGER NOT NULL,
    "DataCriacao" TEXT NOT NULL DEFAULT (datetime('now'))
);

-- Índice único para garantir códigos únicos
CREATE UNIQUE INDEX IF NOT EXISTS "IX_PlanoContas_Codigo" ON "PlanoContas" ("Codigo");

-- Tabela: Transacoes
-- Armazena as movimentações financeiras
CREATE TABLE IF NOT EXISTS "Transacoes" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Transacoes" PRIMARY KEY AUTOINCREMENT,
    "Historico" TEXT NOT NULL COLLATE NOCASE,
    "Data" TEXT NOT NULL,
    "PlanoContasId" INTEGER NOT NULL,
    "Valor" TEXT NOT NULL,
    "DataCriacao" TEXT NOT NULL DEFAULT (datetime('now')),
    CONSTRAINT "FK_Transacoes_PlanoContas_PlanoContasId" 
        FOREIGN KEY ("PlanoContasId") REFERENCES "PlanoContas" ("Id") ON DELETE RESTRICT
);

-- Índice para melhorar performance nas consultas
CREATE INDEX IF NOT EXISTS "IX_Transacoes_PlanoContasId" ON "Transacoes" ("PlanoContasId");
CREATE INDEX IF NOT EXISTS "IX_Transacoes_Data" ON "Transacoes" ("Data");

-- =====================================================
-- DADOS INICIAIS (SEED DATA)
-- =====================================================

-- Inserir Plano de Contas
INSERT OR IGNORE INTO "PlanoContas" ("Id", "Codigo", "Descricao", "Tipo", "DataCriacao") VALUES
(1, 1, 'Combustível', 0, datetime('now')),
(2, 2, 'Supermercado', 0, datetime('now')),
(3, 3, 'Alimentação', 0, datetime('now')),
(4, 4, 'IPTU', 0, datetime('now')),
(5, 5, 'IPVA', 0, datetime('now')),
(6, 6, 'Salário', 1, datetime('now')),
(7, 7, 'Escola', 0, datetime('now')),
(8, 8, 'Financiamento de Carro', 0, datetime('now'));

-- Inserir Transações de Exemplo
INSERT OR IGNORE INTO "Transacoes" ("Id", "Historico", "Data", "PlanoContasId", "Valor", "DataCriacao") VALUES
(1, 'Supermercado', '2025-07-05 13:00:00', 2, '450.00', datetime('now')),
(2, 'Jantar', '2025-06-23 20:30:00', 3, '250.00', datetime('now')),
(3, 'Salário', '2025-06-10 00:00:00', 6, '2000.00', datetime('now')),
(4, 'Mensalidade Escola', '2025-07-01 08:30:00', 7, '870.00', datetime('now'));
