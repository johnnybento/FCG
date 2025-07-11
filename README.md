# FIAP Cloud Games (FCG)

**Grupo: Dev FCG Solo**

**Usuário discord: johnnybento#9118**

**link da documentação: https://drive.google.com/drive/folders/1fYKynvxK0IOdTGuICBVDYDsHvkrWra6b?usp=drive_link**

**link do repositório: https://github.com/johnnybento/FCG**

**link do vídeo: https://drive.google.com/file/d/1EkY2c0655dE1FP8mwduRPAzv9SeWCUSU/view?usp=drive_link**





Uma plataforma de venda de jogos digitais e gestão de servidores para partidas online, construída com .NET 8 seguindo princípios de Domain‑Driven Design (DDD) e Clean Architecture.

---

##  Visão Geral

* **Objetivo**: Vender jogos digitais, gerenciar biblioteca de usuários, aplicações de promoções e alocação de partidas via matchmaking e servidores dedicados.
* **Público‑alvo**: Jogadores, administradores de plataforma e operadores de servidores de jogo.

---

##  Escopo do Projeto 

Para garantir clareza, escalabilidade e alinhamento entre as equipes, o FCG é organizado em Contextos Delimitados (Bounded Contexts):

* **User Management**: cadastro, autenticação, perfil de usuário e gestão de biblioteca de jogos (entidade `Usuario`, `JogoAdquirido`).
* **Catalog**: manutenção de catálogo de jogos (`Jogo`) e ofertas de promoções (`Promocao`).
* **Sales**: fluxo de compra de jogos (`ComprarJogoCommand`), aplicação de descontos e persistência de transações.


Utilizamos uma **Linguagem Ubíqua** (Ubiquitous Language) para nomear entidades, comandos e eventos, garantindo que termos como *CreateUserCommand*, *UsuarioCadastradoEvent*, *JogoAdquiridoNaBibliotecaEvent* e *PromocaoCriadaEvent* tenham significado claro e compartilhado entre desenvolvedores e stakeholders.

O design de domínio segue os princípios de **Domain‑Driven Design (DDD)**:

* **Aggregates** protegem invariantes e encapsulam regras de negócio.
* **Value Objects** (`EmailVo`, `SenhaVo`) garantem integridade de tipos específicos.
* **Domain Events** refletem mudanças de estado importantes e permitem lógica assíncrona.

---

##  Arquitetura

A solução está dividida em quatro camadas principais:



1. **Domain** (FCG.Domain)

   * Entidades, Value Objects e Aggregates
   * Contratos de repositórios (interfaces)
   * Domain Events (fila estática) e modelos de eventos
2. **Application** (FCG.Application)

   * Commands/Queries, Handlers e Validators (MediatR + FluentValidation)
   * DTOs de aplicação e AutoMapper Profiles
   * Contratos de infra (IPasswordHasher, IJwtService, IEmailSender, IDomainEventDispatcher)
3. **Infrastructure** (FCG.Infrastructure)

   * EF Core 8 (DefaultDbContext, Configurations, Migrations)
   * Implementações de repositórios e serviços (hashing, JWT, e‑mail)
   * Dispatcher de Domain Events (publicação via MediatR)
   * Integração com Redis (fila) e Kafka (event bus) — Fase 4
4. **WebApi** (FCG.WebApi)

   * Minimal API (.NET 8) organizada por módulos de endpoints
   * Swagger/OpenAPI (documentação interativa)
   * Autenticação JWT Bearer e autorização por roles/policies
   * Middleware de tratamento de erros e logs

---

##  Tecnologias e Ferramentas

* **Linguagem**: C# com .NET 8
* **ORM**: EF Core 8 + SQL Server
* **Autenticação**: JWT Bearer
* **Validação**: FluentValidation
* **Mapeamento**: AutoMapper
* **Dependency Injection**: Microsoft.Extensions.DependencyInjection
* **Documentação**: Swagger/OpenAPI, Event Storming e Domain Storytelling

---

## Funcionalidades por Fase

### Fase 1 – Cadastro de Usuários e Biblioteca

* Registro, atualização de perfil, alteração de senha e desativação de usuários
* Gestão de biblioteca: aquisição de jogos (entidade `JogoAdquirido`)
* Domain Events: `UsuarioCadastradoEvent`, `PerfilAtualizadoEvent`, `SenhaAlteradaEvent`, `UsuarioDesativadoEvent`, `JogoAdquiridoNaBibliotecaEvent`
* Commands, Queries e DTOs correspondentes
*Testes Unitarios

### Fase 2 – Catálogo de Jogos e Promoções

* Entidades `Jogo` e `Promocao`
* Promoções ativas (descontos, período)
* Eventos: `JogoCadastradoEvent`, `PromocaoCriadaEvent`

### Fase 3 – Compra de Jogos e Integração de Promoções

* Unificação do fluxo de compra com aplicação de desconto
* Command `ComprarJogoCommand` e resposta com `PrecoPago`


## Documentação

* **Swagger UI**: `GET /swagger` após rodar a aplicação
* **Event Storming**: adicione imagens em `docs/event-storming/`:

  * `01_CadastroUsuario.png`
  * `02_CompraJogo.png`
  * `03_Promocao.png`
  * …
* **Domain Storytelling**: imagens em `docs/domain-storytelling/`

> **Placeholder**:
>
> ```markdown
> ![Event Storming - Cadastro](/docs/event-storming/01_CadastroUsuario.png)
> ![Domain Storytelling - Compra](/docs/domain-storytelling/CompraFluxo.png)
> ```

---

##  Como Executar

1. **Pré-requisitos**: .NET 8 SDK SQL Server
2. \*\*Configurar \*\*\`\`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=FGC;User Id=sa;Password=SuaSenha!;TrustServerCertificate=True"
     },
     "Jwt": { "Issuer": "FCG-Auth-Server", "Audience": "FCG-Clients", "SecretKey": "<sua-chave-base64>", "ExpiresInMinutes": "120" },
     "Smtp": { /* Host, Port, User, Password, From, EnableSsl */ }
   }
   ```
3. **Migrações**:

   ```bash
   ```

dotnet ef database update --project FCG.Infrastructure --startup-project FCG.WebApi

````
4. **Rodar local**:
```bash
cd FCG.WebApi
dotnet run
````

* Acesse `https://ocalhost:7027:/swagger`



## Estrutura de Pastas

```

/FCG.sln 
		/src 
			/FCG.Domain 
			/FCG.Application 
			/FCG.Infrastructure 
			/FCG.WebApi 
			/Tests
				/FCG.Core.Tests
				/FCG.Application.Tests
		/docs 
			/event-storming 
			/domain-storytelling
			/video
			/README.md

```

---


---


```
