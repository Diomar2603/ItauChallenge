# Escalabilidade e Performance

---

## Auto-Scaling Horizontal

O auto-scaling horizontal � a pr�tica de adicionar ou remover automaticamente inst�ncias (c�pias) da aplica��o em resposta � demanda de tr�fego. Em vez de tornar uma �nica m�quina mais forte (scaling vertical), distribu�mos a carga entre v�rias m�quinas.

Para aplicar esta estrat�gia ao servi�o, seguimos estes passos:

1.  **Containeriza��o:** A aplica��o deve ser empacotada em um cont�iner **Docker**. Isso a torna um artefato port�til e f�cil de replicar, garantindo que cada nova inst�ncia seja id�ntica �s outras.

2.  **Orquestra��o:** Utilizamos uma ferramenta de orquestra��o como o **Kubernetes (K8S)** ou servi�os nativos de nuvem (AWS ECS, Azure App Service) para gerenciar os cont�ineres. O orquestrador � respons�vel por iniciar, parar e monitorar a sa�de das inst�ncias.

3.  **Defini��o de M�tricas:** O auto-scaling � acionado por m�tricas. As regras s�o configuradas no orquestrador com base em limites predefinidos:
    * **Uso de CPU (mais comum):** Ex: "Se o uso m�dio de CPU exceder 75%, adicione uma nova inst�ncia."
    * **Uso de Mem�ria:** �til para aplica��es que consomem muita mem�ria.
    * **Tamanho de Fila:** Para servi�os de background, como o consumidor Kafka, a escala pode ser baseada no n�mero de mensagens na fila de espera.

4.  **Configura��o:** No Kubernetes, isso � feito atrav�s de um recurso chamado `HorizontalPodAutoscaler` (HPA), onde se define a m�trica, o limiar e o n�mero m�nimo/m�ximo de inst�ncias desejadas.

---

## Comparativo de Estrat�gias de Balanceamento de Carga

### Estrat�gia: Round Robin

� o m�todo mais simples de balanceamento de carga. Ele distribui as requisi��es de forma sequencial e c�clica para cada inst�ncia dispon�vel.

* **Vantagens:**
    * Extremamente simples de implementar e entender.
    * Distribui o tr�fego de forma matematicamente uniforme.
* **Desvantagens:**
    * N�o considera a carga ou a sa�de atual de cada inst�ncia.
    * Pode continuar enviando tr�fego para uma inst�ncia que est� lenta ou sobrecarregada, piorando a performance geral.

### Estrat�gia: Baseado em Lat�ncia (Least Response Time / Least Connections)

Esta � uma abordagem mais inteligente e adaptativa, onde o balanceador de carga monitora ativamente as inst�ncias.

* **Vantagens:**
    * **Adaptativo:** Envia novas requisi��es para a inst�ncia que est� respondendo mais r�pido ou que tem o menor n�mero de conex�es ativas.
    * **Resiliente:** Desvia o tr�fego automaticamente de inst�ncias que est�o lentas ou com problemas, melhorando a estabilidade e a experi�ncia do usu�rio.
    * Otimiza o uso dos recursos, pois direciona a carga para onde ela pode ser processada mais rapidamente.
* **Desvantagens:**
    * Ligeiramente mais complexo de configurar, pois exige capacidade de monitoramento no balanceador.

### Conclus�o e Recomenda��o

Para um sistema de alta performance e grande volume como o proposto no desafio, a estrat�gia de balanceamento **baseada em lat�ncia � superior**. Enquanto o Round Robin oferece uma distribui��o simples, o m�todo baseado em lat�ncia garante uma distribui��o **eficiente e resiliente**, o que � fundamental para manter a estabilidade e a velocidade da aplica��o sob forte demanda.