import Repository from "./repository";

export default {
    login(email, password) {
        return Repository.post("/api/users/login",{
            email, password
        });
    }
};