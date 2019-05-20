import Repository from "./repository";

export default {
	changeLogo(formData) {
		return Repository.post(`/api/organisations/logo`, formData, true);
	},
	changeOrganisation(organisationId, changes) {
		return Repository.put(`/api/organisations/${organisationId}`, changes);
	},
	singleOrganisation(organisationId) {
		return Repository.get(`/api/organisations/${organisationId}`);
	}
}