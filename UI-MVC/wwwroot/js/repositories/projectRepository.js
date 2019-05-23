import Repository from "./repository";

export default {
	createProject(formData) {
		console.log("ORG: ", formData.get('organisationId'));
		return Repository.post("/projects", formData, true);
	},
	createPhase(title, description, startDate, endDate, projectId) {
		return Repository.post("/projectPhases", {
			title,
			description,
			startDate,
			endDate,
			projectId
		})
	}
}