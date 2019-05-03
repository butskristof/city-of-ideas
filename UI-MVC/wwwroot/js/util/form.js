import Page from "./page";

export default {
	loadForm(formSelector) {
		const form = Page.query(formSelector);
		if (!form) {
			throw Error(`Form with selector '${formSelector}' not found`)
		}
		return {
			onSubmit(callback) {
				form.addEventListener("submit", (e) => {
					const formData = new FormData(e.target);
					callback(formData);
					e.preventDefault();
				}, false);
			},
			showError(msg) {
				const res = Page.query(".form__errors", form);
				const errorLI = document.createElement("LI");
				const textNode = document.createTextNode(msg);
				errorLI.appendChild(textNode);
				res.appendChild(errorLI);
			},
			clear() {
				// Remove errors
				const errorList = Page.query(".form__errors", form);
				if (errorList != null) {
					while (errorList.firstChild) {
						errorList.removeChild(errorList.firstChild);
					}
				}
			}
		}
	}
}
