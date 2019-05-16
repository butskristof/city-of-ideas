import Repository from "./repository";

export default {
	create(title, description, startDate, endDate, organisationId) {
		return Repository.post("/api/projects", {
			title,
			description,
			startDate,
			endDate,
			organisationId
		});
	}
}