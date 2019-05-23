import Repository from "./repository";

export default {
	changeLogo(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/api/organisations/logo`, formData, true, true);
	},
	changeImage(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/api/organisations/image`, formData, true, true);
	},
	changeOrganisation(organisationId, name, identifier, color, description) {
		console.log(":", organisationId);
		return Repository.put(`/api/organisations/${organisationId}`, {
			name,
			identifier,
			color,
			description
		}, false, true);
	},
	createOrganisation(name, identifier, color, description) {
		return Repository.post(`/api/organisations`, {
			name,
			identifier,
			color,
			description
		}, false, true);
	},
	singleOrganisation(organisationId) {
		return Repository.get(`/organisations/${organisationId}`);
	}
}