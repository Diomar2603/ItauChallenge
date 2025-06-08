-- SELECIONA O BANCO DE DADOS
USE itau_challenge_db;

-- Desativa verificações de chave estrangeira para permitir a inserção em qualquer ordem
SET FOREIGN_KEY_CHECKS=0;

-- Limpa os dados das tabelas antes de inserir novos para evitar duplicatas
TRUNCATE TABLE dividendos;
TRUNCATE TABLE posicoes;
TRUNCATE TABLE cotacoes;
TRUNCATE TABLE operacoes;
TRUNCATE TABLE ativos;
TRUNCATE TABLE usuarios;

SET FOREIGN_KEY_CHECKS=1;

INSERT INTO usuarios (nome, email, prct_corretagem) VALUES
('João Silva', 'joao.silva@email.com', 0.0050),
('Maria Oliveira', 'maria.oliveira@email.com', 0.0025),
('Carlos Pereira', 'carlos.pereira@email.com', 0.0100),
('Ana Costa', 'ana.costa@email.com', 0.0050),
('Pedro Santos', 'pedro.santos@email.com', 0.0010),
('Sofia Almeida', 'sofia.almeida@email.com', 0.0075),
('Lucas Ferreira', 'lucas.ferreira@email.com', 0.0050),
('Juliana Lima', 'juliana.lima@email.com', 0.0020),
('Mateus Ribeiro', 'mateus.ribeiro@email.com', 0.0150),
('Laura Gonçalves', 'laura.goncalves@email.com', 0.0050);

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

INSERT INTO cotacoes (atv_id, prc_unt, dt_hr)
SELECT
    FLOOR(1 + RAND() * 10) AS atv_id, 
    ROUND(RAND() * 200 + 5, 2) AS prc_unt, 
    DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 365 * 24 * 60) MINUTE) AS dt_hr 
FROM
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5) AS a,
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5) AS b,
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4) AS c;


INSERT INTO operacoes (usr_id, atv_id, qtd, prc_unt, tipo, corretagem, dt_hr)
SELECT
    u.id AS usr_id,
    FLOOR(1 + RAND() * 10) AS atv_id, 
    (FLOOR(1 + RAND() * 10)) * 100 AS qtd,
    ROUND(RAND() * 200 + 5, 2) AS prc_unt,
    FLOOR(RAND() * 2) AS tipo, 
    ( (FLOOR(1 + RAND() * 10)) * 100 * ROUND(RAND() * 200 + 5, 2) * u.prct_corretagem ) AS corretagem,
    DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 365 * 24 * 60) MINUTE) AS dt_hr
FROM
    usuarios u
CROSS JOIN
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5) AS dummy_ops;


INSERT INTO posicoes (usr_id, atv_id, qtd, prc_medio, pl) VALUES
(1, 1, 500, 28.50, 1250.00),
(1, 3, 1000, 25.10, -500.50),
(2, 2, 200, 65.80, 3200.00),
(3, 5, 800, 15.20, 800.00),
(3, 4, 300, 12.90, -150.75),
(4, 7, 100, 35.40, 450.00),
(5, 10, 2000, 48.75, 5000.20),
(6, 8, 600, 44.30, 1230.00),
(7, 9, 700, 10.50, -350.00),
(8, 1, 100, 30.15, 200.00),
(8, 6, 400, 3.50, 150.00),
(9, 2, 50, 70.00, -250.00),
(9, 7, 150, 38.00, 600.00),
(10, 3, 500, 27.80, 1100.00),
(10, 5, 100, 14.50, 50.00);

INSERT INTO dividendos (usr_id, atv_id, vlr_cota, qtd_base, dt_pgto)
SELECT
    FLOOR(1 + RAND() * 10) AS usr_id, 
    FLOOR(1 + RAND() * 10) AS atv_id, 
    ROUND(RAND() * 2 + 0.1, 4) AS vlr_cota, 
    (FLOOR(1 + RAND() * 10)) * 100 AS qtd_base, 
    DATE_SUB(NOW(), INTERVAL FLOOR(RAND() * 730) DAY) AS dt_pgto 
FROM
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5) AS a,
    (SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4) AS b;
