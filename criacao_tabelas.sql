--  CRIAÇÃO DO BANCO DE DADOS
---- Apaga o banco de dados se ele já existir para garantir uma iativosnstalação limpa
DROP DATABASE IF EXISTS itau_challenge_db;

---- Cria o novo banco de dados
CREATE DATABASE itau_challenge_db;

---- Seleciona o banco de dados recém-criado para uso
USE itau_challenge_db;


-- 2. CRIAÇÃO DAS TABELAS
---- Primeiro, as tabelas que não dependem de outras (usuarios, ativos).

---- Tabela de Usuários
CREATE TABLE usuarios (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    prct_corretagem DECIMAL(5, 4) NOT NULL COMMENT 'Percentual de corretagem. Ex: 0.0050 para 0.50%'
);

---- Tabela de Ativos
CREATE TABLE ativos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    cdg VARCHAR(15) NOT NULL UNIQUE COMMENT 'Código de negociação do ativo.',
    nome VARCHAR(255) NOT NULL
);

---- Tabela de Operações
---- Depende de `Usuarios`(usr) e `Ativos`(atv)
CREATE TABLE operacoes (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    usr_id INT NOT NULL,
    atv_id INT NOT NULL,
    qtd INT NOT NULL,
    prc_unt DECIMAL(18, 2) NOT NULL,
    tipo TINYINT NOT NULL COMMENT 'Tipo da operação: 0 para Venda, 1 para Compra',
    corretagem DECIMAL(10, 2) NOT NULL DEFAULT 0,
    dt_hr DATETIME(3) NOT NULL,
    FOREIGN KEY (usr_id) REFERENCES usuarios(id),
    FOREIGN KEY (atv_id) REFERENCES ativos(id)
);

---- Tabela de Cotações
---- Depende de `Ativos` (atv)
CREATE TABLE cotacoes (
    id BIGINT PRIMARY KEY AUTO_INCREMENT,
    atv_id INT NOT NULL,
    prc_unt DECIMAL(18, 2) NOT NULL,
    dt_hr DATETIME(3) NOT NULL,
    FOREIGN KEY (atv_id) REFERENCES ativos(id)
);

---- Tabela de Posição Consolidada
---- Depende de `Usuarios`(usr) e `Ativos`(atv)
CREATE TABLE posicoes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usr_id INT NOT NULL,
    atv_id INT NOT NULL,
    qtd INT NOT NULL,
    prc_medio DECIMAL(18, 2) NOT NULL COMMENT 'Preço médio de aquisição do ativo',
    pl DECIMAL(18, 2) NOT NULL COMMENT 'Lucro e Prejuízo (P&L)',
    FOREIGN KEY (usr_id) REFERENCES usuarios(id),
    FOREIGN KEY (atv_id) REFERENCES ativos(id),
    UNIQUE (usr_id, atv_id)
);

---- Tabela de  Dividendos
---- Depende de `Usuarios` (usr) e `Ativos` (atv)
CREATE TABLE dividendos (
    id INT PRIMARY KEY AUTO_INCREMENT,
    usr_id INT NOT NULL,
    atv_id INT NOT NULL,
    vlr_cota DECIMAL(18, 2) NOT NULL,
    qtd_base INT NOT NULL COMMENT 'Quantidade de ativos na data de direito ao provento',
    dt_pgto DATE NOT NULL,
    FOREIGN KEY (usr_id) REFERENCES usuarios(id),
    FOREIGN KEY (atv_id) REFERENCES ativos(id)
);