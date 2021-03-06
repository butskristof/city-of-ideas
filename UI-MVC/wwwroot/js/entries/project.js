import Page from "../util/page";
import Form from "../components/form";
import ProjectRepository from "../repositories/projectRepository";
import Logger from "../util/logger";
import Editor from "../components/editor";
import DateHelper from "../util/dateHelper";

Page.onLoad(() => {

	// Project page, open & closed
	const selectStateEl = Page.query("#project_statedropdown");
	if (selectStateEl != null) {
		selectStateEl.addEventListener("change", () => {
			Page.reloadWithQueryParams({
				showLimited: selectStateEl.value !== "all",
				open: selectStateEl.value === "open"
			});
		});
	}

	const createProjectFormEl = Page.query("#project_create");
	if (createProjectFormEl != null) {
		
		const createProjectEditor = Editor.init(createProjectFormEl);
		createProjectEditor.onSubmit(async formData => {
			formData.append("organisationId", Page.getOrganisation().OrganisationId);
			let response = await ProjectRepository.createProject(formData);
			if (response.ok) {
				window.location.replace(`/${Page.getOrgId()}/project`);
			} else {
				const body = await response.json();
				if (body.errors) {
					createProjectEditor.getForm().showErrors(body.errors);
				} else {
					Logger.error(body);
					createProjectEditor.getForm().showError(body);
				}
			}
		});
	}
	
	const createPhaseFormEl = Page.query("#phase_create");
	if (createPhaseFormEl != null) {
		
		const createPhaseForm = Form.init(createPhaseFormEl);
		createPhaseForm.onSubmit(async formData => {
			const response = await ProjectRepository.createPhase(
				formData.get('title'),
				formData.get('description'),
				DateHelper.dateInputToISO(formData.get('startDate')),
				DateHelper.dateInputToISO(formData.get('endDate')),
				projectId
			);
			if (response.ok) {
				window.location.replace(`/${Page.getOrgId()}/project/details/${projectId}`);
			} else {
				const body = await response.json();
				if (body.errors) {
					createPhaseForm.showErrors(body.errors);
				} else {
					Logger.error(body);
					createPhaseForm.showError(body);
				}
			}
		});
	}

});