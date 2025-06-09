# Escalabilidade e Performance

---

## Auto-Scaling Horizontal

O auto-scaling horizontal é a prática de adicionar ou remover automaticamente instâncias (cópias) da aplicação em resposta à demanda de tráfego. Em vez de tornar uma única máquina mais forte (scaling vertical), distribuímos a carga entre várias máquinas.

Para aplicar esta estratégia ao serviço, seguimos estes passos:

1.  **Containerização:** A aplicação deve ser empacotada em um contêiner **Docker**. Isso a torna um artefato portátil e fácil de replicar, garantindo que cada nova instância seja idêntica às outras.

2.  **Orquestração:** Utilizamos uma ferramenta de orquestração como o **Kubernetes (K8S)** ou serviços nativos de nuvem (AWS ECS, Azure App Service) para gerenciar os contêineres. O orquestrador é responsável por iniciar, parar e monitorar a saúde das instâncias.

3.  **Definição de Métricas:** O auto-scaling é acionado por métricas. As regras são configuradas no orquestrador com base em limites predefinidos:
    * **Uso de CPU (mais comum):** Ex: "Se o uso médio de CPU exceder 75%, adicione uma nova instância."
    * **Uso de Memória:** Útil para aplicações que consomem muita memória.
    * **Tamanho de Fila:** Para serviços de background, como o consumidor Kafka, a escala pode ser baseada no número de mensagens na fila de espera.

4.  **Configuração:** No Kubernetes, isso é feito através de um recurso chamado `HorizontalPodAutoscaler` (HPA), onde se define a métrica, o limiar e o número mínimo/máximo de instâncias desejadas.

---

## Comparativo de Estratégias de Balanceamento de Carga

### Estratégia: Round Robin

É o método mais simples de balanceamento de carga. Ele distribui as requisições de forma sequencial e cíclica para cada instância disponível.

* **Vantagens:**
    * Extremamente simples de implementar e entender.
    * Distribui o tráfego de forma matematicamente uniforme.
* **Desvantagens:**
    * Não considera a carga ou a saúde atual de cada instância.
    * Pode continuar enviando tráfego para uma instância que está lenta ou sobrecarregada, piorando a performance geral.

### Estratégia: Baseado em Latência (Least Response Time / Least Connections)

Esta é uma abordagem mais inteligente e adaptativa, onde o balanceador de carga monitora ativamente as instâncias.

* **Vantagens:**
    * **Adaptativo:** Envia novas requisições para a instância que está respondendo mais rápido ou que tem o menor número de conexões ativas.
    * **Resiliente:** Desvia o tráfego automaticamente de instâncias que estão lentas ou com problemas, melhorando a estabilidade e a experiência do usuário.
    * Otimiza o uso dos recursos, pois direciona a carga para onde ela pode ser processada mais rapidamente.
* **Desvantagens:**
    * Ligeiramente mais complexo de configurar, pois exige capacidade de monitoramento no balanceador.

### Conclusão e Recomendação

Para um sistema de alta performance e grande volume como o proposto no desafio, a estratégia de balanceamento **baseada em latência é superior**. Enquanto o Round Robin oferece uma distribuição simples, o método baseado em latência garante uma distribuição **eficiente e resiliente**, o que é fundamental para manter a estabilidade e a velocidade da aplicação sob forte demanda.