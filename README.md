# Desafio Técnico Itaú Unibanco - Leandro Diomar

## Visão Geral do Projeto

Este repositório contém a solução completa para o Desafio Técnico , proposto pelo Itaú Unibanco. O projeto consiste em um sistema de controle de investimentos em renda variável, desenvolvido com uma arquitetura em camadas para garantir separação de responsabilidades, testabilidade e escalabilidade.

As principais tecnologias utilizadas foram:
* **Backend:** .NET / C#
* **API:** ASP.NET Core Web API
* **Acesso a Dados:** Entity Framework Core e MySQL
* **Testes:** xUnit
* **Mensageria:** Kafka com Docker
* **Resiliência:** Polly

---

##  Estrutura do Projeto

A solução foi organizada seguindo os princípios da Clean Architecture, dividida nas seguintes camadas:

* **`ItauChallenge.Core`**: Camada mais interna, contendo as entidades de domínio e as interfaces dos repositórios. Não possui dependências externas.
* **`ItauChallenge.Application`**: Contém a lógica de negócio, os serviços e os DTOs (Data Transfer Objects). Orquestra o fluxo de dados entre a apresentação e a infraestrutura.
* **`ItauChallenge.Infrastructure`**: Implementa o acesso a dados (repositórios com EF Core) e a comunicação com serviços externos como o Kafka.
* **`ItauChallenge.Api`**: A camada de apresentação, expondo os endpoints da API REST.
* **`ItauChallenge.WorkerService`**: Serviço de background para consumir mensagens do Kafka em tempo real.
* **`ItauChallenge.Tests`**: Projeto com os testes unitários para garantir a qualidade do código.

---

## 📝 Respostas e Justificativas

As respostas para as questões dissertativas e as justificativas técnicas solicitadas no desafio estão organizadas em arquivos `.md`.

| Atividade | Tópico | Localização no Repositório |
| :--- | :--- | :--- |
| **1** | Modelagem de Banco e Justificativas | `justificativa_tipo_dados.md` |
| **2** | Índices e Performance | `justificativa_index.md` |
| **6** | Teste Mutante | `testes_mutantes.md` |
| **9** | Escalabilidade e Performance | `escalabilidade_performance.md` |
| **10** | Documentação da API (OpenAPI) | `documentacaoApi.json` |

---

##  Maneira de efetuar os testes:

### Testes relacionados a trabalho com os dados
Geração de base de dados utilizando o arquivo `criacao_dados_ficticios.sql`

### Testes Kafka
Foram efetuados enviando mensagens utilizando o kafka-ui, em um ambiente doker gerado pelo `docker-kafka-ui.yml`, com mensagens utilizando o padrão dentro do `ItauChallenge.WorkerService\mensagensTest\mensagem1.json`

## Evidencias de teste:
### Testes unitarios
![image](https://github.com/user-attachments/assets/5da49430-859f-4652-99c1-673cca0fc7ff)

### Retorno do console referente ao topico 4
![image](https://github.com/user-attachments/assets/06bc7d61-546f-4eea-865c-653f86647aaa)

### Testes Kafka Inserir 
#### Cotações antes da mensagem:
![image](https://github.com/user-attachments/assets/3a8de846-b45c-4492-a385-0fd4baaf97dc)

#### Envio da mensagem no kafka-ui:
![image](https://github.com/user-attachments/assets/304e4ef7-07c0-4cbf-9148-3e42fe72d723)

#### Cotações após mensagem:
![image](https://github.com/user-attachments/assets/607f2276-f6b0-4e40-8fd6-046b688f55e9)

### Testes de End-points API
#### Informar um ativo e receber a última cotação.
![image](https://github.com/user-attachments/assets/90734b72-65ee-4f7f-950e-aa6750ded486)
#### Consultar o preço médio por ativo para um usuário
![image](https://github.com/user-attachments/assets/0972b6de-67f5-468d-b416-3f965854a5c2)
#### Consultar a posição de um cliente
![image](https://github.com/user-attachments/assets/efa690b0-6258-46b8-92c0-e01d9ef70f31)
![image](https://github.com/user-attachments/assets/57687531-04d3-4b79-80d2-62208c82cd8d)
#### Valor financeiro total ganho pela corretora com as corretagens
![image](https://github.com/user-attachments/assets/f556f503-2c47-483e-a181-18c9f6f30575)
####  Top 10 clientes com maiores posições, e os Top 10 clientes que mais pagaram corretagem.
![image](https://github.com/user-attachments/assets/bdc103ae-c7be-467d-871e-1189a3269fc3)
![image](https://github.com/user-attachments/assets/506f2e7d-22b4-407b-8af7-4b6db951f69f)
![image](https://github.com/user-attachments/assets/be41d0fb-282e-4e1d-ba1d-14becf8b1bf7)

##  Como Configurar e Rodar o Projeto

### Pré-requisitos

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (ou a versão utilizada no projeto)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [MySQL Server](https://dev.mysql.com/downloads/mysql/) (ou um cliente compatível)
* Um cliente Git

### 1. Clonar o Repositório
```bash
git clone https://github.com/Diomar2603/ItauChallenge.git
```

### 2. Executar projeto no Visual Studio
#### ConsoleApp
Neste caso apenas abrir a Solution no visual studio, encontrar o projeto `**ItauChallenge.ConsoleApp`** e executar o projeto

#### Projeto WorkerService
No caso do WorkerService será necessario inserir os parametros na `**appsettings.json`** do projeto, contendo suar configurações Kafka 
e do banco de dados. EX:
```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultBdConnection": "Server=localhost;Database=itau_challenge_db;User=root;Password=Lea@2023!;"
  },
  "KafkaConnectionSettings": {
    "BootstrapServers": "localhost:9092",
    "GroupId": "cotacoes-processor-group"
  },
  "AllowedHosts": "*"
}
```
Após executar o projeto executar o doker relacionado ao tópico de "Testes Kafka"
para poder testar o envio de mensagens para o consulmer.

### ItauChallenge.Api

Para executar a API é recomendado o uso de uma execução como desenvolvedor, para que possa utilizar a UI de testes do 
Swagger. Mas antes é necessario configurar como no tópico anterior o `**appsettings.json`**. EX:

```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultBdConnection": "Server=localhost;Database=itau_challenge_db;User=root;Password=Lea@2023!;"
  },
  "AllowedHosts": "*"
}
```
