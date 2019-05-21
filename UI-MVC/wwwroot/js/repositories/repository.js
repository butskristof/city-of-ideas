
const baseUrl = "";

export default {
    get(route, useFormData = false) {
        return this.callApi("GET", route, null, useFormData);
    },
    post(route, data = null, useFormData = false) {
        return this.callApi("POST", route, data, useFormData);
    },
    put(route, data = null, useFormData = false) {
    	console.log(data);
        return this.callApi("PUT", route, data, useFormData);
    },
    delete(route, data = null, useFormData = false) {
        return this.callApi("DELETE", route, data, useFormData);
    },
    callApi(method, route, data = null, useFormData = false) {
    	let settings = {
			method,
			mode: "cors",
			cache: "no-cache",
			credentials: "same-origin",
			redirect: "follow",
		};
    	
    	let headers = {
			"Accept": "application/json"
		};
    	if (!useFormData) {
    		headers["Content-Type"] = "application/json";
		}
    	settings["headers"] = headers;
    	
    	if (data !== null) {
			if (useFormData) {
				settings["body"] = data;
			} else {
				settings["body"] = JSON.stringify(data);
			}
		}
    	
        return fetch(baseUrl + route, settings).catch((err) => {
        	// Default error handling, log the error and or show a popup
			console.error("An error occured during a request", err);
		})
    }
};