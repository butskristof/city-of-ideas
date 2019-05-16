import Page from "../util/page";
import Form from "../components/form";
import ProjectRepository from "../repositories/projectRepository";
import ImageUploader from "../components/imageuploader";

Page.onLoad(() => {
	
	// Create Project
	const createProjectFormEl = Page.query("#project_create");
	if (createProjectFormEl != null) {
		
		ImageUploader.init(createProjectFormEl);
		
		const createProjectForm = Form.init(createProjectFormEl);
		createProjectForm.onSubmit(async (formData) => {
			let response = await ProjectRepository.create(
				formData.get('title'),
				formData.get('description'),
				formData.get('start-date'),
				formData.get('end-date'),
				Page.getOrganisationId()
			);
			if (response.ok) {
				window.location.replace("/projects")
			} else {
				const body = await response.json();
				createProjectForm.showErrors(body.errors);
			}
		});
	}

});