import Page from "../util/page";
import Form from "./form";

export default {
	init(editor) {
		if (!editor) {
			throw Error("Editor not valid");
		}

		let imagesArray = [];
		let videosArray = [];
		
		editor.addEventListener("keyup", function () {
			// Show submit
			const submit = Page.query(".editor__submit", editor);
			const textareas = Page.queryAll(".editor__text", editor);
			textareas.forEach(textarea => {
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
		});
		
		// Add images
		const addImagesEl = Page.query(".editor__option--image", editor);
		if (addImagesEl != null) {
			addImagesEl.addEventListener("click", function () {
				Page.query(".editor__file-upload--image", editor).click();
			});

			Page.query(".editor__file-upload--image", editor).addEventListener("change", function () {
				if (this.files && this.files[0]) {
					const file = this.files[0];
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
		}
		
		// Add videos
		const addVideosEl = Page.query(".editor__option--video", editor);
		if (addVideosEl != null) {
			addVideosEl.addEventListener("click", function () {
				Page.query(".editor__file-upload--video", editor).click();
			});

			Page.query(".editor__file-upload--video", editor).addEventListener("change", function () {
				if (this.files && this.files[0]) {
					const file = this.files[0];
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
		}
		
		const editorForm = Form.init(editor);
		
		return {
			onSubmit(callback) {
				editorForm.onSubmit((formData) => {
					this.getForm().clearErrors();
					if (formData.get('text') != null) {
						if (formData.get('text').length < 10) {
							return;
						}
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

