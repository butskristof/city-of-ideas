import Page from "./util/page";
import Form from "./util/form";
import AuthRepository from "./repositories/authRepository";

/* Login */
const loginForm = Form.loadForm(Page.query("#auth_login"));
loginForm.onSubmit((formData) => {
	loginForm.clear();
	AuthRepository.login(formData.get('email'), formData.get('password'))
		 .then((response) => {
			 if (response.ok) {
				 Page.reload();
			 } else {
				 loginForm.showError("Username and or password are not correct");	
			 }
		 });
});

/* Register */
const registerForm = Form.loadForm(Page.query("#auth_register"));
registerForm.onSubmit((formData) => {
	AuthRepository.register(
		formData.get('first_name'),
		formData.get('last_name'),
		formData.get('email'),
		formData.get('password'),
		formData.get('password_re')
	).then(response => {
		if (response.ok) {
			console.log('Register complete');
		} else {
			response.json().then(body => {
				registerForm.showErrors(body.errors);
			});
		}
	});
});
