import Repository from "./repository";

export default {
	all(ideaId) {
		return Repository.get(`/ideas/${ideaId}/comments`);
	},
	new(formData) {
		return Repository.post(`/comments`, formData, true);
	}
}