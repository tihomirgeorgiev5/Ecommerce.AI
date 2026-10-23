# Ecommerce.AI

An AI-enabled online store with semantic search and a chatbot assistant,
built with .NET 9 and Angular 20. The project is designed to be extended
with Retrieval-Augmented Generation (RAG).

> **Learning project.** Built while following the Udemy course
> "AI-Powered E-Commerce App with .NET 9, Angular 20 & RAG" by Rahul Sahay.

## Features

- Product catalog with three search modes: **catalog**, **semantic** and **hybrid**
- Semantic search based on text embeddings stored in PostgreSQL (pgvector)
- Chatbot assistant that understands natural-language questions and recommends products
- Basket and checkout flow
- API Gateway routing all requests to the services
- Whole stack runs in containers with Docker Compose

## Tech stack

| Area | Technology |
|---|---|
| Backend | .NET 9, ASP.NET Core Minimal APIs, C# |
| Architecture | Clean Architecture, repository pattern |
| Frontend | Angular 20 (standalone components, Signals) |
| AI | Azure OpenAI or Ollama for embeddings and chat |
| Database | PostgreSQL with pgvector |
| Gateway | Ocelot |
| Containers | Docker, Docker Compose |

## Project structure

| Folder / file | Description |
|---|---|
| `AiService` | <What this service does, for example embeddings and chat> |
| `<other services>` | <Catalog, basket, gateway, frontend, if present> |
| `docker-compose.yml` | Runs all services together |
| `.github` | <Workflows, if any> |

## Getting started

### Prerequisites

- .NET 9 SDK
- Node.js (LTS) and Angular CLI
- Docker Desktop
- An Azure OpenAI resource **or** a local Ollama installation

### Configuration

| Setting | Description |
|---|---|
| `<AzureOpenAI endpoint>` | Endpoint of your Azure OpenAI resource |
| `<AzureOpenAI key>` | API key |
| `<Embedding model name>` | Model used to create embeddings |

Never commit API keys. Use environment variables, Docker secrets or
.NET user secrets (`dotnet user-secrets set "<Name>" "<value>"`).

### Run with Docker Compose

```bash
docker compose up --build
```

| Service | Address |
|---|---|
| Frontend | <http://localhost:xxxx> |
| API Gateway | <http://localhost:xxxx> |

## Roadmap

- [x] Semantic and hybrid search
- [x] Chatbot assistant
- [x] Basket and checkout
- [ ] Retrieval-Augmented Generation (RAG) pipeline
- [ ] Voice input and output
- [ ] Conversation memory across turns
- [ ] Web search augmentation

## Acknowledgements

Based on the course by [Rahul Sahay](https://www.udemy.com/user/rahul-sahay/)
on Udemy. <Add the course link if you want.>

## License

<Choose a license, or write "No license yet.">
