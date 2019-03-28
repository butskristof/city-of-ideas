using System;
using System.Collections.Generic;
using System.Linq;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Ideation;
using COI.BL.Questionnaire;
using COI.DAL.Ideation;
using COI.DAL.Questionnaire;
using Moq;
using Xunit;

namespace COI.BL.Tests.Questionnaire
{
	public class QuestionnaireManagerTests
	{
		[Fact]
		public void GetQuestionnaires_WithEmptyRepo_ReturnsEmptyCollection()
		{
			// arrange
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			questionnaireRepo
				.Setup(x => x.ReadQuestionnaires())
				.Returns(new List<Domain.Questionnaire.Questionnaire>());
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetQuestionnaires();
			
			// assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void GetIdeas_WithIdeasInRepo_ReturnsIdeas()
		{
			// arrange
			// arrange
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			questionnaireRepo
				.Setup(x => x.ReadQuestionnaires())
				.Returns(new List<Domain.Questionnaire.Questionnaire>() { new Domain.Questionnaire.Questionnaire()});
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetQuestionnaires();
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(questionnaireRepo.Object.ReadQuestionnaires().Count(), result.Count());
			Assert.Equal(questionnaireRepo.Object.ReadQuestionnaires(), result);
		}
		
		[Fact]
		public void GetQuestionnaire_WithValidId_ReturnsQuestionnaireObject()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			questionnaireRepo
				.Setup(x => x.ReadQuestionnaire(id))
				.Returns(new Domain.Questionnaire.Questionnaire() { QuestionnaireId = id});
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetQuestionnaire(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.QuestionnaireId);
			Assert.Equal(questionnaireRepo.Object.ReadQuestionnaire(id), result);
		}
		
		[Fact]
		public void GetQuestionnaire_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			questionnaireRepo
				.Setup(x => x.ReadQuestionnaire(id))
				.Returns((Domain.Questionnaire.Questionnaire) null);
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetQuestionnaire(id);
			
			// assert
			Assert.Null(result);
		}
		
		[Fact]
		public void GetOpenQuestion_WithValidId_ReturnsOpenQuestionObject()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOpenQuestion(id))
				.Returns(new OpenQuestion(){QuestionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetOpenQuestion(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.QuestionId);
			Assert.Equal(questionRepo.Object.ReadOpenQuestion(id), result);
		}
		
		[Fact]
		public void GetOpenQuestion_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOpenQuestion(id))
				.Returns((OpenQuestion) null);
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetOpenQuestion(id);
			
			// assert
			Assert.Null(result);
		}
		
		[Fact]
		public void GetChoiceQuestion_WithValidId_ReturnsChoiceQuestionObject()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadChoice(id))
				.Returns(new Choice(){QuestionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetChoiceQuestion(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.QuestionId);
			Assert.Equal(questionRepo.Object.ReadChoice(id), result);
		}
		
		[Fact]
		public void GetChoiceQuestion_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadChoice(id))
				.Returns((Choice) null);
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetChoiceQuestion(id);
			
			// assert
			Assert.Null(result);
		}
		
		[Fact]
		public void GetOptionQuestion_WithValidId_ReturnsOptionQuestionObject()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOption(id))
				.Returns(new Option(){OptionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetOption(id);
			
			// assert
			Assert.NotNull(result);
			Assert.Equal(id, result.OptionId);
			Assert.Equal(questionRepo.Object.ReadOption(id), result);
		}
		
		[Fact]
		public void GetOptionQuestion_WithInvalidId_ReturnsNull()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOption(id))
				.Returns((Option) null);
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.GetOption(id);
			
			// assert
			Assert.Null(result);
		}

		[Fact]
		public void AddAnswerToOpenQuestion_WithValidQuestionId_AddsAnswer()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOpenQuestion(id))
				.Returns(new OpenQuestion() { QuestionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Answer newAnswer = new Answer();
			mgr.AddAnswerToOpenQuestion(id, newAnswer);
			OpenQuestion question = mgr.GetOpenQuestion(id);
			
			// assert
			Assert.Equal(1, question.Answers.Count);
			Assert.Equal(newAnswer, question.Answers.First());
		}

		[Fact]
		void AddAnswerToOpenQuestion_WithInvalidQuestionId_Throws()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Action result = () => mgr.AddAnswerToOpenQuestion(id, new Answer());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AnswerOpenQuestion_WithValidQuestionId_ReturnsAnswer()
		{
			// arrange
			const int id = 1;
			const String content = "Test Answer";
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOpenQuestion(id))
				.Returns(new OpenQuestion() { QuestionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			answerRepo
				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
				.Returns<Answer>(a => a);
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.AnswerOpenQuestion(id, content);
			OpenQuestion question = mgr.GetOpenQuestion(id);
			
			// assert
			Assert.Equal(1, question.Answers.Count);
			Assert.Equal(result, question.Answers.First());
		}

		[Fact]
		void AnswerOpenQuestion_WithInvalidQuestionId_Throws()
		{
			// arrange
			const int id = 1;
			const String content = "Test Answer";
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Action result = () => mgr.AnswerOpenQuestion(id, content);
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AddAnswerToOption_WithValidQuestionId_AddsAnswer()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOption(id))
				.Returns(new Option() { OptionId = id});
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Answer newAnswer = new Answer();
			mgr.AddAnswerToOption(id, newAnswer);
			Option option = mgr.GetOption(id);
			
			// assert
			Assert.Equal(1, option.Answers.Count);
			Assert.Equal(newAnswer, option.Answers.First());
		}

		[Fact]
		void AddAnswerToOption_WithInvalidQuestionId_Throws()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Action result = () => mgr.AddAnswerToOption(id, new Answer());
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}

		[Fact]
		public void AnswerChoiceQuestion_WithValidQuestionId_ReturnsAnswer()
		{
			// arrange
			const int optionId = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			questionRepo
				.Setup(x => x.ReadOption(optionId))
				.Returns(new Option(){OptionId = optionId});
			var answerRepo = new Mock<IAnswerRepository>();
			answerRepo
				.Setup(x => x.CreateAnswer(It.IsAny<Answer>()))
				.Returns<Answer>(a => a);
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			var result = mgr.AnswerChoiceQuestion(optionId);
			var option = mgr.GetOption(optionId);
			
			// assert
			Assert.Equal(1, option.Answers.Count);
			Assert.Equal(result, option.Answers.First());
		}

		[Fact]
		void AnswerChoiceQuestion_WithInvalidQuestionId_Throws()
		{
			// arrange
			const int id = 1;
			
			var questionRepo = new Mock<IQuestionRepository>();
			var answerRepo = new Mock<IAnswerRepository>();
			var questionnaireRepo = new Mock<IQuestionnaireRepository>();
			var mgr = new QuestionnaireManager(questionRepo.Object, answerRepo.Object, questionnaireRepo.Object);
			
			// act
			Action result = () => mgr.AnswerChoiceQuestion(id);
			
			// assert
			Assert.Throws<ArgumentException>(result);
		}
	}
}