import Page from "../util/page";
import Editor from "../components/editor";
import Comments from "../components/comments";
import CommentsRepository from "../repositories/commentsRepository";
import IdeationRepository from "../repositories/ideationRepository";
import Form from "../components/form";
import Voter from "../components/voter";
import Logger from "../util/logger";

Page.onLoad(async () => {
	
	// Populate comments
	const commentsEl = Page.query("#comments");
	if (commentsEl != null) {
		const comments = Comments.init(commentsEl);
		await comments.addCommentsForIdea(ideaId);
	}
	
	Voter.initAll();

	// Post a comment
	const editorElement = Page.query("#ideaEditor");
	if (editorElement != null) {
		const editor = Editor.init(editorElement);
		editor.onSubmit(async formData => {
			formData.append("ideaId", ideaId);
			formData.append("userId", userId);
			const response = await CommentsRepository.new(formData);
			editor.getForm().handleResponse(response, () => {
				Page.reload();
			});
		});
	}

	// Post an idea
	const ideaEditorEl = Page.query("#idea_create");
	if (ideaEditorEl != null) {
		const editor = Editor.init(ideaEditorEl);
		editor.onSubmit(async formData => {
			formData.append("ideationId", ideationId);
			formData.append("userId", userId);

			const response = await IdeationRepository.createIdea(formData);
			editor.getForm().handleResponse(response, () => {
				Page.reload();
				editor.getForm().clear();
			});
		})
	}
	
	// Post an ideation
	const createIdeationFormEl = Page.query("#ideation_create");
	if (createIdeationFormEl != null) {

		const createIdeationEditor = Editor.init(createIdeationFormEl);
		createIdeationEditor.onSubmit(async formData => {
			formData.append("organisationId", Page.getOrganisation().OrganisationId);
			// formData.append("");
			let response = await IdeationRepository.createIdeation(formData);
			if (response.ok) {
				// window.location.replace(`/${Page.getOrgId()}/project`);
				const ideation = await response.json();
				Page.routeTo(`/Ideation/Details/${ideation.ideationId}`);
				console.log('success');
			} else {
				const body = await response.json();
				if (body.errors) {
					createIdeationEditor.getForm().showErrors(body.errors);
				} else {
					Logger.error(body);
					createIdeationEditor.getForm().showError(body);
				}
			}
		});
	}
	
	
	// QR Code idea
	const confirmEmailEl = Page.query("#idea_confirm-email");
	if (confirmEmailEl != null) {
		const cEform = Form.init(confirmEmailEl);
		cEform.onSubmit(formData => {
			console.log(formData.get('email'));
		});
	}
	
});


