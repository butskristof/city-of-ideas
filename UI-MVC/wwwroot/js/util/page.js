
export default {
	onLoad(callback) {
		document.addEventListener("DOMContentLoaded", callback, false);
	},
	reload() {
		location.reload();
	},
	reloadWithQueryParams(params) {
		let newLoc = location.origin + location.pathname + "?";
		for (let key in params) {
			const value = params[key];
			newLoc += key + "=" + value + "&"
		} 
		newLoc = newLoc.substr(0, newLoc.length - 1);
		this.routeTo(newLoc, true);
	},
	routeTo(newLocation, raw = false) {
		if (!raw) {
			newLocation = "/" + this.getOrgId() + newLocation;
		}
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
	getOrgId() {
		return this.getOrganisation().Identifier;
	},
	scrollTo(el) {
		window.scrollTo({
			behavior:  'smooth',
			left: 0,
			top: el.offsetTop
		})
	}
}