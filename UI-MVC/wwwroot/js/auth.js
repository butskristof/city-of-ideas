import Page from "./util/page";
import Form from "./util/form";
import AuthRepository from "./repositories/authRepository";

/* Login */
const loginForm = Form.loadForm("#auth_login");
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


