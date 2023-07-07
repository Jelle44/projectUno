namespace MyApp.Domain.Tests;
using Xunit;
using MyApp.Domain;
using static MyApp.Domain.Pile;
using System.ComponentModel.DataAnnotations;

public class UnoTest
{
    [Fact]
    public void TestUno()
    {
        string[] playerOne = { "Timmy" };
        Uno uno = new (playerOne);
        Assert.Equal(108, uno.Counter);
    }

    [Fact]
    public void TestUnoCounter()
    {
        Assert.Equal(107, Uno.DecreaseCounter(108));
    }

    [Fact]
    public void TestDrawCardCreatesCard()
    {
        string[] playerOne = { "Timmy" };
        Uno game = new (playerOne);
        var hand = game.DrawCard("Timmy");
        Assert.True(hand.GetType() == typeof(Card));
    }
}
