import Page from "../util/page";
import Logger from "../util/logger";

const MAX_FILE_SIZE = 2000000;
const MAX_UPLOAD_COUNT = 3;

export default {
	init(imageUploadElement) {
		if (!imageUploadElement) {
			Logger.error(`Imaeg upload element not valid`);
			throw Error("Image upload element not valid");
		}
		
		let imagesArray = [];
		
		const addButton = Page.query(".image-uploader__add", imageUploadElement);
		addButton.addEventListener("click", () => {
			Page.query(".image-uploader__file-upload", imageUploadElement).click();
		});
		
		Page.query(".image-uploader__file-upload", imageUploadElement).addEventListener("change", function () {
			if (this.files && this.files[0]) {
				console.log(this.files);
				// this.files.forEach(file => {
				for (let i = 0; i < this.files.length; i++) {
					const file = this.files[i];
					
					if (file.size > MAX_FILE_SIZE) {
						Logger.error(`The maximum file size is ${MAX_FILE_SIZE / 1000000} mb`)
						return
					}
					if (Page.query(".image-uploader__images", imageUploadElement).childElementCount >= 3) {
						Logger.error(`You can only upload up to ${MAX_UPLOAD_COUNT} images`);
						return
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
						Page.query(".image-uploader__images").appendChild(img);
					};
					reader.readAsDataURL(file);
				}
			}
		});
	}
}
