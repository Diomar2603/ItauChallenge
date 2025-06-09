USE itau_challenge_db;

SET FOREIGN_KEY_CHECKS=0;

TRUNCATE TABLE dividendos;
TRUNCATE TABLE posicoes;
TRUNCATE TABLE cotacoes;
TRUNCATE TABLE operacoes;
TRUNCATE TABLE ativos;
TRUNCATE TABLE usuarios;

SET FOREIGN_KEY_CHECKS=1;

INSERT INTO usuarios (nome, email, prct_corretagem) VALUES
('João Silva', 'joao.silva@email.com', 0.0050),
('Maria Oliveira', 'maria.oliveira@email.com', 0.0000),
('Carlos Pereira', 'carlos.pereira@email.com', 0.0025),
('Ana Costa', 'ana.costa@email.com', 0.0000),
('Pedro Santos', 'pedro.santos@email.com', 0.0010),
('Sofia Almeida', 'sofia.almeida@email.com', 0.0050),
('Lucas Ferreira', 'lucas.ferreira@email.com', 0.0000),
('Juliana Lima', 'juliana.lima@email.com', 0.0020),
('Mateus Ribeiro', 'mateus.ribeiro@email.com', 0.0100),
('Laura Gonçalves', 'laura.goncalves@email.com', 0.0000);

INSERT INTO ativos (cdg, nome) VALUES
('PETR4', 'Petrobras PN'),
('VALE3', 'Vale ON'),
('ITUB4', 'Itaú Unibanco PN'),
('BBDC4', 'Bradesco PN'),
('ABEV3', 'Ambev ON'),
('MGLU3', 'Magazine Luiza ON'),
('WEGE3', 'WEG ON'),
('SUZB3', 'Suzano ON'),
('GGBR4', 'Gerdau PN'),
('B3SA3', 'B3 ON');

INSERT INTO cotacoes (atv_id, prc_unt, dt_hr) VALUES
(1, 38.50, '2024-10-05 15:00:00'), (1, 39.80, '2025-02-10 11:00:00'), (1, 40.15, '2025-05-20 17:00:00'),
(2, 61.20, '2024-11-15 10:00:00'), (2, 65.50, '2025-03-22 14:00:00'), (2, 62.70, '2025-05-28 16:00:00'),
(3, 32.80, '2024-09-20 12:00:00'), (3, 34.00, '2025-01-30 13:00:00'), (3, 33.50, '2025-05-15 18:00:00'),
(4, 13.50, '2025-02-01 10:30:00'), (4, 12.80, '2025-04-10 15:30:00'),
(5, 12.10, '2024-12-10 11:00:00'), (5, 12.50, '2025-03-18 16:00:00'),
(6, 2.10, '2025-01-15 14:45:00'), (6, 1.80, '2025-04-25 12:00:00'),
(7, 39.50, '2024-10-22 10:00:00'), (7, 41.20, '2025-03-05 17:00:00'),
(8, 50.30, '2024-11-25 15:00:00'), (8, 55.80, '2025-05-09 11:30:00'),
(9, 21.80, '2024-09-12 10:00:00'), (9, 19.50, '2025-04-01 14:00:00'),
(10, 11.40, '2025-01-20 16:00:00'), (10, 10.90, '2025-05-10 12:00:00');

INSERT INTO operacoes (usr_id, atv_id, qtd, prc_unt, tipo, corretagem, dt_hr)
SELECT u.id, o.atv_id, o.qtd, o.prc_unt, o.tipo,
       (o.qtd * o.prc_unt * u.prct_corretagem) AS corretagem,
       o.dt_hr
FROM usuarios u
JOIN (
    SELECT 1 AS usr_id, 1 AS atv_id, 300 AS qtd, 38.50 AS prc_unt, 1 AS tipo, '2024-10-05 15:01:00' AS dt_hr UNION ALL
    SELECT 1, 3, 500, 32.80, 1, '2024-09-20 12:01:00' UNION ALL
    SELECT 1, 1, 100, 40.15, 0, '2025-05-20 17:01:00' UNION ALL
    SELECT 2, 2, 200, 61.20, 1, '2024-11-15 10:01:00' UNION ALL
    SELECT 3, 5, 1000, 12.10, 1, '2024-12-10 11:01:00' UNION ALL
    SELECT 3, 5, 200, 12.50, 0, '2025-03-18 16:01:00' UNION ALL
    SELECT 4, 7, 100, 39.50, 1, '2024-10-22 10:01:00' UNION ALL
    SELECT 4, 6, 2000, 2.10, 1, '2025-01-15 14:46:00' UNION ALL
    SELECT 9, 9, 700, 21.80, 1, '2024-09-12 10:01:00' UNION ALL
    SELECT 9, 9, 700, 19.50, 0, '2025-04-01 14:01:00'
) AS o ON u.id = o.usr_id;

INSERT INTO posicoes (usr_id, atv_id, qtd, prc_medio, pl)
WITH
PosicaoAtual AS (
    SELECT
        usr_id,
        atv_id,
        SUM(CASE WHEN tipo = 1 THEN qtd ELSE -qtd END) AS qtd_atual,
        SUM(CASE WHEN tipo = 1 THEN qtd * prc_unt ELSE 0 END) AS custo_total_compra,
        SUM(CASE WHEN tipo = 1 THEN qtd ELSE 0 END) AS qtd_total_comprada
    FROM
        operacoes
    GROUP BY
        usr_id,
        atv_id
),
UltimaCotacao AS (
    SELECT
        atv_id,
        prc_unt AS ultimo_preco
    FROM (
        SELECT atv_id, prc_unt, ROW_NUMBER() OVER(PARTITION BY atv_id ORDER BY dt_hr DESC) as rn
        FROM cotacoes
    ) AS ranked_cotacoes
    WHERE rn = 1
)
SELECT
    p.usr_id,
    p.atv_id,
    p.qtd_atual,
    COALESCE(p.custo_total_compra / NULLIF(p.qtd_total_comprada, 0), 0) AS prc_medio,
    (uc.ultimo_preco - COALESCE(p.custo_total_compra / NULLIF(p.qtd_total_comprada, 0), 0)) * p.qtd_atual AS pl
FROM
    PosicaoAtual p
JOIN
    UltimaCotacao uc ON p.atv_id = uc.atv_id
WHERE
    p.qtd_atual > 0;