import Page from "./util/page";
import Form from "./components/form";
import AuthRepository from "./repositories/authRepository";
import Logger from "./util/logger";

/* Login */
const loginFormEl = Page.query("#auth_login");
if (loginFormEl != null) {
	const loginForm = Form.init(loginFormEl);
	loginForm.onSubmit((formData) => {
		loginForm.clear();
		AuthRepository.login(formData.get('email'), formData.get('password'))
			.then(async (response) => {
				if (response.ok) {
					Page.reload();
					loginForm.clear();
				} else {
					loginForm.showError("Username and or password are not correct");
				}
			});
	});
}

/* Register */
const registerFormEl = Page.query("#auth_register");
if (registerFormEl != null) {
	const registerForm = Form.init(registerFormEl);
	registerForm.onSubmit((formData) => {
		registerForm.clear();
		console.log(formData.get('birthdate'));
		console.log(new Date(formData.get('birthdate')).toISOString());
		if (formData.get('gender') !== "male" && formData.get('gender') !== "female") {
			registerForm.showError("The gender field is required");
			return;
		}
		AuthRepository.register(
			formData.get('first_name'),
			formData.get('last_name'),
			formData.get('email'),
			formData.get('password'),
			formData.get('password_re'),
			formData.get('gender'),
			formData.get('zip'),
			formData.get('birthdate')
		).then(response => {
			if (response.ok) {
				registerForm.clear();
				Page.reload();
			} else {
				response.json().then(body => {
					if (body.errors) {
						registerForm.showErrors(body.errors);
					} else {
						Logger.error(body);
					}
				});
			}
		});
	});
}

// TODO: Wat is dit? is niet door mij geschreven das zeker
// let organisationInputfield = Page.query("#div1");
// Page.query("#cb-organisatie").addEventListener("change", function () {
// 	if (this.checked){
// 		organisationInputfield.style.display = "block";
// 	}
// 	else {
// 		organisationInputfield.style.display = "none";
// 	}
// });
