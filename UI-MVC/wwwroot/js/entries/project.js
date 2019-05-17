import Page from "../util/page";
import Form from "../components/form";
import ProjectRepository from "../repositories/projectRepository";
// import ImageUploader from "../components/imageuploader";
import Logger from "../util/logger";
import Editor from "../components/editor";

Page.onLoad(() => {


	const createProjectFormEl = Page.query("#project_create");
	if (createProjectFormEl != null) {
		
		const createProjectEditor = Editor.init(createProjectFormEl);
		createProjectEditor.onSubmit(formData => {
			console.log("Submitted");
		});
		
	}
	
	
	// Create Project
	// const createProjectFormEl = Page.query("#project_create");
	// if (createProjectFormEl != null) {
	//	
	// 	const imageUploader = ImageUploader.init(createProjectFormEl);
	//	
	// 	const createProjectForm = Form.init(createProjectFormEl);
	// 	createProjectForm.onSubmit(async (formData) => {
	// 		formData.append("organisationId", Page.getOrganisationId());
	// 		imageUploader.getImages().forEach(image => {
	// 			formData.append("images", image);
	// 		});
	// 		let response = await ProjectRepository.createProject(formData);
	// 		if (response.ok) {
	// 			window.location.replace("/projects")
	// 		} else {
	// 			const body = await response.json();
	// 			if (body.errors) {
	// 				createProjectForm.showErrors(body.errors);
	// 			} else {
	// 				Logger.error(body);
	// 				createProjectForm.showError(body);
	// 			}
	// 		}
	// 	});
	// }
	
	const createPhaseFormEl = Page.query("#phase_create");
	if (createPhaseFormEl != null) {
		
		const createPhaseForm = Form.init(createPhaseFormEl);
		createPhaseForm.onSubmit(async formData => {
			
		});
	}

});