import Repository from "./repository";

export default {
    login(email, password) {
        return Repository.post("/api/users/login",{
            email, password
        });
    },
	register(
		firstName, lastName, email, password, password_re
	) {
    	return Repository.post("/api/users/register", {
    		FirstName: firstName,
			LastName: lastName,
			Email: email,
			Password: password,
			ConfirmPassword: password_re
		});
	}
};