import Page from "../util/page";
import Editor from "../components/editor";
import Comments from "../components/comments";
import CommentsRepository from "../repositories/commentsRepository";

Page.onLoad(async () => {
	
	const comments = Comments.init(Page.query("#comments"));
	await comments.addCommentsForIdea(ideaId);

	const editorElement = Page.query("#ideaEditor");
	if (editorElement != null) {
		const editor = Editor.init(Page.query("#ideaEditor"));
		editor.onSubmit(async (fields) => {
			console.log(fields);
			const response = await CommentsRepository.new(ideaId, userId, fields);
			console.log(response);
			const responseData = await response.json();
			console.log(responseData);
			// const response = await CommentsRepository.new(ideaId, {
			// 	fields: [
			// 		{
			// 			content: "test",
			// 			ideaId: ideaId
			// 		}
			// 	],
			// 	__RequestVerificationToken: formData.__RequestVerificationToken
			// });
			// if (!response.ok) {
			// 	// const ro = await response.json();
			// 	console.log("err:", await response.text());
			// }
			// console.log(response);
		});
	}
	
	
	
});


