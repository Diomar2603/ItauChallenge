SELECT
    id,
    qtd,
    prc_unt,
    tipo,
    corretagem,
    dt_hr
FROM
    operacoes
WHERE
        usr_id =        
    AND atv_id = 
    AND dt_hr >= DATE_SUB(NOW(), INTERVAL 30 DAY);