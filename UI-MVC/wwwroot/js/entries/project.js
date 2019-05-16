import Page from "../util/page";
import Form from "../components/form";
import ProjectRepository from "../repositories/projectRepository";
import ImageUploader from "../components/imageuploader";
import Logger from "../util/logger";

Page.onLoad(() => {
	
	// Create Project
	const createProjectFormEl = Page.query("#project_create");
	if (createProjectFormEl != null) {
		
		ImageUploader.init(createProjectFormEl);
		
		const createProjectForm = Form.init(createProjectFormEl);
		createProjectForm.onSubmit(async (formData) => {
			let response = await ProjectRepository.createProject(
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
				if (body.errors) {
					createProjectForm.showErrors(body.errors);
				} else {
					Logger.error(body);
					createProjectForm.showError(body);
				}
			}
		});
	}
	
	const createPhaseFormEl = Page.query("#phase_create");
	if (createPhaseFormEl != null) {
		
		const createPhaseForm = Form.init(createPhaseFormEl);
		createPhaseForm.onSubmit(async formData => {
			
		});
	}

});