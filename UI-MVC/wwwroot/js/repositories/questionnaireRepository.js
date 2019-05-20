import Repository from "./repository";

export default {
	allQuestions(questionnaireId) {
		return Repository.get(`/api/questionnaires/${questionnaireId}/questions`);
	},
	createAnswers(answers) {
		return Repository.post(`/api/answers`, answers);
	}
}