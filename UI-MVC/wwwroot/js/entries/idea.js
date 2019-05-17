import Page from "../util/page";
import Editor from "../components/editor";
import Comments from "../components/comments";
import CommentsRepository from "../repositories/commentsRepository";
import IdeationRepository from "../repositories/ideationRepository";
import Voters from "../components/voters";

Page.onLoad(async () => {
	
	// Populate comments
	const commentsEl = Page.query("#comments");
	if (commentsEl != null) {
		const comments = Comments.init(commentsEl);
		await comments.addCommentsForIdea(ideaId);
	}

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
			});
		})
	}
	
	// Voters
	Voters.init();
	
});


