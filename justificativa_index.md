### Otimização de Consulta: Índice Composto

Para otimizar a consulta por *"todas as operações de um usuário em um determinado ativo nos últimos 30 dias"*, a abordagem mais eficiente é a criação de um **índice composto** na tabela `operacoes`.

A escolha e a ordem das colunas no índice são estratégicas para o **desempenho**:

1.  Primeiro, inserimos as colunas `usr_id` e `atv_id`, que são usadas para filtros de correspondência exata.
2.  Em seguida, adicionamos a coluna `dt_hr`, que servirá para o filtro de período (range scan).

Dessa forma, o banco de dados localiza rapidamente o conjunto de dados pelo usuário e pelo ativo e, somente sobre esse resultado já reduzido, aplica o filtro final dos últimos 30 dias, garantindo **máxima eficiência** na consulta.

**Exemplo do Comando SQL:**
```sql
CREATE INDEX idx_operacoes_usuario_ativo_data 
ON operacoes (usr_id, atv_id, dt_hr);