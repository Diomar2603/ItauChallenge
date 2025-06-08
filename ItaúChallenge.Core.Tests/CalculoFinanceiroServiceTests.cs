using System;
using System.Collections.Generic;
using Xunit;
using ItauChallenge.Core.Service;
using ItauChallenge.Core.Entities;
using System.Linq; 

public class CalculoFinanceiroServiceTests
{
    private readonly CalculoFinanceiroService _servico;

    public CalculoFinanceiroServiceTests()
    {
        _servico = new CalculoFinanceiroService();
    }

    [Fact]
    public void CalcularMediaPonderada_ComMultiplasComprasDoMesmoAtivo_DeveRetornarMediaPonderadaCorreta()
    {
        var compras = new Operacao[]
        {
            new Operacao { Id = 1, AtivoId = 123, Quantidade = 10, PrecoUnitario = 10.00m },
            new Operacao { Id = 2, AtivoId = 123, Quantidade = 20, PrecoUnitario = 25.00m }
        };

        var resultado = _servico.CalcularMediaPonderadaAtivo(compras);

        Assert.Equal(20.00m, resultado);
    }

    [Fact]
    public void CalcularMediaPonderada_ComListaVazia_DeveRetornarZero()
    {
        // Arrange
        var compras = Array.Empty<Operacao>();

        // Act
        var resultado = _servico.CalcularMediaPonderadaAtivo(compras);

        // Assert
        Assert.Equal(0, resultado);
    }

    [Fact]
    public void CalcularMediaPonderada_ComListaNula_DeveLancarArgumentNullException()
    {
        Operacao[] compras = null;

        var exception = Assert.Throws<ArgumentNullException>(() => _servico.CalcularMediaPonderadaAtivo(compras));
        Assert.Equal("compras", exception.ParamName);
        Assert.Contains("A lista de compras não pode ser nula.", exception.Message);
    }

    [Fact]
    public void CalcularMediaPonderada_ComOperacoesDeAtivosDiferentes_DeveLancarArgumentExceptionComDetalhes()
    {
        var compras = new Operacao[]
        {
            new Operacao { Id = 1, AtivoId = 123, Quantidade = 10, PrecoUnitario = 10.00m },
            new Operacao { Id = 2, AtivoId = 999, Quantidade = 5, PrecoUnitario = 50.00m }
        };

        var exception = Assert.Throws<ArgumentException>(() => _servico.CalcularMediaPonderadaAtivo(compras));
        Assert.Contains("A lista de compras deve conter operações de apenas um ativo", exception.Message);
        Assert.Contains("123", exception.Message);
        Assert.Contains("999", exception.Message);
    }

    [Fact]
    public void CalcularMediaPonderada_ComOperacaoDePrecoZero_DeveCalcularMediaCorretamente()
    {
        var compras = new Operacao[]
        {
            new Operacao { Id = 1, AtivoId = 123, Quantidade = 10, PrecoUnitario = 20.00m }, 
            new Operacao { Id = 2, AtivoId = 123, Quantidade = 10, PrecoUnitario = 0.00m }   
        };

        var resultado = _servico.CalcularMediaPonderadaAtivo(compras);

        Assert.Equal(10.00m, resultado);
    }

    [Fact]
    public void CalcularMediaPonderada_ComPrecoUnitarioNegativo_DeveLancarArgumentException()
    {
        var operacaoComPrecoInvalido = new Operacao { Id = 5, AtivoId = 123, Quantidade = 10, PrecoUnitario = -10.00m };
        var compras = new Operacao[] { operacaoComPrecoInvalido };

        var exception = Assert.Throws<ArgumentException>(() => _servico.CalcularMediaPonderadaAtivo(compras));
        Assert.Contains($"A operação com ID {operacaoComPrecoInvalido.Id} tem um preço unitário inválido", exception.Message);
    }

    [Fact]
    public void CalcularMediaPonderada_ComQuantidadeNegativa_DeveLancarArgumentException()
    {
        var operacaoComQuantidadeInvalida = new Operacao { Id = 3, AtivoId = 123, Quantidade = -10, PrecoUnitario = 10.00m };
        var compras = new Operacao[] { operacaoComQuantidadeInvalida };

        var exception = Assert.Throws<ArgumentException>(() => _servico.CalcularMediaPonderadaAtivo(compras));
        Assert.Contains($"A operação com ID {operacaoComQuantidadeInvalida.Id} tem uma quantidade inválida", exception.Message);
    }


    [Fact]
    public void CalcularMediaPonderada_ComQuantidadeZero_DeveLancarArgumentException()
    {
        var operacaoComQuantidadeInvalida = new Operacao { Id = 4, AtivoId = 123, Quantidade = 0, PrecoUnitario = 10.00m };
        var compras = new Operacao[]
        {
            new Operacao { Id = 1, AtivoId = 123, Quantidade = 10, PrecoUnitario = 20.00m },
            operacaoComQuantidadeInvalida
        };

        var exception = Assert.Throws<ArgumentException>(() => _servico.CalcularMediaPonderadaAtivo(compras));
        Assert.Contains($"A operação com ID {operacaoComQuantidadeInvalida.Id} tem uma quantidade inválida", exception.Message);
    }

}