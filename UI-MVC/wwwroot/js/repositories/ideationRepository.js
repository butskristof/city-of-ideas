import Repository from "./repository";

export default {
	createIdea(formData) {
		return Repository.post("/api/ideas", formData, true);
	}
}
