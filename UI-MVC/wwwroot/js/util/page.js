
export default {
	onLoad(callback) {
		document.addEventListener("DOMContentLoaded", callback, false);
	},
	reload() {
		location.reload();
		// location = location;
	},
	query(selector, parent = document) {
		return parent.querySelector(selector);
	},
	queryAll(selector, parent = document) {
		return parent.querySelectorAll(selector);
	},
	readCookie(key) {
		const cookieStrings = document.cookie.split(";");
		for (let i = 0; i < cookieStrings.length; i++) {
			const partials = cookieStrings[i].split("=");
			if (partials[0] === key) {
				console.log(partials[1]);
				return partials[1];
			}
		}
		return null;
	},
	getOrganisationId() {
		return this.readCookie("organisation");
	}
}