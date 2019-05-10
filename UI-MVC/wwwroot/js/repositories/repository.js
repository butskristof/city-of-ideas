
const baseUrl = "";

export default {
    get(route) {
        return this.callApi("GET", route);
    },
    post(route, body = {}) {
        return this.callApi("POST", route, body);
    },
    put(route, body = {}) {
        return this.callApi("PUT", route, body);
    },
    delete(route, body = {}) {
        return this.callApi("DELETE", route, body);
    },
    callApi(method, route, body = null) {
        return fetch(baseUrl + route, {
            method,
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
            },
            redirect: "follow",
            body: (body == null) ? undefined : JSON.stringify(body)
        }).catch((err) => {
        	// Default error handling, log the error and or show a popup
			console.error("An error occured during a request", err);
		})
    }
};