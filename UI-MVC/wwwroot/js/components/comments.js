import Page from "../util/page";
import CommentsRepository from "../repositories/commentsRepository";
import Voter from "./voter";

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
				if (comments.length === 0) {
					commentsList.innerHTML = "<p class='ml-3'>Er zijn nog geen reacties</ph>"
				}
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
				const textContent = this.getTextContent(comment);
				const images = this.getImagesContent(comment);
				const videosContent = this.getVideosContent(comment);
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
							${textContent}
						</p>
						<div class="comment__images">
							${images.map(imageSrc => {
								return `
									<img class="mb-3" src="${imageSrc}" alt="">
								`
							}).join("")}
						</div>
						<div class="comment__videos">
							${videosContent}
						</div>
					  </div>
					  <div class="options-bar">
						<div 
							class="options-bar__option options-bar__option--vote" 
							data-target="Comment"
							data-targetId="${comment.commentId}"
							data-userVal="${comment.userVoteValue}"
							>
							<span class="options-bar__votes mr-2">${comment.voteCount}</span>
							<i class="voter material-icons outline mr-2" data-value="1">thumb_up</i>
							<i class="voter material-icons outline" data-value="-1">thumb_down</i>
						</div>
						<div class="options-bar__option">
							<i class="material-icons outline ml-4">share</i>
						</div>
					</div>
				`;
				return newComment;
			},
			getTextContent(comment) {
				let text = "";
				comment.fields.forEach(field => {
					if (field.fieldType === "Text") {
						text += field.content;
					}
				});
				return text;
			},
			getImagesContent(comment) {
				let images = [];
				comment.fields.forEach(field => {
					if (field.fieldType === "Picture") {
						images.push(field.content);
					}
				});
				return images;
			},
			getVideosContent(comment) {
				let content = "";
				comment.fields.forEach((field) => {
					if (field.fieldType !== "Video") return;
					
					content += `
						<video controls>
							<source src="${field.content}"/>
						</video>
					`;
				});
				return content;
			}
		}
	}
}
