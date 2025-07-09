# API Hora do Lixo

## 📖 Sobre o Projeto

A API **Hora do Lixo** é um serviço de backend desenvolvido em ASP.NET Core 7 para gerenciar usuários e informações sobre a coleta de lixo. O objetivo é fornecer dados para que um aplicativo cliente possa notificar os usuários sobre os horários da coleta comum e seletiva em suas respectivas zonas.

---

## ✨ Funcionalidades

* **Gerenciamento de Usuários**: Cadastro (`Create`), Leitura (`Read`), Atualização (`Update`).
* **Autenticação**: Sistema de Login com geração de Token JWT (JSON Web Token).
* **Consulta de Coleta**: Obtenção de informações detalhadas sobre as zonas e horários de coleta de um usuário.
* **Configuração de Alertas**: Permite ao usuário definir e ativar/desativar alertas para os dias de coleta.

---

## 🚀 Tecnologias e Pacotes Utilizados

Esta API foi construída com as seguintes tecnologias e pacotes NuGet:

### Tecnologias Principais
* **.NET 7.0**: Framework de desenvolvimento da aplicação.
* **ASP.NET Core Web API**: Estrutura para criação de serviços RESTful.
* **SQL Server**: Banco de dados relacional para armazenamento dos dados.
* **ADO.NET**: Camada de acesso a dados para comunicação com o SQL Server.

### Pacotes NuGet Instalados
| Pacote                                         | Versão (Compatível com .NET 7) | Finalidade                                        |
| ---------------------------------------------- | ------------------------------ | ------------------------------------------------- |
| `Microsoft.AspNetCore.Authentication.JwtBearer`| `7.0.20`                       | Autenticação e validação de tokens JWT na API.    |
| `System.IdentityModel.Tokens.Jwt`              | `7.6.0`                        | Criação e manipulação de tokens JWT.              |
| `BCrypt.Net-Next`                              | `4.0.3`                        | Hashing seguro de senhas de usuários.             |
| `Microsoft.Data.SqlClient`                     | `5.2.1`                        | Provedor de dados para conectar ao SQL Server.    |

---

## 🛠️ Configuração e Execução do Projeto

Siga os passos abaixo para configurar e rodar o projeto em um ambiente de desenvolvimento.

### Pré-requisitos
* **[.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)**
* **Visual Studio 2022 Community** (ou superior)
* **SQL Server Express** (ou superior)

### Passos para Configuração

1.  **Clone o Repositório**
    ```bash
    git clone [URL_DO_SEU_REPOSITORIO]
    cd HoraDoLixo
    ```

2.  **Configure o Banco de Dados**
    * Abra o SQL Server e crie um banco de dados chamado `HoraDoLixoDB`.
    * Execute os scripts SQL (fornecidos separadamente) para criar as tabelas `Usuario`, `ZonaColetaComum`, `ProgramacaoColetaComum`, etc.

3.  **Configure a String de Conexão**
    * Abra o arquivo `appsettings.json`.
    * Verifique se a `ConnectionStrings.DefaultConnection` está configurada corretamente para o seu ambiente. A configuração padrão é:
    ```json
    "DefaultConnection": "Server=NOME_DO_SEU_SERVER\\SQLEXPRESS;Database=HoraDoLixoDB;Trusted_Connection=True;TrustServerCertificate=True;"
    ```

4.  **Configure a Chave Secreta do JWT (Segurança)**
    * É altamente recomendado usar o "User Secrets" para armazenar a chave do JWT em desenvolvimento.
    * Abra o terminal na pasta do projeto e execute os comandos:
    ```bash
    dotnet user-secrets init
    dotnet user-secrets set "Jwt:Key" "SuaChaveSecretaSuperLongaEDificilDeAdivinharAqui"
    ```

5.  **Execute a Aplicação**
    * Abra o projeto no Visual Studio.
    * Pressione `F5` ou o botão de play "HoraDoLixo" para iniciar a API.

---

## Endpoints da API

| Método | URL                               | Descrição                                         |
| :----- | :-------------------------------- | :------------------------------------------------ |
| `POST` | `/api/Usuario`                    | Cadastra um novo usuário.                         |
| `POST` | `/api/Usuario/login`              | Autentica um usuário e retorna um token JWT.      |
| `GET`  | `/api/Usuario`                    | Lista todos os usuários.                          |
| `GET`  | `/api/Usuario/{id}`               | Obtém os dados de um usuário específico.          |
| `PUT`  | `/api/Usuario/{id}`               | Atualiza os dados de um usuário específico.       |
| `GET`  | `/api/Usuario/{id}/coleta`        | Obtém as informações de coleta do usuário.        |

✅ Opções mais comuns de entrega de alerta (para seu cenário):
Canal	                            Custo	                            Complexidade	                    Alcance
E-mail	                            Baixo	                            Baixa	                            Muito bom (quase todo mundo tem e-mail)
Notificação Push (App Mobile)	    Médio/Alto	                        Alta (exige app mobile publicado)	Muito bom, mas só se tiver app
WhatsApp (via API)	                Médio/Alto (tem custo por mensagem)	Média	                            Excelente engajamento
SMS	                                Alto (paga por mensagem)	        Baixa	                            Alcance universal
In-app Notification (Web Frontend)	Baixo	                            Baixa	                            Só para quem está online no momento
