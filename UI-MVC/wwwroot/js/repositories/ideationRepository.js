import Repository from "./repository";

export default {
	createIdea(formData) {
		return Repository.post("/ideas", formData, true);
	}
}
