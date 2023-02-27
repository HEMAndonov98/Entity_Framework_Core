namespace P05.ChangeTownNamesCasing
{
	public static class Query
	{
		public const string UpdateCountryNamesToUpperCMD = @"
		UPDATE Towns
	       SET Name = UPPER(Name)
		 WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

		public const string PrintAffectedTownsCMD = @"
		SELECT t.Name 
		  FROM Towns as t
		  JOIN Countries
		    AS c
			ON c.Id = t.CountryCode
		 WHERE c.Name = @countryName";
    }
}

