using System;
using System.Collections.Generic;
using COI.BL.Domain.Questionnaire;
using COI.DAL.Questionnaire;

namespace COI.BL.Questionnaire
{
	public class QuestionnaireManager : IQuestionnaireManager
	{
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IQuestionnaireRepository _questionnaireRepository;

		public QuestionnaireManager(IQuestionRepository questionRepository, IAnswerRepository answerRepository, IQuestionnaireRepository questionnaireRepository)
		{
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_questionnaireRepository = questionnaireRepository;
		}

		public IEnumerable<Domain.Questionnaire.Questionnaire> GetQuestionnaires()
		{
			return _questionnaireRepository.ReadQuestionnaires();
		}

		public Domain.Questionnaire.Questionnaire GetQuestionnaire(int questionnaireId)
		{
			return _questionnaireRepository.ReadQuestionnaire(questionnaireId);
		}

		public OpenQuestion GetOpenQuestion(int questionId)
		{
			return _questionRepository.ReadOpenQuestion(questionId);
		}

		public Answer AnswerOpenQuestion(int questionId, string content)
		{
			Answer answer = new Answer()
			{
				Content = content
			};
			
			AddAnswerToOpenQuestion(questionId, answer);

			return _answerRepository.CreateAnswer(answer);
		}

		public void AddAnswerToOpenQuestion(int questionId, Answer answer)
		{
			OpenQuestion question = this.GetOpenQuestion(questionId);
			if (question != null)
			{
				answer.OpenQuestion = question;
				question.Answers.Add(answer);
				_questionRepository.UpdateQuestion(question);
			}
			else
			{
				throw new ArgumentException("Question not found.");
			}
		}

		public Choice GetChoiceQuestion(int questionId)
		{
			return _questionRepository.ReadChoice(questionId);
		}

		public Option GetOption(int optionId)
		{
			return _questionRepository.ReadOption(optionId);
		}

		public Answer AnswerChoiceQuestion(int optionId)
		{
			Answer answer = new Answer();
			
			AddAnswerToOption(optionId, answer);

			return _answerRepository.CreateAnswer(answer);
		}

		public void AddAnswerToOption(int optionId, Answer answer)
		{
			Option option = this.GetOption(optionId);
			if (option != null)
			{
				answer.Option = option;
				option.Answers.Add(answer);
				_questionRepository.UpdateOption(option);
			}
			else
			{
				throw new ArgumentException("Option not found.");
			}
		}
	}
}