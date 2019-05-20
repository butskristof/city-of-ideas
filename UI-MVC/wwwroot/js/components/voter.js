import Page from "../util/page";
import VoteRepository from "../repositories/voteRepository";
import Logger from "../util/logger";

export default {
	initAll() {
		const voters = Page.queryAll(".voter");
		voters.forEach(voter => {
			this.init(voter)
		});
	},
	init(voter) {
		if (!voter) {
			throw Error(`voter not valid`);
		}
		
		voter.addEventListener("click", async function () {
			if (userId === "-1") {
				Page.query("#loginButton").click();
				return;
			}

			const target = this.dataset.target + "Id";
			const targetId = this.dataset.targetid;
			const value = this.dataset.value;

			const response = await VoteRepository.vote(target, targetId, value, userId);
			if (response.ok) {
				const counter = this.dataset.counter;
				const counterEL = Page.query("#" + counter);
				const oldVal = parseInt(counterEL.innerText);
				const value = parseInt(this.dataset.value);
				counterEL.innerText = oldVal + value;
			} else {
				console.log(await response.json());
			}
		});
	}
};