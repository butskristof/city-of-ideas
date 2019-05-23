import Repository from "./repository";

export default {
	vote(target, targetId, value, userId, orgId) {
		const data = {
			[target]: parseInt(targetId),
			value: parseInt(value),
			userId
		};
		return Repository.post(`/votes`, data);
	}
}