using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COI.BL.Application;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Ideation;
using COI.BL.Organisation;
using COI.BL.Project;
using COI.BL.Questionnaire;

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
		private readonly IQuestionnaireManager _questionnaireManager;
		private readonly ICityOfIdeasController _cityOfIdeasController;

		public SeedService(IOrganisationManager organisationManager, IUserService userService, IProjectManager projectManager, IIdeationManager ideationManager, IQuestionnaireManager questionnaireManager, ICityOfIdeasController cityOfIdeasController)
		{
			_organisationManager = organisationManager;
			_userService = userService;
			_projectManager = projectManager;
			_ideationManager = ideationManager;
			_questionnaireManager = questionnaireManager;
			_cityOfIdeasController = cityOfIdeasController;
		}

		public async Task Seed()
		{
			if (_userService.NumberOfUsers() != 0)
			{
				return;
			}

			try
			{

				#region Users

				var user1 = await _userService.RegisterNewUser("coi@kristofbuts.be", "testtest1", "Kristof", "Buts");
				var user2 = await _userService.RegisterNewUser("emre@kristofbuts.be", "testtest1", "Emre", "Arslan");
				var user3 = await _userService.RegisterNewUser("jordy@kristofbuts.be", "testtest1", "Jordy", "Bruyns");
				var user4 = await _userService.RegisterNewUser("ian@kristofbuts.be", "testtest1", "Ian", "Jakubek");
				var user5 = await _userService.RegisterNewUser("wout@kristofbuts.be", "testtest1", "Wout", "Peeters");
				var user6 = await _userService.RegisterNewUser("jana@kristofbuts.be", "testtest1", "Jana", "Wouters");

				#endregion

				#region Organisations

				var org1 = _organisationManager.AddOrganisation("District Antwerpen", "districtantwerpen");

				#endregion

				#region Projects

				var proj1 = _projectManager.AddProject("Vrijbroekspark",
					"In onze gemeente hebben we een prachtig park. Beslis mee over dit prachtige park.",
					new DateTime(2019, 2, 16), new DateTime(2019, 3, 16), org1.OrganisationId);

				#endregion

				#region ProjectPhases

				var projPhase1 = _projectManager.AddProjectPhase("Project setup", "De setup van het Vrijbroekspark",
					new DateTime(2019, 4, 16), new DateTime(2019, 4, 20), proj1.ProjectId);
				var projPhase2 = _projectManager.AddProjectPhase("Bank plaatsen", "Het bankje plaatsen",
					new DateTime(2019, 4, 21), new DateTime(2019, 5, 1), proj1.ProjectId);
				var projPhase3 = _projectManager.AddProjectPhase("Afwerking bank", "Het bankje afwerken en controleren",
					new DateTime(2019, 5, 2), new DateTime(2019, 6, 21), proj1.ProjectId);

				#endregion

				#region Ideations

				var ideation1 = _ideationManager.AddIdeation("Kleur kiezen",
					new List<Field>() {new Field() {Content = "De kleur van de banken kiezen"}},
					projPhase1.ProjectPhaseId);
				var ideation2 = _ideationManager.AddIdeation("Materiaal kiezen",
					new List<Field>()
						{new Field() {Content = "Het materiaal waarin de banken gemaakt moeten worden kiezen"}},
					projPhase1.ProjectPhaseId);
				var ideation3 = _ideationManager.AddIdeation("Locatie bepalen",
					new List<Field>() {new Field() {Content = "De exacte locatie van de banken bepalen"}},
					projPhase2.ProjectPhaseId);
				var ideation4 = _ideationManager.AddIdeation("Hoeveelheid",
					new List<Field>() {new Field() {Content = "Het aantal banken dat geplaatst moet worden bepalen"}},
					projPhase2.ProjectPhaseId);
				var ideation5 = _ideationManager.AddIdeation("Afmetingen",
					new List<Field>() {new Field() {Content = "De grootte, lengte en breedte van de banken bepalen"}},
					projPhase2.ProjectPhaseId);
				var ideation6 = _ideationManager.AddIdeation("Inspectie",
					new List<Field>()
						{new Field() {Content = "De geplaatste bankjes inspecteren op correcte specificaties"}},
					projPhase3.ProjectPhaseId);

				#endregion

				#region Ideas

				var idea1 = _ideationManager.AddIdea("Blauwe banken",
					new List<Field>() {new Field() {Content = "Ik vind dat de kleur van de banken blauw moet zijn."}},
					ideation1.IdeationId);
				var idea2 = _ideationManager.AddIdea("Bruine banken",
					new List<Field>() {new Field() {Content = "Ik vind dat de kleur van de banken bruin moet zijn."}},
					ideation1.IdeationId);
				var idea3 = _ideationManager.AddIdea("Grijze banken",
					new List<Field>() {new Field() {Content = "Ik vind dat de kleur van de banken grijs moet zijn."}},
					ideation1.IdeationId);
				var idea4 = _ideationManager.AddIdea("Houten banken met metalen leuning",
					new List<Field>()
					{
						new Field()
						{
							Content =
								"Banken gemaakt van een lokale houtsoort met een metale leuning lijkt mij het beste."
						}
					}, ideation2.IdeationId);
				var idea5 = _ideationManager.AddIdea("Dicht bij het meer",
					new List<Field>()
						{new Field() {Content = "Ik zou het tof vinden als de banken dicht bij het meer staan."}},
					ideation3.IdeationId);
				var idea6 = _ideationManager.AddIdea("3 keer dubbele banken",
					new List<Field>()
						{new Field() {Content = "Ik wil telkens 2 banken tegen elkaar, en in totaal 2 keer."}},
					ideation4.IdeationId);
				var idea7 = _ideationManager.AddIdea("2 meter lang per bank",
					new List<Field>()
					{
						new Field()
						{
							Content =
								"Mij lijkt de ideale lengte 2 meter, zo kunnen er voldoende mensen op 1 bank zitten, en nemen ze niet teveel plaats in beslag."
						}
					}, ideation5.IdeationId);
				var idea8 = _ideationManager.AddIdea("Controleren of het aantal klopt",
					new List<Field>()
						{new Field() {Content = "Er moet zeker worden nagekeken of de hoeveelheid banken juist is."}},
					ideation6.IdeationId);

				#endregion

				#region Comments

				var comment1 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "Bankjes moeten hier gewoon komen!"}}, user1.Id,
					idea1.IdeaId);
				var comment2 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "Wij zitten hier elke dag met onze kinderen en nieuwe bankjes zouden zeker een must zijn."}}, user2.Id,
					idea1.IdeaId);
				var comment3 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "Ik wil ook een nieuw bankje."}}, user3.Id,
					idea1.IdeaId);
				var comment4 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "Groot gelijk Fons!"}}, user4.Id,
					idea2.IdeaId);
				var comment5 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "Nu kunnen we er tenminste een klimaatmars organiseren."}}, user5.Id,
					idea3.IdeaId);
				var comment6 = _cityOfIdeasController.AddCommentToIdea(
					new List<Field>() {new Field() {Content = "BOMEN BOMEN BOMEN!"}}, user6.Id,
					idea4.IdeaId);

				#endregion

				#region Votes

				var vote1 = _cityOfIdeasController.AddVoteToIdea(1, user2.Id, idea1.IdeaId);
				var vote2 = _cityOfIdeasController.AddVoteToIdea(-1, user3.Id, idea1.IdeaId);
				var vote3 = _cityOfIdeasController.AddVoteToIdea(1, user4.Id, idea1.IdeaId);
				var vote4 = _cityOfIdeasController.AddVoteToIdea(-1, user5.Id, idea1.IdeaId);
				var vote5 = _cityOfIdeasController.AddVoteToIdea(1, user6.Id, idea1.IdeaId);

				#endregion

				#region Questionnaires

				var questionnaire1 =
					_questionnaireManager.AddQuestionnaire("Usability evaluation", "Beste bezoeker, In volgende enquete willen wij graag de tevredenheid bij het gebruik van onze website bevragen.", projPhase2.ProjectPhaseId);
				var question1 = _questionnaireManager.AddQuestion("Hoe heeft u onze website gevonden?",
					QuestionType.MultipleChoice, questionnaire1.QuestionnaireId);
				var option1 = _questionnaireManager.AddOption("Zoekmachine", question1.QuestionId);
				var option2 = _questionnaireManager.AddOption("Sociale media", question1.QuestionId);
				var option3 = _questionnaireManager.AddOption("Online advertentie", question1.QuestionId);
				var option4 = _questionnaireManager.AddOption("Via vrienden of kennissen", question1.QuestionId);
				var question2 = _questionnaireManager.AddQuestion("Is onze website makkelijk te gebruiken?",
					QuestionType.SingleChoice, questionnaire1.QuestionnaireId);
				var option5 = _questionnaireManager.AddOption("Ja", question2.QuestionId);
				var option6 = _questionnaireManager.AddOption("Neen", question2.QuestionId);
				var question3 = _questionnaireManager.AddQuestion("Wat vindt u van het design van onze website",
					QuestionType.OpenQuestion, questionnaire1.QuestionnaireId);

				#endregion
			}
			catch (Exception e)
			{
				return;
			}
		}
	}
}