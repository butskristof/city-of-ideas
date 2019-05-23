import Page from "../util/page"

const baseUrlDefault = `/api/${Page.getOrgId()}`;

export default {
    get(route, useFormData = false, rawUrl = false) {
        return this.callApi("GET", route, null, useFormData, rawUrl);
    },
    post(route, data = null, useFormData = false, rawUrl = false) {
        return this.callApi("POST", route, data, useFormData, rawUrl);
    },
    put(route, data = null, useFormData = false, rawUrl = false) {
    	console.log(data);
        return this.callApi("PUT", route, data, useFormData, rawUrl);
    },
    delete(route, data = null, useFormData = false, rawUrl = false) {
        return this.callApi("DELETE", route, data, useFormData, rawUrl);
    },
    callApi(method, route, data = null, useFormData = false, rawUrl = false) {
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
    	
    	let baseUrl = "";
    	if (!rawUrl) {
    		baseUrl = baseUrlDefault;
		}
    	
        return fetch(baseUrl + route, settings).catch((err) => {
        	// Default error handling, log the error and or show a popup
			console.error("An error occured during a request", err);
		})
    }
};