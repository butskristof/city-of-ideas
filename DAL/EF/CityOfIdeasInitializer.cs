using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Common;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Platform;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.Relations;
using COI.BL.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace COI.DAL.EF
{
	public static class CityOfIdeasInitializer
	{
//		private static bool _hasRunDuringExecution; // make sure we initialise only once per execution

		public static void Initialize(CityOfIdeasDbContext ctx, bool dropCreateDb = false, bool addTestData = true)
		{
//			if (!_hasRunDuringExecution)
//			{
				if (dropCreateDb)
				{
					ctx.Database.EnsureDeleted();
				}

				if (ctx.Database.EnsureCreated())
				{
					if (addTestData)
					{
						Seed(ctx);
					}
				}
				
//				_hasRunDuringExecution = true;
//			}
		}

		public static void Seed(CityOfIdeasDbContext ctx) // provide some initial data
		{
			#region PLATFORM

			Platform cityOfIdeas = new Platform()
			{
				Name = "City of Ideas",
				MaxCharsQuestions = 255,
				MaxCharAnswers = 255
			};
			ctx.Platforms.Add(cityOfIdeas);

			#endregion

			#region ORGANISATIONS

			Organisation districtAntwerpen = new Organisation()
			{
				Name = "District Antwerpen",
				Identifier = "districtantwerpen",
				Platform = cityOfIdeas
			};
			ctx.Organisations.Add(districtAntwerpen);

			#endregion
			
			#region USERS

			BL.Domain.User.User user1 = new BL.Domain.User.User()
			{
				UserId = 1,
				FirstName = "Kristof",
				LastName = "Buts",
				Email = new Email()
				{
					Front = "cityofideas",
					Domain = "kristofbuts",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified,
			};
			user1.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user1});
			ctx.Users.Add(user1);
			BL.Domain.User.User user2 = new BL.Domain.User.User()
			{
				UserId = 2,
				FirstName = "Emre",
				LastName = "Arslan",
				Email = new Email()
				{
					Front = "emre",
					Domain = "coi",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified,
			};
			user2.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user2});
			ctx.Users.Add(user2);
			BL.Domain.User.User user3 = new BL.Domain.User.User()
			{
				UserId = 3,
				FirstName = "Jordy",
				LastName = "Bruyns",
				Email = new Email()
				{
					Front = "jordy",
					Domain = "coi",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified,
			};
			user3.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user3});
			ctx.Users.Add(user3);
			BL.Domain.User.User user4 = new BL.Domain.User.User()
			{
				UserId = 4,
				FirstName = "Ian",
				LastName = "Jakubek",
				Email = new Email()
				{
					Front = "ian",
					Domain = "coi",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified,
			};
			user4.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user4});
			ctx.Users.Add(user4);
			BL.Domain.User.User user5 = new BL.Domain.User.User()
			{
				UserId = 5,
				FirstName = "Wout",
				LastName = "Peeters",
				Email = new Email()
				{
					Front = "wout",
					Domain = "coi",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified
			};
			user5.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user5});
			ctx.Users.Add(user5);
			BL.Domain.User.User user6 = new BL.Domain.User.User()
			{
				UserId = 6,
				FirstName = "Jana",
				LastName = "Wouters",
				Email = new Email()
				{
					Front = "jana",
					Domain = "coi",
					Tld = "be"
				},
				Gender = Gender.Male,
				DateOfBirth = new DateTime(1996,6,2),
				Role = Role.UserVerified,
			};
			user6.Organisations.Add(new OrganisationUser() { Organisation = districtAntwerpen, User = user6});
			ctx.Users.Add(user6);

			#endregion

			#region IDEAS

			Idea idea1 = new Idea()
			{
				IdeaId = 1,
				Title = "Nieuwe bankjes rond het meer",
				CreatedBy = user5
			};
			idea1.Comments.Add(new Comment()
			{
				CommentId = 1,
				User = user1, 
				Created = DateTime.Now.AddDays(-5),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "Bankjes moeten hier gewoon komen!"
					}
				},
			});
			idea1.Comments.Add(new Comment()
			{
				CommentId = 2,
				User = user2, 
				Created = DateTime.Now.AddDays(-4),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "Wij zitten hier elke dag met onze kinderen en nieuwe bankjes zouden zeker een must zijn."
					}
				},
			});
			idea1.Comments.Add(new Comment()
			{
				CommentId = 3,
				User = user3, 
				Created = DateTime.Now.AddDays(-3),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "Ik wil ook een nieuw bankje."
					}
				},
			});
			idea1.Fields.Add(new Field()
			{
				Content = "Het meer is een van de meest bezochte plaatsen in het park. Net daar zijn de bankjes van een matige kwaliteit. Zou het mogelijk zijn om deze bankjes te vernieuwen en misschien nog zelfs extra bankjes bij te plaatsen."
			});
			idea1.Votes.Add(new Vote(){User = user5, Value = 1});
			idea1.Votes.Add(new Vote(){User = user2, Value = -1});
			idea1.Votes.Add(new Vote(){User = user3, Value = 1});
			idea1.Votes.Add(new Vote(){User = user4, Value = 1});
			ctx.Ideas.Add(idea1);
			
			Idea idea2 = new Idea()
			{
				IdeaId = 2,
				Title = "Meer bomen in het bos",
				CreatedBy = user2
			};
			idea2.Comments.Add(new Comment()
			{
				CommentId = 4,
				User = user4, 
				Created = DateTime.Now.AddDays(-3),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "Groot gelijk Fons!"
					}
				},
			});
			idea2.Comments.Add(new Comment()
			{
				CommentId = 5,
				User = user5, 
				Created = DateTime.Now.AddDays(-2),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "Nu kunnen we er tenminste een klimaatmars organiseren."
					}
				},
			});
			idea2.Comments.Add(new Comment()
			{
				CommentId = 6,
				User = user6, 
				Created = DateTime.Now.AddDays(-1),
				Fields = new List<Field>()
				{
					new Field()
					{
						Content = "BOMEN BOMEN BOMEN!"
					}
				},
			});
			idea2.Fields.Add(new Field()
			{
				Content = "Ik ben boos! Ik wil meer bomen!!!"
			});
			
			ctx.Ideas.Add(idea2);
			
			// idea without comments
			Idea idea3 = new Idea()
			{
				IdeaId = 3,
				Title = "Idee zonder comments",
				CreatedBy = user6
			};
			ctx.Ideas.Add(idea3);

			#endregion

			#region Questionnaires

			BL.Domain.Questionnaire.Questionnaire questionnaire1 = new BL.Domain.Questionnaire.Questionnaire()
			{
				QuestionnaireId = 1,
				Title = "Usability evaluation",
				Description = "Beste bezoeker, In volgende enquete willen wij graag de tevredenheid bij het gebruik van onze website bevragen.",
				Questions = new List<Question>()
				{
					new Choice()
					{
						QuestionId = 1,
						Inquiry = "Hoe heeft u onze website gevonden?",
						IsMultipleChoice = true,
						Options = new List<Option>()
						{
							new Option()
							{
								OptionId = 1,
								Content = "Zoekmachine",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 2,
								Content = "Sociale media",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 3,
								Content = "Online advertentie",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 4,
								Content = "Via vrienden of kennissen",
								Answers = new List<Answer>()
							},
						}
					},
					new Choice()
					{
						QuestionId = 2,
						Inquiry = "Is onze website makkelijk te navigeren?",
						IsMultipleChoice = false,
						Options = new List<Option>()
						{
							new Option()
							{
								OptionId = 5,
								Content = "Ja",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 6,
								Content = "Neen",
								Answers = new List<Answer>()
							}
						}
					},
					new Choice()
					{
						QuestionId = 3,
						Inquiry = "Heeft u gevonden wat u zocht?",
						IsMultipleChoice = false,
						Options = new List<Option>()
						{
							new Option()
							{
								OptionId = 7,
								Content = "Ja",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 8,
								Content = "Ongeveer",
								Answers = new List<Answer>()
							},
							new Option()
							{
								OptionId = 9,
								Content = "Neen",
								Answers = new List<Answer>()
							}
						}
					},
					new OpenQuestion()
					{
						QuestionId = 4,
						Inquiry = "Wat vindt u van het design van onze website?",
						Answers = new List<Answer>()
					}
				}
			};
			
			ctx.Questionnaires.Add(questionnaire1);

			#endregion

			ctx.SaveChanges();

			foreach (EntityEntry entry in ctx.ChangeTracker.Entries().ToList())
			{
				entry.State = EntityState.Detached;
			}
		}
	}
}