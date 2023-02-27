namespace P06.RemoveVillain
{
	public static class Query
	{
		public const string FindVillainByIdCMD = @"SELECT Name FROM Villains WHERE Id = @villainId";

		public const string DeleteVillainByIdCMD = @"DELETE FROM Villains
      WHERE Id = @villainId";

		public const string ReleaseVillanMinionsCMD = @"DELETE FROM MinionsVillains 
      WHERE VillainId = @villainId";
    }
}

