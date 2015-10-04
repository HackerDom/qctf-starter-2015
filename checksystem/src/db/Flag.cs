namespace main.db
{
	public class Flag
	{
		public string Value;
		public FlagType Type;
	}

	public enum FlagType
	{
		Unknown,
		Normal,
		Bomb
	}
}