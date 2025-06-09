# Justificativa da Modelagem de Dados

## Tabela: `usuarios`

Armazena as informações dos clientes do sistema.

* **`id`** (`INT`): Escolhido por ser o modelo padrão para chaves primárias auto-incrementais em SQL, garantindo um identificador único e eficiente.
* **`nome`** (`VARCHAR(255) NOT NULL`): `VARCHAR(255)` é um tamanho padrão e seguro para nomes completos, oferecendo flexibilidade sem desperdiçar espaço.
* **`email`** (`VARCHAR(255) NOT NULL UNIQUE`): Utilizado por ser uma representação adequada para strings, com o constraint `UNIQUE` para garantir que cada e-mail seja único no sistema.
* **`prct_corretagem`** (`DECIMAL(5, 4) NOT NULL`): `DECIMAL` é a escolha correta para valores financeiros ou percentuais, evitando erros de arredondamento. A precisão `(5, 4)` permite armazenar percentuais com alta granularidade (ex: 0.0050 para 0.50%).

---

## Tabela: `ativos`

Funciona como um catálogo de todos os ativos negociáveis na plataforma.

* **`id`** (`INT`): Padrão para chaves primárias.
* **`cd`** (`VARCHAR(15)`): O ticker do ativo (ex: `ITSA4`) é um código alfanumérico. `VARCHAR` é o tipo apropriado, e o tamanho 15 é suficiente para cobrir os padrões da B3.
* **`nome`** (`VARCHAR(255) NOT NULL`): Armazena o nome completo da empresa ou fundo, sendo `VARCHAR(255)` um tamanho adequado.

---

## Tabela: `operacoes`

Registra todas as transações de compra e venda de ativos.

* **`id`** (`INT`): Chave primária da tabela.
* **`usr_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `usuarios`, devendo ser do mesmo tipo.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`, devendo ser do mesmo tipo.
* **`qtd`** (`INT`): Representa a quantidade de ativos em uma operação, que é sempre um número inteiro.
* **`prc_unt`** (`DECIMAL(18,2)`): Necessário para precisão financeira, representando o preço unitário do ativo no momento da transação.
* **`tipo`** (`TINYINT`): Escolha otimizada para representar valores fixos (0 para Venda, 1 para Compra), alinhada ao uso de `Enum` no C# e mais eficiente em armazenamento e performance que uma `string`.
* **`corretagem`** (`DECIMAL(10, 2)`): Representa um valor monetário final (a taxa de corretagem), sendo ideal o uso de `DECIMAL` com duas casas decimais.
* **`dt_hr`** (`DATETIME`): Tipo padrão e apropriado para armazenar data e hora com precisão.

---

## Tabela: `cotacoes`

Armazena o histórico de preços de mercado dos ativos.

* **`id`** (`INT`): Chave primária da tabela.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`.
* **`prc_unt`** (`DECIMAL(18,2)`): Utilizado para registrar o preço de mercado com a precisão financeira necessária.
* **`dt_hr`** (`DATETIME`): Armazena o momento exato em que a cotação foi registrada.

---

## Tabela: `posicoes`

Consolida a carteira de cada cliente, mostrando seus ativos e resultados.

* **`id`** (`INT`): Chave primária da tabela.
* **`usr_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `usuarios`.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`.
* **`qtd`** (`INT`): A quantidade atual de ativos na posição do cliente, sendo um número inteiro.
* **`prc_medio`** (`DECIMAL(18, 2)`): Representa o preço médio de aquisição, exigindo a precisão de um `DECIMAL` para cálculos financeiros corretos.
* **`pl`** (`DECIMAL(18, 2)`): Armazena o Lucro ou Prejuízo (Profit & Loss), um valor financeiro final que se beneficia da precisão e do formato monetário de `DECIMAL` com duas casas.
