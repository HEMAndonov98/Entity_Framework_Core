using P02_FootballBetting.Data;

namespace P02_FootballBetting;

public static class StartUp
{
    static void Main(string[] args)
    {
        using (var context = new FootballBettingContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}