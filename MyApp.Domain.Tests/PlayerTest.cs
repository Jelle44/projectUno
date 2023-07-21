using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyApp.Domain.Tests;
public class PlayerTest
{
    [Fact]
    public void TestPlayerExists()
    {
        string[] names = { "Timmy", "Jimmy" };
        var playerTwo = new Player(names);
        Assert.NotNull(playerTwo);
    }

    [Fact]
    public void TestPlayerHasNextPlayer()
    {
        string[] names = { "Timmy", "Jimmy" };
        Player playerTwo = new(names);
        Assert.NotNull(playerTwo.NextPlayer);
    }

    [Fact]
    public void TestPlayerHasPreviousPlayer()
    {
        string[] names = { "Timmy", "Jimmy" };
        Player playerTwo = new(names);
        Assert.NotNull(playerTwo.PreviousPlayer);
    }

    [Fact]
    public void TestThreeplayersKnoweachOther()
    {
        string[] names = { "Timmy", "Jimmy", "Kimmy" };
        Player playerThree = new (names);
        Assert.Equal("Kimmy", playerThree.Name);
        Assert.Equal("Jimmy", playerThree.NextPlayer.Name);
        Assert.Equal("Timmy", playerThree.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestThreePlayersPlayerOneAndFourAreSame()
    {
        string[] names = { "Timmy", "Jimmy", "Kimmy" };
        Player playerThree = new(names);
        Assert.Equal("Kimmy", playerThree.NextPlayer.NextPlayer.NextPlayer.Name);
    }

    [Fact]
    public void TestPlayerHasUnoProperty()
    {
        string[] names = { "Timmy", "Jimmy" };
        Player playerTwo = new(names);
        Assert.False(playerTwo.Uno);
    }

    [Fact]
    public void TestPlayerHasName()
    {
        string[] names = { "Timmy", "Jimmy" };
        Player playerTwo = new(names);
        Assert.Equal("Jimmy", playerTwo.Name);
    }

    [Fact]
    public void TestPlayersAllHaveCorrectPrevPlayer()
    {
        string[] names = { "Timmy", "Jimmy" };
        Player playerTwo = new(names);
        Assert.Equal("Timmy", playerTwo.PreviousPlayer.Name);
        Assert.Equal("Jimmy", playerTwo.NextPlayer.PreviousPlayer.Name);
    }

    [Fact]
    public void TestDrawCardSwitchesTurn()
    {
        string[] names = { "Timmy", "Jimmy" };
        Deck game = new(names);
        game.DrawCard("Timmy");

        Assert.Equal("Timmy", game.Pile.Owner!.Name);
    }
}
