# MyFinance Web

Gerencie suas finanças pessoais de forma simples, visual e eficiente.

---

## Visão Geral

**MyFinance Web** é um aplicativo pensado para famílias que buscam registrar entradas e saídas de dinheiro, acompanhar o fluxo financeiro mensal e tomar decisões mais conscientes sobre o orçamento.

---

## Por que usar?

Segundo pesquisas recentes, mais da metade dos brasileiros não sabe como planejar suas finanças para os próximos anos. O MyFinance Web foi criado justamente para preencher essa lacuna, oferecendo uma interface intuitiva e ferramentas práticas para controle financeiro.

---

## Como está estruturado?

O projeto segue o padrão **MVC** (Model-View-Controller):

- **Modelos:** Estruturas de dados como Plano de Contas e Transações
- **Visualizações:** Páginas dinâmicas criadas com Razor Pages
- **Controladores:** Responsáveis pelo processamento das regras de negócio

### Organização dos arquivos
├── Controllers/   # Controladores MVC   
├── Models/   # Modelos de dados  
├── Views/   # Interfaces de usuário  
├── Data/   # Contexto do banco de dados  
├── wwwroot/   # Arquivos estáticos (CSS, JS, imagens)  
└── docs/   # Documentação e scripts  


---

## Banco de Dados

- **ORM:** Entity Framework Core 6.0
- **Banco:** SQLite (ambiente de desenvolvimento)
- **Migrations:** Controle de versões do banco
- **Relacionamento:** Cada transação está vinculada a um plano de contas

---

## Stack de Tecnologias

| Camada        | Tecnologias e Ferramentas           |
|---------------|-------------------------------------|
| Backend       | ASP.NET Core 6, EF Core 6, C# 10, SQLite |
| Frontend      | HTML5, CSS3, Bootstrap 5, JS ES6, Chart.js, FontAwesome |
| Dev Tools     | .NET CLI, EF CLI, Visual Studio/VS Code |

---

## Guia de Instalação

### Requisitos

- .NET 6.0 SDK instalado
- Editor de código (Visual Code ou similar)

### Passo a passo

1. **Clone o repositório**
    ```
    git clone <url-do-repositorio>
    cd myfinance-web-dotnet-o8/myfinance-web-netcore
    ```

2. **Baixe as dependências**
    ```
    dotnet restore
    ```

3. **(Opcional) Instale o Entity Framework CLI**
    ```
    dotnet tool install --global dotnet-ef --version 6.0.25
    ```

4. **Verifique a configuração do banco**
    O arquivo `appsettings.json` já vem pronto para SQLite:
    ```
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=myfinance.db"
      }
    }
    ```

5. **Crie e atualize o banco de dados**
    ```
    dotnet ef migrations add InitialCreate   # Caso ainda não exista
    dotnet ef database update
    ```

6. **Inicie a aplicação**
    ```
    dotnet run
    ```

7. **Acesse no navegador**
    - HTTPS: https://localhost:7006
    - HTTP:  http://localhost:5088

---

## Créditos e Licença

Este software foi desenvolvido como parte do curso de Pós-Graduação em Engenharia de Software (PUC Minas), na disciplina de Práticas de Implementação e Evolução de Software.

**Autor:** Gustavo Braulio

---

