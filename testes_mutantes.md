## Teste de Muta��o

### Conceito e Import�ncia

O **teste de muta��o** � uma t�cnica avan�ada utilizada para avaliar e garantir a qualidade da sua su�te de testes unit�rios. A ideia central n�o � testar o c�digo da aplica��o em si, mas sim **testar os pr�prios testes**.

O processo funciona da seguinte maneira:
1.  Pequenas altera��es sintaticamente v�lidas, chamadas **muta��es**, s�o introduzidas automaticamente no seu c�digo-fonte. Por exemplo, um operador `+` � trocado por `-`, ou um `>` por `>=`.
2.  Os testes unit�rios existentes s�o executados novamente sobre o c�digo mutante.
3.  Se os testes falharem, diz-se que eles **"mataram o mutante"**. Isso � um bom sinal, pois indica que sua su�te de testes � robusta o suficiente para detectar essa pequena altera��o.
4.  Se os testes continuarem a passar, o **"mutante sobreviveu"**. Isso aponta uma fraqueza ou uma lacuna na sua su�te de testes, que n�o foi capaz de identificar a mudan�a no comportamento do c�digo.

> A qualidade de uma bateria de testes � medida pela sua capacidade de detectar falhas no c�digo � inclusive falhas introduzidas de prop�sito.

---

### Exemplo Pr�tico

Vamos aplicar o conceito ao m�todo `CalcularMediaPonderada` da nossa classe `CalculoFinanceiroService`.

#### C�digo Original
O c�lculo do custo total � feito pela multiplica��o de `Quantidade` por `PrecoUnitario`.

```csharp
// ...
custoTotal += compra.Quantidade * compra.PrecoUnitario;
// ...

### C�digo para teste mutante
Trocar o operador * foi trocado por -

```csharp
// ...
custoTotal += compra.Quantidade - compra.PrecoUnitario; 
// ...