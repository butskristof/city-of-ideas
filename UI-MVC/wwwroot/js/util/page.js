
export default {
	onLoad(callback) {
		document.addEventListener("DOMContentLoaded", callback, false);
	},
	reload() {
		location.reload();
	},
	query(selector, parent = document) {
		return parent.querySelector(selector);
	},
	setCookie(key, value) {
		document.cookie = `${key}=${value}`;
	},
	getCookies() {
		return document.cookie;
	},
	getCookie(key) {
		return this.getCookies().filter((ckey, cvalue) => {
			return ckey === key;
		})[0];
	}
}