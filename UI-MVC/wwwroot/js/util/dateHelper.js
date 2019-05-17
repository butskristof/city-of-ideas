export default {
	dateInputToISO(dateInputValue) {
		return new Date(dateInputValue).toISOString();
	}
}