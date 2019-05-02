import {render} from 'timeago.js';
import initEditor from './editor'

initEditor();
console.log(idea);
fillIdeaInfo(idea);

function fillIdeaInfo(idea) {
	showIdeaTitle(idea.Title);
	showIdeaScore(idea.VoteCount);
	showComments(idea.Comments);
}

function showIdeaTitle(title) {
	document.querySelector("#ideaTitle").innerHTML = title;
}

function showIdeaScore(score) {
	document.querySelector("#ideaVoteCount").innerHTML = score;
}

function showComments(comments) {
	document.querySelector("#comments").innerHTML = "";
	comments.forEach(c => addCommentToPage(c));
	registerTimeElements();
}

function addCommentToPage(comment) {
	let newComment = document.createElement("div");
	newComment.classList = "comment";
	newComment.innerHTML = `
		<div class="comment__profile">
			<div
			  class="comment__profile-picture"
			  style="background-image: url('/img/profile.jpeg')"
			></div>
		  </div>
		  <div class="comment__content">
			<h3 class="comment__name">
				${comment.User.FirstName} ${comment.User.LastName}
			</h3>
			<div class="comment__time text--right-under text--underword">
				<time class="timeago" datetime="${comment.Created}"></time>
			</div>
			<p class="comment__text">
				${comment.Fields[0].Content}
			</p>
		  </div>
		  <div class="options-bar">
			<div class="options-bar__option">
				<span class="options-bar__votes mr-2">${comment.Score}</span>
				<i class="material-icons mr-2">thumb_up</i>
				<i class="material-icons outline">thumb_down</i>
			</div>
			<div class="options-bar__option">
				<i class="material-icons outline ml-4">share</i>
			</div>
		</div>
	`;
	document.querySelector("#comments").appendChild(newComment);
}

function registerTimeElements() {
	render(document.querySelectorAll("time.timeago"));
}