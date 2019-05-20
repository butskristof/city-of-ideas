import Page from "../util/page";
import Form from "../components/form";
import OrganisationRepository from "../repositories/organisationRepository";

Page.onLoad(async () => {
	
	const organisationResponse = await OrganisationRepository.singleOrganisation(Page.getOrganisationId());
	const organisation = await organisationResponse.json();
	
	const orgLogoEl = Page.query("#admin_org-logo");
	if (orgLogoEl != null) {
		orgLogoEl.src = organisation.logoLocation;
	}
	const orgColorEl = Page.query("#admin_org-color");
	if (orgColorEl != null) {
		// orgColorEl.value = organisation.identifier;
	}
	const orgIdentEl = Page.query("#admin_org-identifier");
	if (orgIdentEl != null) {
		orgIdentEl.value = organisation.identifier;
	}
	
	const logoFormEl = Page.query("#admin_change-logo-form");
	if (logoFormEl != null) {
		const logoForm = Form.init(logoFormEl);
		const logoFileInput = Page.query("#admin_logo-file-input");
		
		logoForm.onSubmit(() => {
			logoFileInput.click();
		});
		
		logoFileInput.addEventListener("change", async function () {
			if (!this.files || !this.files[0]) return;
			
			const formData = new FormData();
			formData.append("organisationId", Page.getOrganisationId());
			formData.append("picture", this.files[0]);
			
			const response = await OrganisationRepository.changeLogo(formData);
			logoForm.handleResponse(response, () => {
				logoForm.clearErrors();
				Page.reload();
			});
		});
	}
	
	
	const orgIdentFormEl = Page.query("#admin_change-identifier");
	if (orgIdentFormEl != null) {
		const orgIdentForm = Form.init(orgIdentFormEl);
		orgIdentForm.onSubmit(async function (formData) {
			const response = await OrganisationRepository.changeOrganisation(Page.getOrganisationId(), {
				identifier: formData.get('identifier'),
				name: formData.get('identifier')
			});
			orgIdentForm.handleResponse(response, () => {
				orgIdentForm.showSuccess("De organisatie naam is succesvol veranderd");
			});
		});
	}
	
	
	
});