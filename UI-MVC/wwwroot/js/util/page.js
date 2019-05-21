
export default {
	onLoad(callback) {
		document.addEventListener("DOMContentLoaded", callback, false);
	},
	reload() {
		location.reload();
		// location = location;
	},
	routeTo(newLocation) {
		location = newLocation
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
				return partials[1];
			}
		}
		return null;
	},
	getOrganisation() {
		return organisation;
	},
	getOrganisationId() {
		return this.getOrganisation().OrganisationId;
	},
	scrollTo(el) {
		window.scrollTo({
			behavior:  'smooth',
			left: 0,
			top: el.offsetTop
		})
	}
}