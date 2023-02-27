namespace P09.IncreaseMinionAgeSP
{
	public static class Query
	{
		public const string SelectMinions = @"SELECT Name, Age FROM Minions WHERE Id = @Id";
    }
}

