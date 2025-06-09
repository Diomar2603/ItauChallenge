## Teste de Mutação

### Conceito e Importância

O **teste de mutação** é uma técnica avançada utilizada para avaliar e garantir a qualidade da sua suíte de testes unitários. A ideia central não é testar o código da aplicação em si, mas sim **testar os próprios testes**.

O processo funciona da seguinte maneira:
1.  Pequenas alterações sintaticamente válidas, chamadas **mutações**, são introduzidas automaticamente no seu código-fonte. Por exemplo, um operador `+` é trocado por `-`, ou um `>` por `>=`.
2.  Os testes unitários existentes são executados novamente sobre o código mutante.
3.  Se os testes falharem, diz-se que eles **"mataram o mutante"**. Isso é um bom sinal, pois indica que sua suíte de testes é robusta o suficiente para detectar essa pequena alteração.
4.  Se os testes continuarem a passar, o **"mutante sobreviveu"**. Isso aponta uma fraqueza ou uma lacuna na sua suíte de testes, que não foi capaz de identificar a mudança no comportamento do código.

> A qualidade de uma bateria de testes é medida pela sua capacidade de detectar falhas no código — inclusive falhas introduzidas de propósito.

---

### Exemplo Prático

Vamos aplicar o conceito ao método `CalcularMediaPonderada` da nossa classe `CalculoFinanceiroService`.

#### Código Original
O cálculo do custo total é feito pela multiplicação de `Quantidade` por `PrecoUnitario`.

```csharp
custoTotal += compra.Quantidade * compra.PrecoUnitario;

### Código para teste mutante
Trocar o operador * foi trocado por -

custoTotal += compra.Quantidade - compra.PrecoUnitario; 
```
