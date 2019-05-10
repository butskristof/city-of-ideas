import Page from "../util/page";
import CommentsRepository from "../repositories/commentsRepository";

export default {
	init(commentsList) {
		if (!commentsList) {
			throw Error("Commentslist not valid");
		}
		
		return {
			async addCommentsForIdea(ideaId) {
				const commentsResponse = await CommentsRepository.all(ideaId);
				const comments = await commentsResponse.json();
				this.addComments(comments);
			},
			addComments(comments) {
				comments.forEach((comment) => {
					this.addComment((comment));
				});
			},
			addComment(comment) {
				const el = this.createDOMElement(comment);
				commentsList.appendChild(el);
			},
			createDOMElement(comment) {
				let newComment = document.createElement("div");
				newComment.classList.add("comment");
				newComment.innerHTML = `
					<div class="comment__profile">
						<div
						  class="comment__profile-picture"
						  style="background-image: url('/img/profile.jpeg')"
						></div>
					  </div>
					  <div class="comment__content">
						<h3 class="comment__name">
							${comment.user.firstName} ${comment.user.lastName}
						</h3>
						<div class="comment__time text--right-under text--underword">
							<time class="timeago" datetime="${comment.created}"></time>
						</div>
						<p class="comment__text">
							${comment.fields[0].content}
						</p>
					  </div>
					  <div class="options-bar">
						<div class="options-bar__option">
							<span class="options-bar__votes mr-2">${comment.voteCount}</span>
							<i class="material-icons outline mr-2">thumb_up</i>
							<i class="material-icons outline">thumb_down</i>
						</div>
						<div class="options-bar__option">
							<i class="material-icons outline ml-4">share</i>
						</div>
					</div>
				`;
				return newComment;
			}
		}
	}
}
