Ecommerce.AI: AI-Powered Online Store

A full-stack e-commerce app extended with semantic search, hybrid search and a chatbot assistant, built with .NET 9, Angular 20, Azure OpenAI and PostgreSQL + pgvector, running in Docker.

Home page

Features
Catalog search: classic product browsing with brand/type filters, sorting and pagination
Semantic search: natural-language queries (e.g. "give me best kitbags") matched by meaning using embeddings stored in pgvector
Hybrid search: keyword search combined with semantic similarity
AI chatbot (in progress): conversational product recommendations
Containerized AI stack: AiService, pgvector and Ollama via Docker Compose
Architecture
REST
REST
embeddings / chat
optional local models
Browser
Angular 20 client :4200
Catalog API :8000
AiService .NET 9 :8090
MongoDB
Azure OpenAI
PostgreSQL + pgvector
Ollama
Layer	Technology
Frontend	Angular 20, standalone components, Signals, RxJS
AI service	.NET 9, ASP.NET Core Minimal APIs
Embeddings / LLM	Azure OpenAI (text-embedding-3-small), Ollama (optional)
Vector database	PostgreSQL 16 + pgvector
Catalog	.NET microservice + MongoDB (from a separate course, see Credits)
Containers	Docker, Docker Compose
Prerequisites
Tool	Version / notes
Git	any recent version
Docker Desktop	running before you start
Node.js	20.19+, 22.12+ or 24+
Angular CLI	npm install -g @angular/cli
.NET 9 SDK	only if you run AiService outside Docker
Azure OpenAI resource	an embedding deployment (text-embedding-3-small) and a chat deployment
Internet connection	required: every search and chat request calls Azure OpenAI

Offline? Everything except the AI calls works without internet. Semantic search, hybrid search and chat need a connection to Azure.

Step-by-step: run the app
Step 1. Clone the repository
bash
git clone https://github.com/<your-username>/Ecommerce.AI.git
cd Ecommerce.AI
Step 2. Create the Azure OpenAI deployment
In the Azure Portal create an Azure OpenAI resource.
In Azure AI Foundry / Model deployments, deploy text-embedding-3-small (and a chat model for the assistant).
Copy the endpoint and an API key from Keys and Endpoint.

Azure OpenAI deployments

Never commit your key. Keep it only in .env.

Step 3. Configure secrets

Create a .env file in the repository root (next to docker-compose.yml):

env
AZURE_OPENAI_KEY=<your-azure-openai-key>

Set your endpoint and deployment names in the AiService configuration (the AzureOpenAI section of AiService/appsettings.json).

Make sure .env is listed in .gitignore.

Step 4. Start Docker Desktop

Open Docker Desktop and wait until the engine reports Engine running.

Step 5. Start the AI stack

From the repository root:

bash
docker-compose up -d --build

This starts three containers:

Container	Port	Purpose
aiservice	8090	.NET 9 AI API (search + chat)
pgvector	5433	PostgreSQL 16 with vector extension
ollama	11434	optional local models

docker-compose up

Docker Desktop containers

Step 6. Start the Catalog microservice

The product catalog comes from the companion microservices project (see Credits). Start its stack with Docker Compose so that Catalog API is available on port 8000.

Step 7. Verify the backend
Check	URL	Expected
AiService Swagger	http://localhost:8090/swagger	endpoints /search/vector, /search/hybrid, /search/keyword, /chat/ask, /health
Catalog Swagger	http://localhost:8000/swagger	/api/v1/Catalog/... endpoints
Catalog data	http://localhost:8000/api/v1/Catalog/GetAllBrands	JSON list of brands

AiService Swagger

Catalog Swagger

First run only: if semantic search returns nothing, the vector table is empty. Call POST /semantic/load in Swagger once to generate embeddings for the products.

Step 7b. Verify the API addresses in the client

In Client/src/app/store/services/product.service.ts and Client/src/app/core/services/chat.service.ts:

ts
// product.service.ts
private baseUrl = 'http://localhost:8000/api/v1/Catalog';
private aiUrl   = 'http://localhost:8090';

// chat.service.ts
private baseUrl = 'http://localhost:8090';
Step 8. Start the Angular client
bash
cd Client
npm install      # first time only
ng serve

Wait for Application bundle generation complete and the line Local: http://localhost:4200/.

ng serve

Step 9. Open the app

Open http://localhost:4200.

Home page

Step 10. Try the three search modes

Use the dropdown next to the search box.

Mode	Example query	What happens
Catalog	football	classic product listing
Semantic	give me best kitbags	finds products by meaning via embeddings
Hybrid	gear for racket sports	keyword + semantic ranking

Catalog

Semantic search

Hybrid search

Step 11. Chatbot (in progress)

Chatbot

How semantic search works
The user's query is sent to AiService (POST /search/vector).
AiService requests an embedding for the query from Azure OpenAI.
pgvector returns the products whose stored embeddings are closest to the query vector.
The Angular client normalizes the response (brand/type to brands/types) and renders the product cards.
Troubleshooting
Problem	Cause / fix
npm or ng blocked in PowerShell	Set-ExecutionPolicy -Scope CurrentUser RemoteSigned, then open a new terminal
ng serve says outside a workspace	run it inside the Client folder (where angular.json is)
localhost:4200 not loading	ng serve is not running; start it again, it does not start automatically
Catalog request returns 404	use the versioned path /api/v1/Catalog/...
ERR_SSL_PROTOCOL_ERROR on :8090	use http:// (not https://) for the Docker container
ERR_CONNECTION_REFUSED on :8000 or :8090	the container is not running; check Docker Desktop
Semantic search returns 500 with Name or service not known	no internet or DNS failure inside the container; check connection and restart Docker Desktop
Postgres container restarts endlessly after an image upgrade	pin the version (e.g. postgres:16) instead of latest
Search results appear empty or the page breaks	AI results use brand/type; make sure the client normalization in product.service.ts is present
Project status
Area	Status
Catalog browsing, filters, pagination	Done
Semantic search (pgvector + Azure OpenAI)	Done
Hybrid search	Done
Chatbot with conversation context	In progress
Voice input/output	Planned
Basket, checkout, authentication via API gateway	Planned
What I added or adapted
Took the Angular client from the author's microservices repository and adapted it to my setup
Ran AiService, pgvector and Ollama together in Docker Compose
Fixed the response mapping between AI search results and the catalog model in the Angular service
Aligned API paths and ports between the client and the containers
Documented the full setup and troubleshooting path
Credits

This project is based on two Udemy courses by Rahul Sahay:

AI-Powered E-Commerce App with .NET 9, Angular 20 & RAG: AI integration, semantic search, chatbot
Building Amazon Style Full Stack Microservices: backend microservices and the Angular storefront (course repository)

Note on the frontend: the Angular client in the Client folder was taken from Rahul Sahay's microservices repository linked above (the Client folder there) and then adapted by me to run against my own containers (API ports, API paths and the AI search response mapping). The Catalog microservice also comes from that project. The AI part (AiService, pgvector, Azure OpenAI integration) is built by following the AI course.

Author

Tihomir Georgiev: GitHub
