import Page from "../util/page";
import Form from "../components/form";
import QuestionnaireRepository from "../repositories/questionnaireRepository";

Page.onLoad(() => {
	
	const qFormEl = Page.query("#questionnaire_fill");
	if (qFormEl != null) {
		const qForm = Form.init(qFormEl);
		qForm.onSubmit(async formData => {
			const questionsResponse = await QuestionnaireRepository.allQuestions(questionnaireId); 
			const questions = await questionsResponse.json();
			
			let answers = [];
			
			for(let question of questions) {
				const results = formData.getAll(question.questionId);
				if (results == null) continue;
				results.forEach(result => {
					if (question.questionType === "OpenQuestion" || question.questionType === "Email") {
						answers.push({
							content: result,
							userId: userId,
							questionId: question.questionId
						});
					} else {
						answers.push({
							userId: userId,
							optionId: result,
							questionId: question.questionId
						})
					}
				});
			}
			const response = await QuestionnaireRepository.createAnswers(answers);
			qForm.handleResponse(response, () => {
				Page.routeTo(`/questionnaire/thanks`);
			});
		});
	}
	
	
	const createQFormEl = Page.query("#questionnaire_create");
	if (createQFormEl != null) {
		const createQForm = Form.init(createQFormEl);
		createQForm.onSubmit(async formData => {
			let response = await QuestionnaireRepository.createQuestionnaire(
				formData.get('title'),
				formData.get('description'),
				formData.get('projectPhaseId')
			);
			createQForm.handleResponse(response, async () => {
				const q = await response.json();
				Page.routeTo(`/Questionnaire/Details/${q.questionnaireId}`);
			});
		});
	}
	
});
