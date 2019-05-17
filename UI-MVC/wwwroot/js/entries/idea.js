import Page from "../util/page";
import Editor from "../components/editor";
import Comments from "../components/comments";
import CommentsRepository from "../repositories/commentsRepository";
import Voters from "../components/voters";

Page.onLoad(async () => {
	
	const comments = Comments.init(Page.query("#comments"));
	await comments.addCommentsForIdea(ideaId);

	const editorElement = Page.query("#ideaEditor");
	if (editorElement != null) {
		const editor = Editor.init(Page.query("#ideaEditor"));
		editor.onSubmit(async (formData) => {
			formData.append("ideaId", ideaId);
			formData.append("userId", userId);
			const response = await CommentsRepository.new(formData);
			const responseData = await response.text();
			console.log(response);
			console.log(responseData);
		});
	}
	
	Voters.init();
	
	
	
});


