import Page from "../util/page";
import Form from "./form";
import logger from "../util/logger";

const MAX_FILE_SIZE = 2000000;
const MAX_UPLOAD_COUNT = 3;

export default {
	init(editor) {
		if (!editor) {
			throw Error("Editor not valid");
		}
		
		editor.addEventListener("keyup", function () {
			// Show submit
			const submit = Page.query(".editor__submit", editor);
			const textarea = Page.query(".editor__text", editor);
			if (textarea.value.length > 10) {
				submit.classList.add("editor__submit--show");
			} else {
				submit.classList.remove("editor__submit--show");
			}

			// Auto grow textarea
			if (textarea.clientHeight < textarea.scrollHeight) {
				textarea.style.height = textarea.scrollHeight + "px";
				if (textarea.clientHeight < textarea.scrollHeight) {
					textarea.style.height = (textarea.scrollHeight * 2 - textarea.clientHeight) + "px";
				}
			}
		});
		
		// Add images
		Page.query(".editor__option--image", editor).addEventListener("click", function () {
			Page.query(".editor__file-upload--image", editor).click();
		});
		
		let imagesArray = [];
		
		Page.query(".editor__file-upload--image", editor).addEventListener("change", function () {
			if (this.files && this.files[0]) {
				const file = this.files[0];
				if (file.size > MAX_FILE_SIZE) {
					logger.error(`The maximum file size is ${MAX_FILE_SIZE / 1000000} megabyte`);
					return;
				}
				if (Page.query(".editor__images", editor).childElementCount >= 3) {
					logger.error(`You can only upload up to ${MAX_UPLOAD_COUNT} images`);
					return;
				}
				const reader = new FileReader();
				imagesArray.push(file);
				reader.onload = function(e) {
					const img = document.createElement("img");
					img.src = e.target.result;
					img.addEventListener("click", function () {
						this.parentNode.removeChild(this);
						imagesArray = imagesArray.filter(el => el === file);
					});
					Page.query(".editor__images", editor).appendChild(img);
				};
				reader.readAsDataURL(this.files[0]);
			}
		});
		
		// Add videos
		Page.query(".editor__option--video", editor).addEventListener("click", function () {
			Page.query(".editor__file-upload--video", editor).click();
		});

		let videosArray = [];
		
		Page.query(".editor__file-upload--video", editor).addEventListener("change", function () {
			if (this.files && this.files[0]) {
				const file = this.files[0];
				// if (file.size > MAX_FILE_SIZE) {
				// 	logger.error(`The maximum file size is ${MAX_FILE_SIZE / 1000000} megabyte`);
				// 	return;
				// }
				if (Page.query(".editor__videos", editor).childElementCount >= 3) {
					logger.error(`You can only upload up to ${MAX_UPLOAD_COUNT} images`);
					return;
				}
				const reader = new FileReader();
				videosArray.push(file);
				reader.onload = function(e) {
					const video = document.createElement("video");
					const source = document.createElement("source");
					source.src = e.target.result;
					video.appendChild(source);
					video.addEventListener("click", function () {
						this.parentNode.removeChild(this);
						videosArray = videosArray.filter(el => el === file);
					});
					Page.query(".editor__videos", editor).appendChild(video);
				};
				reader.readAsDataURL(this.files[0]);
			}
		});
		
		const editorForm = Form.init(editor);
		
		return {
			onSubmit(callback) {
				editorForm.onSubmit((formData) => {
					if (formData.get('text').length < 10) {
						// callback(formData);
						return;
					}
					imagesArray.forEach(image => {
						formData.append("images", image);
					});
					videosArray.forEach(image => {
						formData.append("videos", image);
					});
					formData.append("texts", formData.get('text'));
					callback(formData);
				});
			},
			getForm() {
				return editorForm;
			}
		}
	}
}

