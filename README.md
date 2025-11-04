🎨 Personal Stylist IA

Sistema de recomendações de moda com Inteligência Artificial — ASP.NET Core + OpenAI

O Personal Stylist IA é uma aplicação web que utiliza C#, ASP.NET Core Razor Pages, IA generativa e processamento de imagem para gerar sugestões de looks e recomendações personalizadas para o usuário.

Este projeto foi desenvolvido como um laboratório completo de aprendizado, simulando um ciclo real de desenvolvimento profissional:
✅ Design → ✅ Desenvolvimento → ✅ Testes (QA) → ✅ Correções → ✅ Release Final.

✅ 📌 Funcionalidades Principais
🔐 Autenticação Completa

Cadastro de usuário

Login

Logout

Sessões seguras

Fluxo de validação com mensagens claras

🧠 IA Textual – Recomendações Inteligentes

Página dedicada para geração de sugestões de moda via IA:

✅ Usuário descreve a ocasião
✅ IA gera um texto detalhado com recomendações personalizadas
✅ UI responsiva, moderna e estilizada
✅ Tratamento de erro seguro (SEG-004)
✅ Estados visuais: loading, error, success, empty

🖼️ IA Visual – Upload e Geração de Estilo

O usuário pode:

✅ Enviar imagem
✅ IA gera variação/recomendação visual
✅ Sistema exibe imagem com zoom inteligente
✅ Retentativas com fallback seguro caso a IA falhe
✅ Mensagens de processamento progressivo

🔍 Zoom Inteligente em Imagens

Implementação avançada:

Zoom com mouse (desktop)

Zoom com double tap (mobile)

Controle de origin do zoom

Sem interferência com o scroll

Responsivo e fluido

🎨 UI/UX Profissional

Desenvolvido com:

PicoCSS

Design system padronizado

Paleta:

--primary-blue: #1D4A89

--accent-orange: #FF6600

--error-red: #ef4444

✅ Navbar responsiva
✅ Menu hamburger funcional
✅ Layout unificado entre todas as páginas

🧪 Qualidade e Testes (QA)

O projeto passou por ciclos completos de QA:

QA-027: validação de recomendação IA

QA-031: validação de módulo de lojas

Testes de regressão

Simulação de falhas

Correção de fluxos críticos

Validação visual e funcional

✅ 🛠️ Tecnologias Utilizadas
Backend

C#

ASP.NET Core 8

Razor Pages

Entity Framework Core (SQLite)

Injeção de Dependência

HttpClient + OpenAI API

Frontend

HTML + Razor

CSS + PicoCSS

JavaScript modular

Zoom.js personalizado

Responsividade completa

Banco de Dados

SQLite

Migrations

Relacionamentos simples

Outros

Git & GitHub

GitHub Push Protection configurado

Tratamento seguro de segredos

Versionamento limpo

✅ 📦 Instalação e Uso
🔧 Pré-requisitos

.NET SDK 8

Chave da OpenAI (exportada via variável de ambiente)

Visual Studio ou VS Code

📘 1. Clone o projeto
git clone https://github.com/dcair2024/PersonalStylistIA

📗 2. Configure a variável de ambiente da OpenAI
setx OPENAI_API_KEY "SUA_CHAVE_AQUI"


✅ O projeto NÃO contém nenhuma chave sensível.
✅ A API Key é carregada via env:OPENAI_API_KEY no appsettings.json.

📙 3. Rode o projeto
dotnet run


Acesse em:
👉 http://localhost:5000

ou
👉 https://localhost:7000

✅ 📁 Estrutura do Projeto
PersonalStylistIA/
├── Pages/
│   ├── Account/ (Login, Register, Logout)
│   ├── Recommendations/ (IA Textual)
│   ├── Prompt.cshtml (IA Visual)
│   ├── Shared/ (_Layout)
│   └── Index / Privacy
├── Services/
│   ├── OpenAIImageService.cs
│   ├── OpenAITextService.cs
│   └── MockOpenAITextService.cs
├── Models/
├── wwwroot/
│   ├── css/
│   └── js/
└── appsettings.json

✅ 📜 Histórico da Sprint Final

O projeto foi conduzido seguindo Scrum:

✅ Sprint 05 – FT-017

Módulo IA Textual

UI completa

Backend integrado

QA aprovado

✅ Sprint 06 – FT-020

Cadastro de lojas (ficcional – removido da release final)

Correções de imagem

Ajustes UI

Correção de segurança no CRUD

✅ 🎯 Status Final

✅ Projeto finalizado
✅ IA funcional
✅ Layout unificado
✅ Zoom restaurado
✅ Autenticação estável
✅ Sem chaves expostas
✅ Readme pronto para portfólio

✅ 📞 Contato

Se quiser conversar sobre o projeto ou oportunidades:

📧 Email: davi.dev@gmail.com
🔗 LinkedIn: https://www.linkedin.com/in/davi-santana-cairo-797a38141/
*
🐙 GitHub: https://github.com/dcair2024

⭐ Gostou do projeto?

Se este repositório te ajudou de alguma forma:

👉 Deixe uma estrela ⭐ no GitHub!
Isso me ajuda a crescer como dev e apoiar novos projetos!
