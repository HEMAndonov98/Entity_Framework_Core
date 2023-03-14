namespace P02_FootballBetting.Data.Common;

public static class ModelConstraints
{
    //Team
    
    public const string TeamNameLength = "100";

    public const string MaxUrlLength = "2048";

    public const string MaimumNumberOfInitials = "3";

    public const string BudgetDecimalSize = "8,2";
    
    //Color

    public const string ColorNameLength = "50";
    
    //Town

    public const string TownNameLength = "70";
    
    //Country

    public const string CountryNameLength = "60";
    
    // Player

    public const string PlayerNameLength = "200";
    
    // Position

    public const string PositionNameLength = "50";
    
    //Bet

    public const string MoneyAmountSize = "18,2";

    public const string BetRateSize = "4,2";
    
    //User

    public const string BalanceAmount = "19,2";

    public const string UsernameLength = "35";

    public const string PasswordLength = "130";

    public const string NameLength = "60";

    public const string EmailLength = "260";
}