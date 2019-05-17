import Repository from "./repository";

export default {
	all(ideaId) {
		return Repository.get(`/api/ideas/${ideaId}/comments`);
	},
	new(formData) {
		return Repository.post(`/api/comments`, formData, true);
	}
}