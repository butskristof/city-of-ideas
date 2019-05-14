using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
			if (_userService.NumberOfUsers() > 1)
			{
				return;
			}

			try
			{

				#region Users

				List<User> users = new List<User>();

				users.Add(await _userService.RegisterNewUser("coi@kristofbuts.be", "testtest1", "Kristof", "Buts", Gender.Male, new DateTime(1996, 6, 2), 2222));
				users.Add(await _userService.RegisterNewUser("emre@kristofbuts.be", "testtest1", "Emre", "Arslan", Gender.Male, new DateTime(1996, 6, 2), 2222));
				users.Add(await _userService.RegisterNewUser("jordy@kristofbuts.be", "testtest1", "Jordy", "Bruyns", Gender.Male, new DateTime(1996, 6, 2), 2222));
				users.Add(await _userService.RegisterNewUser("ian@kristofbuts.be", "testtest1", "Ian", "Jakubek", Gender.Male, new DateTime(1996, 6, 2), 2222));
				users.Add(await _userService.RegisterNewUser("wout@kristofbuts.be", "testtest1", "Wout", "Peeters", Gender.Male, new DateTime(1996, 6, 2), 2222));
				users.Add(await _userService.RegisterNewUser("jana@kristofbuts.be", "testtest1", "Jana", "Wouters", Gender.Female, new DateTime(1996, 6, 2), 2222));

				#endregion

				#region Organisations

				List<Organisation> organisations = new List<Organisation>();

				organisations.Add(_organisationManager.AddOrganisation("District Antwerpen", "districtantwerpen"));

				#endregion

				#region Projects

				List<Project> projects = new List<Project>();

				var proj1 = _projectManager.AddProject("Vrijbroekspark",
					"In onze gemeente hebben we een prachtig park. Beslis mee over dit prachtige park.",
					new DateTime(2019, 2, 16), new DateTime(2019, 3, 16), organisations[0].OrganisationId);
				
				var proj2 = _projectManager.AddProject("Haven van antwerpen",
					"De haven van Antwerpen is toe aan vernieuwingen, hoe moeten deze gebeuren?",
					new DateTime(2019, 2, 16), new DateTime(2019, 3, 12), organisations[0].OrganisationId);
				

				#endregion

				#region ProjectPhases

				List<ProjectPhase> phases = new List<ProjectPhase>();

				phases.Add(_projectManager.AddProjectPhase("Project setup", "De setup van het Vrijbroekspark",
					new DateTime(2019, 4, 16), new DateTime(2019, 4, 20), proj1.ProjectId));
				phases.Add(_projectManager.AddProjectPhase("Bank plaatsen", "Het bankje plaatsen",
					new DateTime(2019, 4, 21), new DateTime(2019, 5, 1), proj1.ProjectId));
				phases.Add(_projectManager.AddProjectPhase("Afwerking bank", "Het bankje afwerken en controleren",
					new DateTime(2019, 5, 2), new DateTime(2019, 6, 21), proj1.ProjectId));

				#endregion

				#region Ideations

				List<Ideation> ideations = new List<Ideation>();
				List<Field> fields = new List<Field>();

				ideations.Add(_ideationManager.AddIdeation("Kleur kiezen",
					phases[0].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text, "De kleur van de banken kiezen",
					ideations[0].IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Materiaal kiezen",
					phases[0].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"Het materiaal waarin de banken gemaakt moeten worden kiezen", ideations[1].IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Locatie bepalen",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De exacte locatie van de banken bepalen", ideations[2].IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Hoeveelheid",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"Het aantal banken dat geplaatst moet worden bepalen", ideations[3].IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Afmetingen",
					phases[1].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De grootte, lengte en breedte van de banken bepalen", ideations[4].IdeationId));
				
				ideations.Add(_ideationManager.AddIdeation("Inspectie",
					phases[2].ProjectPhaseId));
				fields.Add(_ideationManager.AddFieldToIdeation(FieldType.Text,
					"De geplaatste bankjes inspecteren op correcte specificaties", ideations[5].IdeationId));

				#endregion

				#region Ideas

				List<Idea> ideas = new List<Idea>();

				ideas.Add(_ideationManager.AddIdea("Blauwe banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken blauw moet zijn.", ideas[0].IdeaId));


				ideas.Add(_ideationManager.AddIdea("Bruine banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken bruin moet zijn.", ideas[1].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("Grijze banken",
					ideations[0].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik vind dat de kleur van de banken grijs moet zijn.", ideas[2].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("Houten banken met metalen leuning",
					ideations[1].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Banken gemaakt van een lokale houtsoort met een metale leuning lijkt mij het beste.", ideas[3].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("Dicht bij het meer",
					ideations[2].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik zou het tof vinden als de banken dicht bij het meer staan.", ideas[4].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("3 keer dubbele banken",
					ideations[3].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Ik wil telkens 2 banken tegen elkaar, en in totaal 2 keer.", ideas[5].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("2 meter lang per bank",
					ideations[4].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Mij lijkt de ideale lengte 2 meter, zo kunnen er voldoende mensen op 1 bank zitten, en nemen ze niet teveel plaats in beslag.", ideas[6].IdeaId));
				
				
				ideas.Add(_ideationManager.AddIdea("Controleren of het aantal klopt",
					ideations[5].IdeationId));
				fields.Add(_ideationManager.AddFieldToIdea(FieldType.Text, "Er moet zeker worden nagekeken of de hoeveelheid banken juist is.", ideas[7].IdeaId));

				#endregion

				#region Comments

				List<Comment> comments = new List<Comment>();

				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[0].Id,
					ideas[0].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "Bankjes moeten hier gewoon komen!", comments[0].CommentId));
				
				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[1].Id,
					ideas[0].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "Wij zitten hier elke dag met onze kinderen en nieuwe bankjes zouden zeker een must zijn.", comments[1].CommentId));
				
				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[2].Id,
					ideas[0].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "Ik wil ook een nieuw bankje.", comments[2].CommentId));
				
				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[3].Id,
					ideas[1].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "Groot gelijk Fons!", comments[3].CommentId));
				
				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[4].Id,
					ideas[2].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "Nu kunnen we er tenminste een klimaatmars organiseren.", comments[4].CommentId));
				
				comments.Add(_cityOfIdeasController.AddCommentToIdea(
					users[5].Id,
					ideas[3].IdeaId));
				fields.Add(_ideationManager.AddFieldToComment(FieldType.Text, "BOMEN BOMEN BOMEN!", comments[5].CommentId));
				
				#endregion

				#region Votes

				List<Vote> votes = new List<Vote>();

				votes.Add(_cityOfIdeasController.AddVoteToIdea(1, users[1].Id, null, ideas[0].IdeaId));
				votes.Add(_cityOfIdeasController.AddVoteToIdea(-1, users[2].Id, null, ideas[0].IdeaId));
				votes.Add(_cityOfIdeasController.AddVoteToIdea(1, users[3].Id, null, ideas[0].IdeaId));
				votes.Add(_cityOfIdeasController.AddVoteToIdea(-1, users[4].Id, null, ideas[0].IdeaId));
				votes.Add(_cityOfIdeasController.AddVoteToIdea(1, users[5].Id, null, ideas[0].IdeaId));

				#endregion

				#region Questionnaires

				List<Questionnaire> questionnaires = new List<Questionnaire>();
				List<Question> questions = new List<Question>();
				List<Option> options = new List<Option>();

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
			}
			catch (Exception e)
			{
				return;
			}
		}
	}
}