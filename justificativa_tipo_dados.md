# Justificativa da Modelagem de Dados

## Tabela: `usuarios`

Armazena as informa��es dos clientes do sistema.

* **`id`** (`INT`): Escolhido por ser o modelo padr�o para chaves prim�rias auto-incrementais em SQL, garantindo um identificador �nico e eficiente.
* **`nome`** (`VARCHAR(255) NOT NULL`): `VARCHAR(255)` � um tamanho padr�o e seguro para nomes completos, oferecendo flexibilidade sem desperdi�ar espa�o.
* **`email`** (`VARCHAR(255) NOT NULL UNIQUE`): Utilizado por ser uma representa��o adequada para strings, com o constraint `UNIQUE` para garantir que cada e-mail seja �nico no sistema.
* **`prct_corretagem`** (`DECIMAL(5, 4) NOT NULL`): `DECIMAL` � a escolha correta para valores financeiros ou percentuais, evitando erros de arredondamento. A precis�o `(5, 4)` permite armazenar percentuais com alta granularidade (ex: 0.0050 para 0.50%).

---

## Tabela: `ativos`

Funciona como um cat�logo de todos os ativos negoci�veis na plataforma.

* **`id`** (`INT`): Padr�o para chaves prim�rias.
* **`cd`** (`VARCHAR(15)`): O ticker do ativo (ex: `ITSA4`) � um c�digo alfanum�rico. `VARCHAR` � o tipo apropriado, e o tamanho 15 � suficiente para cobrir os padr�es da B3.
* **`nome`** (`VARCHAR(255) NOT NULL`): Armazena o nome completo da empresa ou fundo, sendo `VARCHAR(255)` um tamanho adequado.

---

## Tabela: `operacoes`

Registra todas as transa��es de compra e venda de ativos.

* **`id`** (`INT`): Chave prim�ria da tabela.
* **`usr_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `usuarios`, devendo ser do mesmo tipo.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`, devendo ser do mesmo tipo.
* **`qtd`** (`INT`): Representa a quantidade de ativos em uma opera��o, que � sempre um n�mero inteiro.
* **`prc_unt`** (`DECIMAL(18,2)`): Necess�rio para precis�o financeira, representando o pre�o unit�rio do ativo no momento da transa��o.
* **`tipo`** (`TINYINT`): Escolha otimizada para representar valores fixos (0 para Venda, 1 para Compra), alinhada ao uso de `Enum` no C# e mais eficiente em armazenamento e performance que uma `string`.
* **`corretagem`** (`DECIMAL(10, 2)`): Representa um valor monet�rio final (a taxa de corretagem), sendo ideal o uso de `DECIMAL` com duas casas decimais.
* **`dt_hr`** (`DATETIME`): Tipo padr�o e apropriado para armazenar data e hora com precis�o.

---

## Tabela: `cotacoes`

Armazena o hist�rico de pre�os de mercado dos ativos.

* **`id`** (`INT`): Chave prim�ria da tabela.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`.
* **`prc_unt`** (`DECIMAL(18,2)`): Utilizado para registrar o pre�o de mercado com a precis�o financeira necess�ria.
* **`dt_hr`** (`DATETIME`): Armazena o momento exato em que a cota��o foi registrada.

---

## Tabela: `posicoes`

Consolida a carteira de cada cliente, mostrando seus ativos e resultados.

* **`id`** (`INT`): Chave prim�ria da tabela.
* **`usr_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `usuarios`.
* **`atv_id`** (`INT`): Chave estrangeira que referencia o `id` da tabela `ativos`.
* **`qtd`** (`INT`): A quantidade atual de ativos na posi��o do cliente, sendo um n�mero inteiro.
* **`prc_medio`** (`DECIMAL(18, 2)`): Representa o pre�o m�dio de aquisi��o, exigindo a precis�o de um `DECIMAL` para c�lculos financeiros corretos.
* **`pl`** (`DECIMAL(18, 2)`): Armazena o Lucro ou Preju�zo (Profit & Loss), um valor financeiro final que se beneficia da precis�o e do formato monet�rio de `DECIMAL` com duas casas.
