# 🎓AcadGest

Sistema de **Gestão Académica** desenvolvido em **ASP.NET Core (.NET 8)**, com autenticação baseada em **Identity**, persistência com **Entity Framework Core** e geração de relatórios em **PDF com QuestPDF**.

## 🚀 Tecnologias Utilizadas
**.NET 8**
**ASP.NET Core MVC**
**Entity Framework Core**
**SQLite (padrão)**
**SQL Server (suportado)**
**ASP.NET Identity (GUID como chave primária)**
**Repository Pattern**
**QuestPDF**
**HTML & CSS**

## 🔹 Padrões aplicados
**Repository Pattern**
**Dependency Injection**
**Identity com Roles**
**Seed automático de dados**
**Separação clara entre camada de dados e interface**

## 🔐 Sistema de Autenticação

O projeto utiliza **ASP.NET Identity** com:

*AppUser
*Roles base:
  *Admin
  *User
  *Coordinator
  *Classdirector

As roles são criadas automaticamente no arranque da aplicação.

## 🗄 Base de Dados

Por padrão, o sistema usa **SQLite**, configurado em:

`builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));`

    Também suporta SQL Server, bastando ativar a configuração correspondente no `Program.cs`.

## 📄 Geração de PDFs
Relatórios são gerados usando:

`QuestPDF.Settings.License = LicenseType.Community;`

Biblioteca utilizada:
  *QuestPDF (licença Community)

## ⚙️ Como Executar o Projeto
### 1️⃣ Pré-requisitos
  *.NET 8 SDK
  *Visual Studio 2022+ ou VS Code
### 2️⃣ Clonar o repositório
  `git clone https://github.com/y-jr/acadgest.git
  cd acadgest`
### 3️⃣ Restaurar dependências
  `dotnet restore`
### 4️⃣ Aplicar migrations
  Se ainda não existir base de dados:
  `dotnet ef database update`
### 5️⃣ Executar
`dotnet run`

A aplicação estará disponível em:
  `https://localhost:5001`

## 🧪 Funcionalidades Principais
  *Gestão de utilizadores
  *Gestão de cursos
  *Gestão de turmas
  *Gestão de disciplinas
  *Gestão de alunos
  *Registo de notas
  *Controle por roles
  *Exportação de relatórios em PDF

## 📌 Dependências Principais

`Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Sqlite
Microsoft.EntityFrameworkCore.SqlServer
QuestPDF`

## 🎯 Objetivo do Projeto

Este projeto foi desenvolvido com foco em:
  *Consolidação de conhecimentos em ASP.NET Core
  *Implementação de autenticação robusta
  *Aplicação de boas práticas de arquitetura
  *Preparação para ambientes empresariais
## 📈 Possíveis Melhorias Futuras
  *API REST paralela
  *Implementação de testes unitários
  *Dockerização
  *CI/CD pipeline
  *Paginação e filtros avançados
  *Dashboard com gráficos

## 👨‍💻 Autor
  ### Desenvolvido por y-jr

[GitHub](https://github.com/y-jr)
[Linkedin](https://www.linkedin.com/in/adilson-muieba-6720043b1)
