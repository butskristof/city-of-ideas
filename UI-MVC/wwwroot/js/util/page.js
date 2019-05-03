
export default {
	onLoad(callback) {
		document.addEventListener("DOMContentLoaded", callback, false);
	},
	reload() {
		location.reload();
	},
	query(selector, parent = document) {
		return parent.querySelector(selector);
	}
}