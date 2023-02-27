namespace P08.IncreaseMinionAge
{
	public static class Query
	{
		public const string UpdateNameAndAgeOfMinionCMD = @" UPDATE Minions
   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
 WHERE Id = @Id";

		public const string SelectAllMinionsCMD = @"SELECT Name, Age FROM Minions";
    }
}

