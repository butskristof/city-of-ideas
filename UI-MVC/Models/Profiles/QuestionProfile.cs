using AutoMapper;
using COI.BL.Domain.Questionnaire;
using COI.UI.MVC.Models.DTO.Questionnaire;

namespace COI.UI.MVC.Models.Profiles
{
	public class QuestionProfile : Profile
	{
		public QuestionProfile()
		{
			CreateMap<Questionnaire, QuestionnaireDto>()
				.ForMember(o => o.ProjectPhaseId,
					opt => opt.MapFrom(
						c => c.ProjectPhase.ProjectPhaseId));
			
			CreateMap<Question, QuestionDto>()
				.ForMember(q => q.QuestionnaireId,
					opt => opt.MapFrom(
						c => c.Questionnaire.QuestionnaireId));

			CreateMap<Option, OptionDto>()
				.ForMember(q => q.QuestionId,
					opt => opt.MapFrom(
						c => c.Question.QuestionId));

			CreateMap<Answer, AnswerDto>()
				.ForMember(q => q.OptionId,
					opt => opt.MapFrom(a => a.Option.OptionId))
				.ForMember(q => q.QuestionId,
					opt => opt.MapFrom(a => a.Question.QuestionId))
				.ForMember(q => q.UserId,
					opt => opt.MapFrom(a => a.User.Id));
		}
	}
}