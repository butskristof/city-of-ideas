import Page from "../util/page";

export default {
	init(form) {
		if (!form) {
			throw Error(`Form not valid`);
		}
		return {
			onSubmit(callback) {
				form.addEventListener("submit", (e) => {
					const formData = new FormData(e.target);
					callback(formData);
					e.preventDefault();
				}, false);
			},
			// Shows an error at the top of the form in the .form__errors
			showError(msg) {
				const res = Page.query(".form__errors", form);
				if (res != null) {
					const errorLI = document.createElement("LI");
					const textNode = document.createTextNode(msg);
					errorLI.appendChild(textNode);
					res.appendChild(errorLI);
				}
			},
			// Uses a list of errors from dotnet
			showErrors(errors) {
				const errorList = Object.values(errors);
				this.showError(errorList[0].toString());
				/*errorList.forEach(error => {
					this.showError(error[0]);
				});*/
				
			},
			clear() {
				// Remove errors
				const errorList = Page.query(".form__errors", form);
				if (errorList != null) {
					while (errorList.firstChild) {
						errorList.removeChild(errorList.firstChild);
					}
				}
			},
			async handleResponse(response, onSuccess) {
				if (response.ok) {
					onSuccess()
				} else {
					const responseObject = await response.json();
					if (responseObject.errors) {
						this.showErrors(responseObject.errors);
					} else {
						this.showError(responseObject);
					}
				}
			}
		}
	}
}
