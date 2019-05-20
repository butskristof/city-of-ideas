import Repository from "./repository";

export default {
	vote(target, targetId, value, userId) {
		const data = {
			[target]: parseInt(targetId),
			value: parseInt(value),
			userId
		};
		console.log(data);
		return Repository.post("/api/votes", data);
	}
}