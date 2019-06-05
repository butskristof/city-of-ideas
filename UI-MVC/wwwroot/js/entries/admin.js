import Page from "../util/page";
import Form from "../components/form";
import OrganisationRepository from "../repositories/organisationRepository";
import Editor from "../components/editor";
import Logger from "../util/logger";

const MAX_FILE_SIZE_IMAGES = 2000000;

let images = [];

Page.onLoad(async () => {
	const orgEditorEl = Page.query("#admin_org");
	if (orgEditorEl != null) {
		const orgEditor = Editor.init(orgEditorEl);
		
		// Org logo
		initImageSelect(
			"logo",
			Page.query("#admin_org-logo"),
			Page.query("#admin_org-logo-upload"),
			Page.query("#admin_org-logo")
		);
		initImageSelect(
			"image",
			Page.query("#admin_org-image"),
			Page.query("#admin_org-image-upload"),
			Page.query("#admin_org-image")
		);
		
		const action = orgEditorEl.dataset.action;
		
		orgEditor.onSubmit(async formData => {
			let response = null;
			if (action === "update") {
				response = await OrganisationRepository.changeOrganisation(
					Page.getOrganisation().OrganisationId,
					formData.get('name'),
					formData.get('identifier'),
					formData.get('color'),
					formData.get('description')
				);
			} else {
				response = await OrganisationRepository.createOrganisation(
					formData.get('name'),
					formData.get('identifier'),
					formData.get('color'),
					formData.get('description')
				);
			}
			orgEditor.getForm().handleResponse(response, async () => {
				const org = await response.json();
				console.log(org);
				
				if (images["logo"]) {
					const response = await OrganisationRepository.changeLogo(org.organisationId, images["logo"]);
					if (!response.ok) {
						orgEditor.getForm().handleResponse(response);
						return;
					}
				}
				if (images["image"]) {
					const response = await OrganisationRepository.changeImage(org.organisationId, images["image"]);
					if (!response.ok) {
						orgEditor.getForm().handleResponse(response);
						return;
					}
				}
				
				if (action === "update") {
					orgEditor.getForm().showSuccess("De update is geslaagd")
				} else {
					Page.routeTo("/", true);
				}
			});
		});
	}
	
	
});

function initImageSelect(imagesKey, triggerEl, uploadEl, imageEl) {
	triggerEl.addEventListener("click", function () {
		uploadEl.click();
	});

	uploadEl.addEventListener("change", function () {
		if (this.files && this.files[0]) {
			const file = this.files[0];
			if (file.size > MAX_FILE_SIZE_IMAGES) {
				Logger.error(`The maximum file size is ${MAX_FILE_SIZE_IMAGES / 1000000} megabyte`);
				return;
			}
			const reader = new FileReader();
			images[imagesKey] = file;
			reader.onload = function(e) {
				imageEl.src = e.target.result;
			};
			reader.readAsDataURL(this.files[0]);
		}
	});
}
