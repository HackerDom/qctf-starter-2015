using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using main.chat;
using main.db;
using main.utils;

namespace main
{
	public partial class Scores : Page
	{
		protected override void OnLoad(EventArgs e)
		{
			Response.SetCacheServerAndPrivate(30, true);

			var scores = DbStorage.FindScores();
			Array.ForEach(scores, score => score.Value = (double)score.Stars / ElCapitan.FlagsCount);
			ScoresList.DataSource = scores;
			ScoresList.DataBind();
		}

		protected HtmlString GetStars(int stars)
		{
			return new HtmlString(string.Join(string.Empty, Enumerable.Range(0, stars).Select(i => "<span class='star'></span>")));
		}
	}

	public class Score
	{
		public string Avatar;
		public string Name;
		public int Stars;
		public double Value;
	}
}