import Repository from "./repository";

export default {
	all(ideaId) {
		return Repository.get(`/api/ideas/${ideaId}/comments`);
	},
	new(ideaId, userId, fields) {
		return Repository.post(`/api/comments`, {
			ideaId: ideaId,
			userId: userId,
			fields: fields
		});
	}
}