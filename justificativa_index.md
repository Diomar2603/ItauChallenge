### Otimiza��o de Consulta: �ndice Composto

Para otimizar a consulta por *"todas as opera��es de um usu�rio em um determinado ativo nos �ltimos 30 dias"*, a abordagem mais eficiente � a cria��o de um **�ndice composto** na tabela `operacoes`.

A escolha e a ordem das colunas no �ndice s�o estrat�gicas para o **desempenho**:

1.  Primeiro, inserimos as colunas `usr_id` e `atv_id`, que s�o usadas para filtros de correspond�ncia exata.
2.  Em seguida, adicionamos a coluna `dt_hr`, que servir� para o filtro de per�odo (range scan).

Dessa forma, o banco de dados localiza rapidamente o conjunto de dados pelo usu�rio e pelo ativo e, somente sobre esse resultado j� reduzido, aplica o filtro final dos �ltimos 30 dias, garantindo **m�xima efici�ncia** na consulta.

**Exemplo do Comando SQL:**
```sql
CREATE INDEX idx_operacoes_usuario_ativo_data 
ON operacoes (usr_id, atv_id, dt_hr);