namespace COI.BL.Helpers
{
	public static class Helpers
	{
		public const int MinVoteValue = -1;
		public const int MaxVoteValue = 1;
		
		/**
		 * Helper method to make sure the value of the vote gets limited to the minimal and maximal value
		 */
		public static int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
		{
			if (value < inclusiveMinimum) { return inclusiveMinimum; }
			if (value > inclusiveMaximum) { return inclusiveMaximum; }
			return value;
		}	
	}
}