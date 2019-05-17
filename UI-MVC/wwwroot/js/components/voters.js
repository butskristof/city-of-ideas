import Page from "../util/page";
import VoteRepository from "../repositories/voteRepository";

export default {
	init() {
		const voters = Page.queryAll(".voter");
		if (!voters) return;
		voters.forEach(voter => {
			voter.addEventListener("click", async function () {
				if (userId === "-1") {
					Page.query("#loginButton").click();
					return;
				}
				
				const target = this.dataset.target + "Id";
				const targetId = this.dataset.targetid;
				const value = this.dataset.value;

				const response = await VoteRepository.vote(target, targetId, value, userId);
				console.log(await response.json());
				if (response.ok) {
					const counter = this.dataset.counter;
					const counterEL = Page.query("#" + counter);
					const oldVal = parseInt(counterEL.innerText);
					const value = parseInt(this.dataset.value);
					counterEL.innerText = oldVal + value;
				}
			});
		});
	}
}