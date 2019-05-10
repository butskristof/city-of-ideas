import Page from "./util/page";
import Form from "./components/form";
import AuthRepository from "./repositories/authRepository";

/* Login */
const loginForm = Form.init(Page.query("#auth_login"));
loginForm.onSubmit((formData) => {
	loginForm.clear();
	AuthRepository.login(formData.get('email'), formData.get('password'))
		 .then(async (response) => {
			 if (response.ok) {
			 	Page.reload();
			 } else {
				 loginForm.showError("Username and or password are not correct");	
			 }
		 });
});

/* Register */
const registerForm = Form.init(Page.query("#auth_register"));
registerForm.onSubmit((formData) => {
	registerForm.clear();
	AuthRepository.register(
		formData.get('first_name'),
		formData.get('last_name'),
		formData.get('email'),
		formData.get('password'),
		formData.get('password_re')
	).then(response => {
		if (response.ok) {
			Page.reload();
		} else {
			response.json().then(body => {
				registerForm.showErrors(body.errors);
			});
		}
	});
});
