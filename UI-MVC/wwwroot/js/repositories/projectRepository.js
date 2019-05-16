import Repository from "./repository";

export default {
	createProject(title, description, startDate, endDate, organisationId) {
		return Repository.post("/api/projects", {
			title,
			description,
			startDate,
			endDate,
			organisationId
		});
	},
	createPhase(title, description, startDate, endDate, projectId) {
		return Repository.post("/api/projectPhases", {
			title,
			description,
			startDate,
			endDate,
			projectId
		})
	}
}