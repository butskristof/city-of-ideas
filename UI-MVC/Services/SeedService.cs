using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using COI.BL.Application;
using COI.BL.Domain.Common;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Organisation;
using COI.BL.Domain.Project;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.BL.Organisation;
using COI.BL.Project;
using COI.BL.Questionnaire;
using COI.BL.User;
using COI.UI.MVC.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace COI.UI.MVC.Services
{
	public interface ISeedService
	{
		Task Seed();
	}

	public class SeedService : ISeedService
	{
		private readonly IOrganisationManager _organisationManager;
		private readonly IUserService _userService;
		private readonly IProjectManager _projectManager;
		private readonly IIdeationManager _ideationManager;
		private readonly IUserManager _userManager;
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly ICityOfIdeasController _cityOfIdeasController;

		private static readonly string[] CommentAnswers = new []
		{
			"Dat is manifest onjuist.", "Volledig akkoord!", "Nee, vind ik maar niets", "Wie denken ze wel dat ze zijn?", 
			"En weer liegt u..", "Keitof!", "Amai, superleuk initiatief!", "Kunnen we dit wel betalen?", 
			"Je kan niet neuriën als je je neus toeknijpt.", "Oh nee..", "YES!!!!!", "Had al veel eerder moeten gebeuren!",
			"Ik ben blij dat hier initiatief voor wordt genomen. Bedankt aan de vrijwilligers!", "Mijne zoon doet da toch goed he.",
			"Zalig !", "Dat is een stap in de goede richting tot welvaart.", 
			"Kijk dit is er mis met de politici zij worden nooit persoonlijk verantwoordelijk gesteld voor hun foute beslissingen en de belastingbetaler moet er voor opdraaien.", 
			"BOMEN BOMEN BOMEN!!!!", "Zakkenvullers"
		};

		public SeedService(IOrganisationManager organisationManager, IUserService userService, IProjectManager projectManager, IIdeationManager ideationManager, IUserManager userManager, IQuestionnaireManager questionnaireManager, ICityOfIdeasController cityOfIdeasController)
		{
			_organisationManager = organisationManager;
			_userService = userService;
			_projectManager = projectManager;
			_ideationManager = ideationManager;
			_userManager = userManager;
			_questionnaireManager = questionnaireManager;
			_cityOfIdeasController = cityOfIdeasController;
		}

		public async Task Seed()
		{
			// cancel if there's more data present than just the superadmin created by the role seed
			if (_userService.NumberOfUsers() > 1)
			{
				return;
			}

			try
			{
				Random rnd = new Random();
				
				#region Organisations

				List<Organisation> organisations = new List<Organisation>();

				organisations.Add(_organisationManager.AddOrganisation("District Antwerpen", 
					"districtantwerpen", 
					"Centraal gelegen district in de provincie Antwerpen", 
					"#d11f38"));
				_organisationManager.AddImageToOrganisation(organisations.Last().OrganisationId, "/img/pexels-photo-167676.jpeg");
				_organisationManager.AddLogoToOrganisation(organisations.Last().OrganisationId, "/img/logo_antwerp.jpg");
				organisations.Add(_organisationManager.AddOrganisation("Brussel", 
					"brussels", 
					"Hoofdstad", 
					"#ffffff"));
				_organisationManager.AddImageToOrganisation(organisations.Last().OrganisationId, "/img/brussel.jpeg");
				_organisationManager.AddLogoToOrganisation(organisations.Last().OrganisationId, "/img/logo_brussels.png");

				#endregion
				
				#region Users

				List<User> users = new List<User>();

				users.Add(await _userService.RegisterNewUser("coi@kristofbuts.be", "testtest1", "Kristof", "Buts", Gender.Male, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users.Last().Id, "/img/Profile_1.jpeg");
				
				users.Add(await _userService.RegisterNewUser("emre@kristofbuts.be", "testtest1", "Emre", "Arslan", Gender.Male, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users.Last().Id, "/img/Profile_3.jpeg");
				
				users.Add(await _userService.RegisterNewUser("jordy@kristofbuts.be", "testtest1", "Jordy", "Bruyns", Gender.Male, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users[2].Id, "/img/Profile_4.jpeg");
				
				users.Add(await _userService.RegisterNewUser("ian@kristofbuts.be", "testtest1", "Ian", "Jakubek", Gender.Male, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users[3].Id, "/img/Profile_5.jpeg");
				
				users.Add(await _userService.RegisterNewUser("wout@kristofbuts.be", "testtest1", "Wout", "Peeters", Gender.Male, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users[4].Id, "/img/Profile_6.jpeg");
				
				users.Add(await _userService.RegisterNewUser("jana@kristofbuts.be", "testtest1", "Jana", "Wouters", Gender.Female, new DateTime(1996, 6, 2), 2222, organisations[0].Identifier));
				_userManager.AddPictureLocationToUser(users[5].Id, "/img/Profile_2.jpeg");

				await _userService.AddUserToOrganisation(users[1].Id, organisations[1].Identifier);

				#endregion

				#region Projects

				List<Project> projects = new List<Project>();
				
				// open project sample
				projects.Add(_projectManager.AddProject("Vrijbroekspark",
					new DateTime(2019, 2, 16), 
					new DateTime(2019, 7, 1), 
					organisations[0].OrganisationId));
				_ideationManager.AddFieldToProject(FieldType.Text,
					"In onze gemeente hebben we een prachtig park. Beslis mee over dit prachtige park.",
					projects.Last().ProjectId);
				_ideationManager.AddFieldToProject(FieldType.Picture,
					"/img/park.jpg",
					projects.Last().ProjectId);
				
				// closed project 
				projects.Add(_projectManager.AddProject("Haven van Antwerpen",
					new DateTime(2019, 2, 16), 
					new DateTime(2019, 3, 12), 
					organisations[0].OrganisationId));
				_ideationManager.AddFieldToProject(FieldType.Text, 
					"De haven van Antwerpen is toe aan vernieuwingen, hoe moeten deze gebeuren?",
					projects.Last().ProjectId);
				_ideationManager.AddFieldToProject(FieldType.Picture,
					"/img/haven.jpg",
					projects.Last().ProjectId);
				
				// closed project 
				projects.Add(_projectManager.AddProject("Fietspaden",
					new DateTime(2019, 2, 20), 
					new DateTime(2019, 4, 1), 
					organisations[0].OrganisationId));
				_ideationManager.AddFieldToProject(FieldType.Text,
					"Wij hechten veel belang aan de veiligheid van de zwakke gebruiker.",
					projects.Last().ProjectId);
				_ideationManager.AddFieldToProject(FieldType.Picture,
					"/img/slechtfietspad.jpg",
					projects.Last().ProjectId);
				
				// open project
				projects.Add(_projectManager.AddProject("Beleid voor elektrische steps",
					new DateTime(2019, 5, 15), 
					new DateTime(2019, 7, 15), 
					organisations[0].OrganisationId));
				_ideationManager.AddFieldToProject(FieldType.Text,
					"Bedrijven als BIRD duiken snel overal op, hoe gaan we hier best mee om?",
					projects.Last().ProjectId);
				_ideationManager.AddFieldToProject(FieldType.Picture,
					"/img/bird.jpg",
					projects.Last().ProjectId);

				#endregion

				#region ProjectPhases

				List<ProjectPhase> phases = new List<ProjectPhase>();

				#region Vrijbroekspark
                    phases.Add(_projectManager.AddProjectPhase("Project setup", "De setup van het Vrijbroekspark",
                        new DateTime(2019, 2, 16), new DateTime(2019, 4, 20), projects[0].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Bank plaatsen", "Het bankje plaatsen",
                        new DateTime(2019, 4, 21), new DateTime(2019, 5, 1), projects[0].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Afwerking bank", "Het bankje afwerken en controleren",
                        new DateTime(2019, 5, 2), new DateTime(2019, 7, 1), projects[0].ProjectId));
				#endregion
				
				#region Haven
                    phases.Add(_projectManager.AddProjectPhase("Bepalen budget", "Hoeveel kunnen we investeren?",
                        new DateTime(2019, 2, 16), new DateTime(2019, 2, 22), projects[1].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Bepalen planning", "Op welke termijn moeten de werken gebeuren?",
                        new DateTime(2019, 2, 23), new DateTime(2019, 3, 1), projects[1].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Opvragen offertes", "Wie kan de werken uitvoeren?",
                        new DateTime(2019, 3, 2), new DateTime(2019, 3, 12), projects[1].ProjectId));
				#endregion
				
				#region Fietspaden
                    phases.Add(_projectManager.AddProjectPhase("Bepalen budget", "De setup van het Vrijbroekspark",
                        new DateTime(2019, 2, 20), new DateTime(2019, 3, 1), projects[2].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Bepalen prioriteiten", "Waar moet er het snelst een nieuw fietspad komen?",
                        new DateTime(2019, 3, 2), new DateTime(2019, 3, 19), projects[2].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Opmaken planning", "Wanneer kunnen we de werken uitvoeren?",
                        new DateTime(2019, 3, 20), new DateTime(2019, 4, 1), projects[2].ProjectId));
				#endregion
				
				#region Steps
                    phases.Add(_projectManager.AddProjectPhase("Verzamelen data", "Informatie inwinnen bij de betrokken instanties.",
                        new DateTime(2019, 5, 15), new DateTime(2019, 6, 14), projects[3].ProjectId));
                    phases.Add(_projectManager.AddProjectPhase("Uitschrijven beleid", "Bepalen van de wettelijke context.",
                        new DateTime(2019, 6, 15), new DateTime(2019, 7, 15), projects[3].ProjectId));
				#endregion

				#endregion

				#region Ideations

				List<Ideation> ideations = new List<Ideation>();
				List<Field> fields = new List<Field>();

				#region Project 1

				#region Phase 1

				ideations.Add(_ideationManager.AddIdeation("Kleur kiezen",
					phases[0].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "De kleur van de banken kiezen", ideations.Last().IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Materiaal kiezen",
					phases[0].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"Het materiaal waarin de banken gemaakt moeten worden kiezen", ideations.Last().IdeationId));

				#endregion
				
				#region Phase 2
				ideations.Add(_ideationManager.AddIdeation("Locatie bepalen",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De exacte locatie van de banken bepalen", ideations.Last().IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Hoeveelheid",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"Het aantal banken dat geplaatst moet worden bepalen", ideations.Last().IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Afmetingen",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De grootte, lengte en breedte van de banken bepalen", ideations.Last().IdeationId));
				#endregion

				#region Phase 3
				ideations.Add(_ideationManager.AddIdeation("Inspectie",
					phases[2].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De geplaatste bankjes inspecteren op correcte specificaties", ideations.Last().IdeationId));
				#endregion
				
				#endregion

				#region Project 2

				#region Phase 1
				ideations.Add(_ideationManager.AddIdeation("Hoe bepalen we het budget?",
					phases[3].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Wat is de eerlijkste manier om een budget vast te leggen?", ideations.Last().IdeationId));
				#endregion
				
				#region Phase 2
				ideations.Add(_ideationManager.AddIdeation("Wat met overlast?",
					phases[4].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Hoe pakken we de hinder aan?", ideations.Last().IdeationId));
				ideations.Add(_ideationManager.AddIdeation("Welke strategie volgen we?",
					phases[4].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Heel snel of langer verspreiden om minder hinder te veroorzaken?", ideations.Last().IdeationId));
				#endregion
				
				#region Phase 3
				ideations.Add(_ideationManager.AddIdeation("Toekenningscriteria",
					phases[5].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Waarmee gaan we rekening houden bij het kiezen van een aannemer?", ideations.Last().IdeationId));
				ideations.Add(_ideationManager.AddIdeation("Wie contacteren?",
					phases[5].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Waar kunnen we een offerte vragen?", ideations.Last().IdeationId));
				#endregion

				#endregion

				#region Project 4

				#region Phase 1
                    ideations.Add(_ideationManager.AddIdeation("Kritieke factoren",
                        phases[9].ProjectPhaseId));
                    fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Wat zijn de beslissende factoren in dergelijk beleid?",
                        ideations.Last().IdeationId));
				#endregion
				
				#region Phase 2
                    ideations.Add(_ideationManager.AddIdeation("Maximumsnelheid",
                        phases[10].ProjectPhaseId));
                    fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "Wat is een realistische toegelaten maximumsnelheid?",
                        ideations.Last().IdeationId));
				#endregion

				#endregion

				#endregion

				#region Ideas

				List<Idea> ideas = new List<Idea>();

				#region Project 1
				#region Ideation 1
				ideas.Add(_ideationManager.AddIdea("Blauwe banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken blauw moet zijn.", ideas[0].IdeaId));


				ideas.Add(_ideationManager.AddIdea("Bruine banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken bruin moet zijn.", ideas[1].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("Grijze banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken grijs moet zijn.", ideas[2].IdeaId));
				
				#endregion
				#region Ideation 2

				ideas.Add(_ideationManager.AddIdea("Houten banken met metalen leuning",
					ideations[1].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Banken gemaakt van een lokale houtsoort met een metale leuning lijkt mij het beste.", ideas[3].IdeaId));
				
				#endregion
				#region Ideation 3
				ideas.Add(_ideationManager.AddIdea("Dicht bij het meer",
					ideations[2].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik zou het tof vinden als de banken dicht bij het meer staan.", ideas[4].IdeaId));
				#endregion
				#region Ideation 4
				ideas.Add(_ideationManager.AddIdea("3 keer dubbele banken",
					ideations[3].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik wil telkens 2 banken tegen elkaar, en in totaal 2 keer.", ideas[5].IdeaId));
				#endregion
				#region Ideation 5
				ideas.Add(_ideationManager.AddIdea("2 meter lang per bank",
					ideations[4].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Mij lijkt de ideale lengte 2 meter, zo kunnen er voldoende mensen op 1 bank zitten, en nemen ze niet teveel plaats in beslag.", ideas[6].IdeaId));
				#endregion
				#region Ideation 6
				ideas.Add(_ideationManager.AddIdea("Controleren of het aantal klopt",
					ideations[5].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Er moet zeker worden nagekeken of de hoeveelheid banken juist is.", ideas[7].IdeaId));
				#endregion
				#endregion

				#region Haven

				#region Ideation 7
				ideas.Add(_ideationManager.AddIdea("1% van het BBP",
					ideations[6].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Een economisch haalbare investering.", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("10% van de belastingsinkomst van elke Antwerpenaar",
					ideations[6].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "De haven is positief voor iedereen, en daar moeten we stevig in investeren..", ideas.Last().IdeaId));
				#endregion
				#region Ideation 8
				ideas.Add(_ideationManager.AddIdea("Alles autovrij tijdens de werken",
					ideations[7].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "We kiezen voor de veiligheid van onze werknemers en maken alles autovrij.", ideas.Last().IdeaId));
				#endregion
				#region Ideation 9
				ideas.Add(_ideationManager.AddIdea("Over lange termijn spreiden",
					ideations[8].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "We proberen de overlast te minimaliseren", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("Alles autovrij tijdens de werken",
					ideations[8].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "We kiezen voor de veiligheid van onze werknemers en maken alles autovrij.", ideas.Last().IdeaId));
				#endregion
				#region Ideation 10
				ideas.Add(_ideationManager.AddIdea("Bekwaamheid",
					ideations[9].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Kwaliteit van het werk primeert", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("Prijs",
					ideations[9].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Zo goedkoop mogelijk", ideas.Last().IdeaId));
				#endregion
				#region Ideation 11
				ideas.Add(_ideationManager.AddIdea("Jan Peeters",
					ideations[10].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik ken hem al lang, levert mooi werk", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("Havenwerken nv",
					ideations[10].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Gespecialiseerd in havenwerken", ideas.Last().IdeaId));
				#endregion

				#endregion

				#region Fietspaden
				#endregion

				#region Steps
				#region Ideation 12
				ideas.Add(_ideationManager.AddIdea("Veiligheid van de zwakke weggebruiker",
					ideations[11].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Voetgangers mogen hier geen hinder van ondervinden", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("Straatbeeld",
					ideations[11].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Die BIRDs slingeren overal rond, ze verknoeien het Antwerpse straatbeeld.", ideas.Last().IdeaId));
				#endregion
				#region Ideation 13
				ideas.Add(_ideationManager.AddIdea("10 km/h",
					ideations[12].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind ze heel onveilig, ze moeten veel trager.", ideas.Last().IdeaId));
				ideas.Add(_ideationManager.AddIdea("50 km/h",
					ideations[12].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Met de nodige verantwoordelijkheid is een hoge maximumsnelheid geen probleem.", ideas.Last().IdeaId));
				#endregion
				
				#endregion

				#endregion

				#region Comments

				List<Comment> comments = new List<Comment>();

				for (int i = 0; i < 100; ++i)
				{
                    comments.Add(_cityOfIdeasController.AddCommentToIdea(
						users[rnd.Next(users.Count)].Id, // random user id
						ideas[rnd.Next(ideas.Count)].IdeaId // random idea id
						));
                    fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, CommentAnswers[rnd.Next(CommentAnswers.Length)], comments.Last().CommentId));
				}
				
				#endregion

				#region Votes
				List<Vote> votes = new List<Vote>();


				// generate random user votes
				for (int i = 0; i < 100; ++i)
				{
					// 80% upvotes
					int voteValue = 1;
					if (i % 5 == 0)
					{
						voteValue = -1;
					}

					votes.Add(_cityOfIdeasController.AddVoteToIdea(
						voteValue,
						users[rnd.Next(users.Count)].Id, // random user id
						null, 
						ideas[rnd.Next(ideas.Count)].IdeaId)); // random idea id
				}
				
				// generate random anonymous votes
				for (int i = 0; i < 500; ++i)
				{
					// 80% upvote
					int voteValue = 1;
					if (i % 5 == 0)
					{
						voteValue = -1;
					}
					votes.Add(_cityOfIdeasController.AddVoteToIdea(
						voteValue,
						null,
						null, 
						ideas[rnd.Next(ideas.Count)].IdeaId)); // random idea id
				}

				#endregion

				#region Questionnaires

				List<Questionnaire> questionnaires = new List<Questionnaire>();
				List<Question> questions = new List<Question>();
				List<Option> options = new List<Option>();

				#region Project 1

				#region Questionnaire 1
				questionnaires.Add(_questionnaireManager.AddQuestionnaire("Usability evaluation", "Beste bezoeker, In volgende enquete willen wij graag de tevredenheid bij het gebruik van onze website bevragen.", phases[1].ProjectPhaseId));
				
				questions.Add(_questionnaireManager.AddQuestion("Hoe heeft u onze website gevonden?",
					true, QuestionType.MultipleChoice, questionnaires[0].QuestionnaireId));
				options.Add(_questionnaireManager.AddOption("Zoekmachine", questions[0].QuestionId));
				options.Add(_questionnaireManager.AddOption("Sociale media", questions[0].QuestionId));
				options.Add(_questionnaireManager.AddOption("Online advertentie", questions[0].QuestionId));
				options.Add(_questionnaireManager.AddOption("Via vrienden of kennissen", questions[0].QuestionId));
				questions.Add(_questionnaireManager.AddQuestion("Is onze website makkelijk te gebruiken?",
					true, QuestionType.SingleChoice, questionnaires[0].QuestionnaireId));
				options.Add(_questionnaireManager.AddOption("Ja", questions[1].QuestionId));
				options.Add(_questionnaireManager.AddOption("Neen", questions[1].QuestionId));
				questions.Add(_questionnaireManager.AddQuestion("Wat vindt u van het design van onze website",
					true, QuestionType.OpenQuestion, questionnaires[0].QuestionnaireId));
				#endregion

				#region Questionnaire 2
				questionnaires.Add(_questionnaireManager.AddQuestionnaire("Bevraging elektische steps", "Graag hadden we mee informatie ingewonnen over de populariteit van elektrische steps.", phases[9].ProjectPhaseId));
				
				questions.Add(_questionnaireManager.AddQuestion("Welke van onderstaande diensten heeft u reeds gebruikt?", true, QuestionType.MultipleChoice, questionnaires.Last().QuestionnaireId));
				options.Add(_questionnaireManager.AddOption("BIRD", questions.Last().QuestionId));
				options.Add(_questionnaireManager.AddOption("Lime", questions.Last().QuestionId));
				options.Add(_questionnaireManager.AddOption("Poppy", questions.Last().QuestionId));
				
				questions.Add(_questionnaireManager.AddQuestion("Verplicht een helm dragen?", true, QuestionType.SingleChoice, questionnaires.Last().QuestionnaireId));
				options.Add(_questionnaireManager.AddOption("Ja", questions.Last().QuestionId));
				options.Add(_questionnaireManager.AddOption("Neen", questions.Last().QuestionId));
				
				questions.Add(_questionnaireManager.AddQuestion("Wat is uw mening over de elektrische step in het straatbeeld?",
					true, QuestionType.OpenQuestion, questionnaires.Last().QuestionnaireId));

				#endregion
				
				#region Questionnaire 3
				questionnaires.Add(_questionnaireManager.AddQuestionnaire("Bevraging uitbreiding haven", "Graag hadden we uw mening gehoord over de uitbreiding van de haven", phases[3].ProjectPhaseId));
				
				questions.Add(_questionnaireManager.AddQuestion("Wat vindt u van de uitbreiding van de haven?", true, QuestionType.OpenQuestion, questionnaires.Last().QuestionnaireId));
				
				questions.Add(_questionnaireManager.AddQuestion("Waaraan moeten we hoogste prioriteit geven?", true, QuestionType.Dropdown, questionnaires.Last().QuestionnaireId));
				options.Add(_questionnaireManager.AddOption("Doorvoer verhogen", questions.Last().QuestionId));
				options.Add(_questionnaireManager.AddOption("Overlast minimaliseren", questions.Last().QuestionId));
				options.Add(_questionnaireManager.AddOption("Drugstraffiek aanpakken", questions.Last().QuestionId));
				
				questions.Add(_questionnaireManager.AddQuestion("Wat zou u doen om de vervuiling door de haven te verminderen?",
					true, QuestionType.OpenQuestion, questionnaires.Last().QuestionnaireId));

				#endregion

				#endregion

				#endregion
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return;
			}
		}
	}
}