using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Project;
using COI.BL.Domain.Questionnaire;
using COI.BL.Project;
using COI.DAL.Questionnaire;

namespace COI.BL.Questionnaire
{
	public class QuestionnaireManager : IQuestionnaireManager
	{
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IOptionRepository _optionRepository;
		private readonly IQuestionnaireRepository _questionnaireRepository;
		private readonly IProjectManager _projectManager;

		public QuestionnaireManager(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IOptionRepository optionRepository, IQuestionnaireRepository questionnaireRepository, IProjectManager projectManager)
		{
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_optionRepository = optionRepository;
			_questionnaireRepository = questionnaireRepository;
			_projectManager = projectManager;
		}

		#region Questionnaires

		public IEnumerable<Domain.Questionnaire.Questionnaire> GetQuestionnaires()
		{
			return _questionnaireRepository.ReadQuestionnaires();
		}

		public Domain.Questionnaire.Questionnaire GetQuestionnaire(int questionnaireId)
		{
			return _questionnaireRepository.ReadQuestionnaire(questionnaireId);
		}

		public Domain.Questionnaire.Questionnaire AddQuestionnaire(string title, string description, int projectPhaseId)
		{
			ProjectPhase phase = _projectManager.GetProjectPhase(projectPhaseId);
			if (phase == null)
			{
				throw new ArgumentException("Project phase not found.");
			}
			
			Domain.Questionnaire.Questionnaire questionnaire = new Domain.Questionnaire.Questionnaire()
			{
				Title = title,
				Description = description,
				ProjectPhase = phase
			};

			return AddQuestionnaire(questionnaire);
		}

		private Domain.Questionnaire.Questionnaire AddQuestionnaire(Domain.Questionnaire.Questionnaire questionnaire)
		{
			Validate(questionnaire);
			return _questionnaireRepository.CreateQuestionnaire(questionnaire);
		}

		public Domain.Questionnaire.Questionnaire ChangeQuestionnaire(int id, string title, string description, int projectPhaseId)
		{
			Domain.Questionnaire.Questionnaire toChange = GetQuestionnaire(id);
			if (toChange != null)
			{
				ProjectPhase phase = _projectManager.GetProjectPhase(projectPhaseId);
				if (phase == null)
				{
					throw new ArgumentException("Project phase not found", "projectPhaseId");
				}

				toChange.Title = title;
				toChange.Description = description;
				toChange.ProjectPhase = phase;
				
				Validate(toChange);
				return _questionnaireRepository.UpdateQuestionnaire(toChange);
			}
			
			throw new ArgumentException("Questionnaire not found.", "id");
		}

		public Domain.Questionnaire.Questionnaire RemoveQuestionnaire(int id)
		{
			return _questionnaireRepository.DeleteQuestionnaire(id);
		}

		private void Validate(Domain.Questionnaire.Questionnaire questionnaire)
		{
			Validator.ValidateObject(questionnaire, new ValidationContext(questionnaire), true);
		}

		#endregion

		#region Questions

		public IEnumerable<Question> GetQuestionsForQuestionnaire(int questionnaireId)
		{
			return _questionnaireRepository.ReadQuestionsForQuestionnaires(questionnaireId);
		}

		public Question GetQuestion(int questionId)
		{
			return _questionRepository.ReadQuestion(questionId);
		}

		public Question AddQuestion(string inquiry, bool required, QuestionType type, int questionnaireId)
		{
			Domain.Questionnaire.Questionnaire questionnaire = GetQuestionnaire(questionnaireId);
			if (questionnaire == null)
			{
				throw new ArgumentException("Questionnaire not found.");
			}

			Question question = new Question()
			{
				Inquiry = inquiry,
				QuestionType = type,
				Questionnaire = questionnaire
			};

			return AddQuestion(question);
		}

		private Question AddQuestion(Question question)
		{
			Validate(question);
			return _questionRepository.CreateQuestion(question);
		}

		public Question ChangeQuestion(int id, string inquiry, QuestionType type, int questionnaireId)
		{
			Question toChange = GetQuestion(id);
			if (toChange != null)
			{
				Domain.Questionnaire.Questionnaire questionnaire = GetQuestionnaire(questionnaireId);
				if (questionnaire == null)
				{
					throw new ArgumentException("Questionnaire not found.", "questionnaireId");
				}

				toChange.Inquiry = inquiry;
				toChange.QuestionType = type;
				toChange.Questionnaire = questionnaire;

				Validate(toChange);
				return _questionRepository.UpdateQuestion(toChange);
			}
			
			throw new ArgumentException("Question not found.", "id");
		}

		public Question RemoveQuestion(int id)
		{
			return _questionRepository.DeleteQuestion(id);
		}

		private void Validate(Question question)
		{
			Validator.ValidateObject(question, new ValidationContext(question), true);
		}

		#endregion

		#region Options

		public IEnumerable<Option> GetOptionsForQuestion(int questionId)
		{
			return _questionRepository.ReadOptionsForQuestion(questionId);
		}

		public Option GetOption(int optionId)
		{
			return _optionRepository.ReadOption(optionId);
		}

		public Option AddOption(string content, int questionId)
		{
			Question question = GetQuestion(questionId);
			if (question == null)
			{
				throw new ArgumentException("Question not found.");
			}
			
			Option option = new Option()
			{
				Content = content,
				Question = question
			};

			return AddOption(option);
		}

		private Option AddOption(Option option)
		{
			Validate(option);
			return _optionRepository.CreateOption(option);
		}

		public Option ChangeOption(int id, string content, int questionId)
		{
			Option toChange = GetOption(id);
			if (toChange != null)
			{
				Question question = GetQuestion(questionId);
				if (question == null)
				{
					throw new ArgumentException("Question not found", "questionId");
				}

				toChange.Content = content;
				toChange.Question = question;

				Validate(toChange);
				return _optionRepository.UpdateOption(toChange);
			}
			
			throw new ArgumentException("Option not found", "id");
		}

		public Option RemoveOption(int optionId)
		{
			return _optionRepository.DeleteOption(optionId);
		}

		private void Validate(Option option)
		{
			Validator.ValidateObject(option, new ValidationContext(option), true);
		}

		#endregion

		#region Answers

		public IEnumerable<Answer> GetAnswersForQuestion(int questionId)
		{
			return _answerRepository.ReadAnswersForQuestion(questionId);
		}
		
		public IEnumerable<Answer> GetAnswersForOption(int optionId)
		{
			return _answerRepository.ReadAnswersForOption(optionId);
		}

		public Answer GetAnswer(int id)
		{
			return _answerRepository.ReadAnswer(id);
		}

		public Answer AnswerQuestion(string content, int questionId)
		{
			Question question = GetQuestion(questionId);
			if (question == null)
			{
				throw new ArgumentException("Question not found.", "questionId");
			}
			
			Answer answer = new Answer()
			{
				Content = content,
				Question = question
			};

			return AddAnswer(answer);
		}

		public Answer AnswerOption(int optionId)
		{
			Option option = GetOption(optionId);
			if (option == null)
			{
				throw new ArgumentException("Option not found.", "optionId");
			}
			
			Answer answer = new Answer()
			{
				Option = option
			};

			return AddAnswer(answer);
		}

		private Answer AddAnswer(Answer answer)
		{
			Validate(answer);
			return _answerRepository.CreateAnswer(answer);
		}

		private void Validate(Answer answer)
		{
			Validator.ValidateObject(answer, new ValidationContext(answer), true);
		}

		#endregion
		
//		public OpenQuestion GetOpenQuestion(int questionId)
//		{
//			return _questionRepository.ReadOpenQuestion(questionId);
//		}
//
//		public Answer AnswerOpenQuestion(int questionId, string content)
//		{
//			Answer answer = new Answer()
//			{
//				Content = content
//			};
//			
//			AddAnswerToOpenQuestion(questionId, answer);
//
//			return _answerRepository.CreateAnswer(answer);
//		}
//
//		public void AddAnswerToOpenQuestion(int questionId, Answer answer)
//		{
//			OpenQuestion question = this.GetOpenQuestion(questionId);
//			if (question != null)
//			{
//				answer.OpenQuestion = question;
//				question.Answers.Add(answer);
//				_questionRepository.UpdateQuestion(question);
//			}
//			else
//			{
//				throw new ArgumentException("Question not found.");
//			}
//		}
//
//		public Choice GetChoiceQuestion(int questionId)
//		{
//			return _questionRepository.ReadChoice(questionId);
//		}
//
//		public Option GetOption(int optionId)
//		{
//			return _questionRepository.ReadOption(optionId);
//		}
//
//		public Answer AnswerChoiceQuestion(int optionId)
//		{
//			Answer answer = new Answer();
//			
//			AddAnswerToOption(optionId, answer);
//
//			return _answerRepository.CreateAnswer(answer);
//		}
//
//		public void AddAnswerToOption(int optionId, Answer answer)
//		{
//			Option option = this.GetOption(optionId);
//			if (option != null)
//			{
//				answer.Option = option;
//				option.Answers.Add(answer);
//				_questionRepository.UpdateOption(option);
//			}
//			else
//			{
//				throw new ArgumentException("Option not found.");
//			}
//		}
	}
}