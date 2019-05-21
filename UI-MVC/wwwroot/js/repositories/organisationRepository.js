import Repository from "./repository";

export default {
	changeLogo(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/api/organisations/logo`, formData, true);
	},
	changeImage(organisationId, picture) {
		const formData = new FormData();
		formData.append("picture", picture);
		formData.append("organisationId", organisationId);
		return Repository.post(`/api/organisations/image`, formData, true);
	},
	changeOrganisation(organisationId, name, identifier, color, description) {
		return Repository.put(`/api/organisations/${organisationId}`, {
			organisationId,
			name,
			identifier,
			color,
			description
		});
	},
	createOrganisation(name, identifier, color, description) {
		return Repository.post(`/api/organisations`, {
			name,
			identifier,
			color,
			description
		});
	},
	singleOrganisation(organisationId) {
		return Repository.get(`/api/organisations/${organisationId}`);
	}
}