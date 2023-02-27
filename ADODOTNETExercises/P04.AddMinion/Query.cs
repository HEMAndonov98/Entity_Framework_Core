namespace P04.AddMinion
{
	public static class Query
	{
		public const string FindTownByNameQuery = @"SELECT Id FROM Towns WHERE Name = @townName";
		public const string FindMinionByName = @"SELECT Id FROM Minions WHERE Name = @Name";
		public const string FindVillainByNameQuery = @"SELECT Id FROM Villains WHERE Name = @Name";


		public const string AddTownToDatabase = @"INSERT INTO Towns (Name) VALUES (@townName)";
		public const string AddVillainWithDefaultEvilness = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";
		public const string AddMinionWithValues = "@INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";
		public const string AddNewMinionMaster = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";
    }
}

