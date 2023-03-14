
namespace Tests
{
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
 
using P02_FootballBetting.Data.Models;

[TestFixture]
public class Test_002
{
    [Test]
    public void ValidateGameTeamPlayer()
    {
        //Get assembly from the most important model
        var assembly = typeof(Team).Assembly;

        var modelNames = new string[]
        {
            "Bet", "Color", "Country", "Game", "Player", "PlayerStatistic", "Position", "Team", "Town", "User"
        };

        var types = new Dictionary<string, Type>();
        foreach (string name in modelNames)
        {
            types[name] = GetModelType(assembly, name);
        }

        //Check Team
        AssertPropertyIsOfType(types["Team"], "TeamId", typeof(int));
        AssertPropertyIsOfType(types["Team"], "Name", typeof(string));
        AssertPropertyIsOfType(types["Team"], "LogoUrl", typeof(string));
        AssertPropertyIsOfType(types["Team"], "Initials", typeof(string));
        AssertPropertyIsOfType(types["Team"], "Budget", typeof(decimal));

        AssertPropertyIsOfType(types["Team"], "PrimaryKitColorId", typeof(int));
        AssertPropertyIsOfType(types["Team"], "PrimaryKitColor", types["Color"]);

        AssertPropertyIsOfType(types["Team"], "SecondaryKitColorId", typeof(int));
        AssertPropertyIsOfType(types["Team"], "SecondaryKitColor", types["Color"]);

        AssertPropertyIsOfType(types["Team"], "TownId", typeof(int));
        AssertPropertyIsOfType(types["Team"], "Town", types["Town"]);

        AssertCollectionIsOfType(types["Team"], "Players", GetCollectionType(types["Player"]));
        AssertCollectionIsOfType(types["Team"], "HomeGames", GetCollectionType(types["Game"]));
        AssertCollectionIsOfType(types["Team"], "AwayGames", GetCollectionType(types["Game"]));

        //Check Game
        AssertPropertyIsOfType(types["Game"], "GameId", typeof(int));
        AssertPropertyIsOfType(types["Game"], "DateTime", typeof(DateTime));

        var contentType = GetPropertyByName(types["Game"], "Result");
        Assert.IsNotNull(contentType, "Game.Result property not found!");

        AssertPropertyIsOfType(types["Game"], "HomeTeamId", typeof(int));
        AssertPropertyIsOfType(types["Game"], "HomeTeam", types["Team"]);

        AssertPropertyIsOfType(types["Game"], "AwayTeamId", typeof(int));
        AssertPropertyIsOfType(types["Game"], "AwayTeam", types["Team"]);

        AssertInteger(types["Game"], "HomeTeamGoals");
        AssertInteger(types["Game"], "AwayTeamGoals");

        AssertRealNumber(types["Game"], "HomeTeamBetRate");
        AssertRealNumber(types["Game"], "AwayTeamBetRate");
        AssertRealNumber(types["Game"], "DrawBetRate");

        AssertCollectionIsOfType(types["Game"], "PlayersStatistics", GetCollectionType(types["PlayerStatistic"]));
        AssertCollectionIsOfType(types["Game"], "Bets", GetCollectionType(types["Bet"]));

        //Check Player
        AssertPropertyIsOfType(types["Player"], "PlayerId", typeof(int));
        AssertPropertyIsOfType(types["Player"], "Name", typeof(string));
        AssertInteger(types["Player"], "SquadNumber");
        AssertPropertyIsOfType(types["Player"], "IsInjured", typeof(bool));

        AssertPropertyIsOfType(types["Player"], "PositionId", typeof(int));
        AssertPropertyIsOfType(types["Player"], "Position", types["Position"]);

        AssertPropertyIsOfType(types["Player"], "TeamId", typeof(int));
        AssertPropertyIsOfType(types["Player"], "Team", types["Team"]);

        AssertCollectionIsOfType(types["Player"], "PlayersStatistics", GetCollectionType(types["PlayerStatistic"]));
    }

    public static void AssertRealNumber(Type type, string propertyName)
    {
        var property = GetPropertyByName(type, propertyName);
        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

        var errorMessage = string.Format($"{type.Name}.{property.Name} property is not a real number!");
        Assert.IsTrue(new[]
            {
                typeof(decimal),
                typeof(float),
                typeof(double)
            }
            .Any(t => t == property.PropertyType), errorMessage);
    }

    public static void AssertInteger(Type type, string propertyName)
    {
        var property = GetPropertyByName(type, propertyName);
        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

        var errorMessage = string.Format($"{type.Name}{property.Name} property is not an integer!");
        Assert.IsTrue(new[]
            {
                typeof(byte),
                typeof(int),
                typeof(long)
            }
            .Any(t => t == property.PropertyType), errorMessage);
    }

    public static PropertyInfo GetPropertyByName(Type type, string propName)
    {
        var properties = type.GetProperties();

        var firstOrDefault = properties.FirstOrDefault(p => p.Name == propName);
        return firstOrDefault;
    }

    public static void AssertPropertyIsOfType(Type type, string propertyName, Type expectedType)
    {
        var property = GetPropertyByName(type, propertyName);
        Assert.IsNotNull(property, $"{type.Name}.{propertyName} property not found.");

        var errorMessage = string.Format($"{type.Name}.{property.Name} property is not {expectedType}!");
        Assert.That(property.PropertyType, Is.EqualTo(expectedType), errorMessage);
    }

    public static void AssertCollectionIsOfType(Type type, string propertyName, Type collectionType)
    {
        var ordersProperty = GetPropertyByName(type, propertyName);

        var errorMessage = string.Format($"{type.Name}.{propertyName} property does not exist!");

        Assert.IsNotNull(ordersProperty, errorMessage);

        Assert.That(collectionType.IsAssignableFrom(ordersProperty.PropertyType));
    }

    public static Type GetModelType(Assembly assembly, string modelName)
    {
        var modelType = assembly.GetTypes()
            .Where(t => t.Name == modelName)
            .FirstOrDefault();

        Assert.IsNotNull(modelType, $"{modelName} model not found!");

        return modelType;
    }

    public static Type GetCollectionType(Type modelType)
    {
        var collectionType = typeof(ICollection<>).MakeGenericType(modelType);
        return collectionType;
    }
}
}