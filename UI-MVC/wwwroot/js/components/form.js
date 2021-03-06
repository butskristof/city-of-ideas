import Page from "../util/page";

export default {
	init(form) {
		if (!form) {
			throw Error(`Form not valid`);
		}
		return {
			onSubmit(callback) {
				form.addEventListener("submit", (e) => {
					e.preventDefault();
					const formData = new FormData(e.target);
					callback(formData);
				}, false);
			},
			// Shows an error at the top of the form in the .form__errors
			showError(msg, scroll = true) {
				const res = Page.query(".form__errors", form);
				if (res != null) {
					const errorLI = document.createElement("LI");
					const textNode = document.createTextNode(msg);
					errorLI.appendChild(textNode);
					res.appendChild(errorLI);
					if (scroll) {
						Page.scrollTo(errorLI);
					}
				}
			},
			showSuccess(msg, scroll = true) {
				const res = Page.query(".form__errors", form);
				if (res != null) {
					const errorLI = document.createElement("LI");
					errorLI.classList.add("success");
					const textNode = document.createTextNode(msg);
					errorLI.appendChild(textNode);
					res.appendChild(errorLI);
					setTimeout(() => {
						this.clearErrors();
					}, 2000);
					if (scroll) {
						Page.scrollTo(errorLI);
					}
				}
			},
			// Uses a list of errors from dotnet
			showErrors(errors) {
				const errorList = Object.values(errors);
				this.showError(errorList[0].toString());
			},
			clear() {
				// Remove errors
				this.clearErrors();
				setTimeout(() => {
					// Remove data from fields
					const inputs = Page.queryAll("input", form);
					inputs.forEach(input => input.value = "");
					const textareas = Page.queryAll("textarea", form);
					textareas.forEach(textarea => textarea.value = "");
				}, 100);
			},
			clearErrors() {
				const errorList = Page.query(".form__errors", form);
				if (errorList != null) {
					while (errorList.firstChild) {
						errorList.removeChild(errorList.firstChild);
					}
				}
			},
			async handleResponse(response, onSuccess, scroll = true) {
				if (response.ok) {
					onSuccess()
				} else {
					const responseObject = await response.json();
					if (responseObject.errors) {
						this.showErrors(responseObject.errors, scroll);
					} else {
						this.showError(responseObject, scroll);
					}
				}
			}
		}
	}
}
