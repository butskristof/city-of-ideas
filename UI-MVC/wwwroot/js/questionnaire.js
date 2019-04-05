import serialize from "./serializeArray";

const BASE_URL = "https://localhost:5001";

let form = document.querySelector("#questionnaireForm");
form.addEventListener("submit", formSubmit, false);

export function formSubmit(event) {
	event.preventDefault();
	let formData = serialize(form);

	let answers = {
		"choices": [],
		"OpenAnswers": []
	};

	formData.forEach(a => {
		if (a.name.startsWith("choice")) {
			answers.choices.push({ "userId": 1, "optionId": a.value.replace("option", "") });
		} else if (a.name.startsWith("openQuestion")) {
			let text = a.value;
			if (text) {
				answers.OpenAnswers.push({"userId": 1, "questionId": a.name.replace("openQuestion", ""), "content": text});
			}
		}
	});

	console.log(answers);
	postAnswers(answers);
}

function postAnswers(answers) {
	fetch(`${BASE_URL}/api/Questions`, {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json'
		},
		body: JSON.stringify(answers)
	})
		.then(response => {
			if (!response.ok) {
				console.log("Er ging iets mis bij het insturen van uw antwoord.");
			} else {
				hideForm();
				console.log(response);
			}
		})
		.catch(error => {
			console.log("Er ging iets mis bij het insturen van uw antwoord.");
		})
}

function hideForm() {
	form.parentNode.removeChild(form);
}
