/**
 * Editor box (write a comment, idea, etc..)
 */

export default function initEditor() {
	// Autogrow textarea
	// $(".editor__text").keyup(function() {
	//   this.style.height = this.scrollHeight + "px";
	//
	//   if ($(this).val().length > 10) {
	// 	$(".editor__submit").addClass("editor__submit--show");
	//   } else {
	// 	$(".editor__submit").removeClass("editor__submit--show");
	//   }
	// });
	//
	//
	// // Adding images
	// $(".editor__option--image").click(function() {
	//   $(".editor__file-upload").trigger('click');
	// });
	//
	// $(".editor__file-upload").change(function() {
	//   if (this.files && this.files[0]) {
	// 	let reader = new FileReader();
	// 	reader.onload = function(e) {
	// 	  $(".editor__images").append(`<img src="${e.target.result}"></img>`);
	// 	  $(".editor__images img").click(function() {
	// 		$(this).remove();
	// 	  })
	// 	}
	// 	reader.readAsDataURL(this.files[0]);
	//   }
	// })
	//
	// $(".editor__images img").click(function() {
	//   $(this).remove();
	// });

	// $(".editor__submit").click(function() {
	//   $(this).addClass("btn--loading");

	//   const text = $(".editor__text").val();

	//   let images = [];
	//   $(".editor__images img").each((e) => {
	//     console.log("Image");
	//   });

	//   const formData = new FormData();
	//   formData.set("text", text);

	// });
		
}
