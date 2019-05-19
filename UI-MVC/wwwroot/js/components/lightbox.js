import Page from "../util/page";


export default {
	init(boxableEl) {
		console.log(Page.queryAll(".boxable"));
		console.log(Page.queryAll(".comment__videos"));
		Page.queryAll(".boxable").forEach(function (el) {
			el.addEventListener("click", function (e) {
				e.preventDefault();
				// $("#exampleModal").modal();
			});
		});
	}
};
