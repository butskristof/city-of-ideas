import Page from "../util/page";
import Form from "../components/form";
import AuthRepository from "../repositories/authRepository";
import Logger from "../util/logger";

/* Login */
const loginFormEl = Page.query("#auth_login");
if (loginFormEl != null) {
	const loginForm = Form.init(loginFormEl);
	loginForm.onSubmit(async formData => {
		loginForm.clearErrors();
		const response = await AuthRepository.login(formData.get('email'), formData.get('password'));
		loginForm.handleResponse(response, () => {
			console.log(response);
			Page.reload();
			loginForm.clear();
			Page.query("#loginButton").click();
		});
	});
}

/* Register */
const registerFormEl = Page.query("#auth_register");
if (registerFormEl != null) {
	const registerForm = Form.init(registerFormEl);
	registerForm.onSubmit(async formData => {
		registerForm.clearErrors();
		if (formData.get('gender') !== "male" && formData.get('gender') !== "female") {
			registerForm.showError("The gender field is required", false);
			return;
		}
		const response = await AuthRepository.register(
			formData.get('first_name'),
			formData.get('last_name'),
			formData.get('email'),
			formData.get('password'),
			formData.get('password_re'),
			formData.get('gender'),
			formData.get('zip'),
			formData.get('birthdate'),
			Page.getOrgId()
		);
		registerForm.handleResponse(response, () => {
			// Page.reload();
			registerForm.clear();
			Page.query("#registerButton").click();
			Page.query("#loginButton").click();
		});
	});
}
