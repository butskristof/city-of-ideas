import Repository from "./repository";

export default {
	allQuestions(questionnaireId) {
		return Repository.get(`/questionnaires/${questionnaireId}/questions`);
	},
	createAnswers(answers) {
		return Repository.post(`/answers`, answers);
	}
}