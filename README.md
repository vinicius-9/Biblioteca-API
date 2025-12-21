# 📚 Biblioteca API

API REST desenvolvida em **ASP.NET Core** para controle de clientes, funcionários, livros e empréstimos.

---

## 🔐 Autenticação e Segurança

- Autenticação realizada por login de funcionário  
- Geração de token após login válido  
- Senhas não são armazenadas em texto puro  
- Utilização de **hash SHA256** para proteção das credenciais  

---

## 🧠 Regras e Comportamentos

- Apenas funcionários ativos podem autenticar no sistema  
- Livros inativos não podem ser emprestados  
- Não é permitido emprestar um livro que já esteja emprestado  
- CPF e email possuem restrições de unicidade  
- Empréstimos controlam data de retirada e devolução  

---

## 🗄️ Observação sobre Datas no Banco de Dados

- O SQL Server armazena datas no formato **DateTime** (data e hora)  
- Para utilizar apenas a **data**, é necessário converter ou definir a coluna como **DATE**  
- Essa conversão garante que horas, minutos e segundos não sejam considerados nos registros  

---

## 🚀 Tecnologias Utilizadas

- **C#**
- **ASP.NET Core**
- **.NET**
- **Entity Framework Core**
- **SQL Server**
- **JWT (JSON Web Token)**
- **Swagger / OpenAPI**

---

## 👨‍💻 Autor

Vinicius Pereira  

📧 vinicius.pereiragoncalves.online@gmail.com  

📅 2025
