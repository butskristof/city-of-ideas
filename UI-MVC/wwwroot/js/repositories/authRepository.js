import Repository from "./repository";

export default {
    login(email, password) {
        return Repository.post("/api/users/Login",{
            email, password
        }, false, true);
    },
	register (
		firstName, lastName, email, password, password_re, gender, postalCode, dateofbirth, organisation
	) {
    	console.log(arguments);
    	return Repository.post("/api/users/register", {
    		FirstName: firstName,
			LastName: lastName,
			Email: email,
			Password: password,
			ConfirmPassword: password_re,
			postalCode: postalCode,
			gender: gender,
			DateOfBirth: dateofbirth,
			organisation
		}, false, true);
	}
};