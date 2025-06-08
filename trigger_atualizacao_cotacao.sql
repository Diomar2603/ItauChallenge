DELIMITER $$

CREATE TRIGGER trg_update_pl_nova_cotacao
AFTER INSERT ON cotacoes
FOR EACH ROW
BEGIN
    UPDATE posicoes
    SET
        pl = (NEW.prc_unt - posicao.prc_medio) * pos.pos_qtd
    WHERE
        pos.ast_id = NEW.ast_id;
END$$

DELIMITER ;