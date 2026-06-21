# Serviços Técnicos

Atividade Prática IV - Programação Orientada a Objetos II

## Cenário escolhido

Cenário 02 - Uma empresa presta serviços técnicos de manutenção, instalação e suporte.

## Descrição do projeto

Este projeto é um sistema em C# Console para controle de ordens de serviço de uma empresa de serviços técnicos.
O sistema permite cadastrar clientes, cadastrar técnicos, abrir ordens de serviço, executar ordens e finalizar ordens com valor a ser pago.

## Funcionalidades

* Cadastrar clientes
* Cadastrar técnicos
* Abrir ordem de serviço
* Executar ordem de serviço
* Finalizar ordem de serviço com valor a pagar
* Listar clientes
* Listar técnicos
* Listar ordens de serviço

## Tecnologias utilizadas

* C#
* Console App
* SQL Server LocalDB
* Microsoft.Data.SqlClient
* Programação Orientada a Objetos

## Banco de dados

O banco de dados está separado no arquivo:
`CreateDatabase.sql`
Para criar o banco, execute o arquivo `CreateDatabase.sql` no SQL Server usando a conexão:
`(localdb)\MSSQLLocalDB`
O banco criado se chama:
`ServicosTecnicosDB`

## Como executar o projeto

1. Abrir o arquivo `Serviços Tecnicos.sln` no Visual Studio.
2. Restaurar os pacotes NuGet, se necessário.
3. Executar o arquivo `CreateDatabase.sql` no SQL Server.
4. Rodar o projeto pelo Visual Studio.

## Estrutura do projeto

* `Models`: classes principais do sistema.
* `DAL`: classes responsáveis pela comunicação com o banco de dados.
* `Services`: regras e fluxo do sistema.
* `Program.cs`: inicialização do sistema.
* `CreateDatabase.sql`: script separado para criação do banco de dados.

## Fluxo do sistema

1. O cliente é cadastrado.
2. O técnico é cadastrado.
3. O cliente abre uma ordem de serviço.
4. O técnico executa a ordem.
5. A ordem é finalizada com o valor a ser pago.

## Tipos de serviço

* Manutenção: R$ 150,00
* Instalação: R$ 200,00
* Suporte: R$ 100,00

## Observação

O projeto foi desenvolvido com foco acadêmico, utilizando conceitos de Programação Orientada a Objetos e acesso a banco de dados SQL Server.
