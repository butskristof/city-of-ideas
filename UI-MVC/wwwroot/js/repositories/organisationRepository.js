import Repository from "./repository";

export default {
	changeLogo(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/organisations/logo`, formData, true);
	},
	changeImage(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/organisations/image`, formData, true);
	},
	changeOrganisation(organisationId, name, identifier, color, description) {
		console.log(":", organisationId);
		return Repository.put(`/organisations/${organisationId}`, {
			name,
			identifier,
			color,
			description
		});
	},
	createOrganisation(name, identifier, color, description) {
		return Repository.post(`/organisations`, {
			name,
			identifier,
			color,
			description
		});
	},
	singleOrganisation(organisationId) {
		return Repository.get(`/organisations/${organisationId}`);
	}
}