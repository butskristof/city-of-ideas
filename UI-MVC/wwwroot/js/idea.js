// import { render } from 'timeago.js';
// import initEditor from './editor'
//
// addEventListener("load", init, false);
// const BASE_URL = "https://localhost:5001";
//
// function init() {
// 	console.log("init");
// 	initEditor();
//	
// 	loadIdeaScore();
// 	loadCommentsForIdea();
//	
// 	setThumbs();
// }
//
// function registerTimeElements() {
// 	render(document.querySelectorAll('time'));
// }
//
// function setThumbs() {
// 	if (userVote === 1) {
// 		document.querySelector("#thumbsupidea").classList.remove("outline");;
// 	} else if (userVote === -1) {
// 		document.querySelector("#thumbsdownidea").classList.remove("outline");;
// 	}
// }
//
// function loadIdeaScore() {
// 	fetch(`${BASE_URL}/api/Ideas/${ideaId}/Score`)
// 		.then(response => {
// 			if (!response.ok) {
// 				console.error(`${response.status} ${response.statusText}`);
// 			} else {
// 				response.text().then(score => showIdeaScore(score))
// 			}
// 		})
// 		.catch(e => {
// 			console.error(e);
// 		});
// }
//
// function showIdeaScore(score) {
// 	document.querySelector("#ideaScore").innerHTML = score;
// }
//
// function loadCommentsForIdea() {
// 	fetch(`/api/Ideas/${ideaId}/Comments`)
// 		.then(response => {
// 			if (!response.ok) {
// 				console.error(`${response.status} ${response.statusText}`);
// 			} else {
// 				response.json().then(comments => {
// 					comments.sort((a, b) => b.score - a.score);
// 					showCommentsForIdea(comments);
// 				})
// 			}
// 		})
// 		.catch(e => {
// 			console.error(e);
// 		});
// }
//
// function showCommentsForIdea(comments) {
// 	// remove current comments
// 	let container = document.querySelector("#comments");
// 	while (container.firstChild) {
// 		container.removeChild(container.firstChild);
// 	}
//	
// 	// add new comments to page
// 	comments.forEach(c => addCommentToPage(c));
// 	registerTimeElements();
// }
//
// function addCommentToPage(comment) {
// 	document.querySelector("#comments").innerHTML += `
// 		<div class="comment">
// 		<div class="comment__profile">
// 			<div
// 				class="comment__profile-picture"
// 				style=""></div>
// 		</div>
// 		<div class="comment__content">
// 			<h3 class="comment__name">
// 				${comment.user.firstName} ${comment.user.lastName}
// 			</h3>
// 			<div class="comment__time text--right-under text--underword">
// 				<time class="timeago" datetime="${comment.created}"></time>
// 			</div>
// 			<p class="comment__text">
// 				${comment.fields[0].content}
// 			</p>
// 		</div>
// 		<div class="options-bar">
// 			<div class="options-bar__option">
// 				<span class="options-bar__votes mr-2" id="comment${comment.commentId}Score">${comment.score}</span>
// 				<i class="material-icons outline mr-2" onClick="postCommentVote(${comment.commentId}, 1)">thumb_up</i>
// 				<i class="material-icons outline" onClick="postCommentVote(${comment.commentId}, -1)">thumb_down</i>
// 			</div>
// 			<div class="options-bar__option">
// 				<i class="material-icons outline ml-4">share</i>
// 			</div>
// 		</div>
// 		</div>
// 		<div class="divider"></div>`;
// }
