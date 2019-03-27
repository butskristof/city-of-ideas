using System;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.DAL.EF;
using COI.DAL.Ideation.EF;

namespace COI.UI.CA
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			IdeaRepository ir = new IdeaRepository(new CityOfIdeasDbContext());
			Comment c = ir.ReadIdeas().FirstOrDefault().Comments.FirstOrDefault();
			Console.WriteLine(c);
		}
	}
}