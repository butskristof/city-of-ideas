namespace COI.BL.Helpers
{
	public static class Helpers
	{
		public const int MinVoteValue = -1;
		public const int MaxVoteValue = 1;
		
		public static int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
		{
			if (value < inclusiveMinimum) { return inclusiveMinimum; }
			if (value > inclusiveMaximum) { return inclusiveMaximum; }
			return value;
		}	
	}
}