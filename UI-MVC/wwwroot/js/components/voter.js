import Page from "../util/page";
import VoteRepository from "../repositories/voteRepository";
import Logger from "../util/logger";

export default {
	initAll(wrapper = document) {
		const voters = Page.queryAll(".options-bar__option--vote", wrapper);
		voters.forEach(voterWrapper => {
			this.init(voterWrapper)
		});
	},
	init(voterWrapper) {
		if (!voterWrapper) {
			throw Error(`voter not valid`);
		}
		
		colorThumbs(voterWrapper);

		const target = voterWrapper.dataset.target + "Id";
		const targetId = voterWrapper.dataset.targetid;
		let userVal = voterWrapper.dataset.userval;
		
		const counterEl = Page.query(".options-bar__votes", voterWrapper);
		
		Page.queryAll(".voter", voterWrapper).forEach(voter => {
			const value = voter.dataset.value;
			
			voter.addEventListener("click", async function () {
				if (userId === "-1") {
					Page.query("#loginButton").click();
					return;
				}
				
				let valueToUpdateTo = value;
				if (userVal === value) {
					valueToUpdateTo = 0;
				}

				const response = await VoteRepository.vote(target, targetId, valueToUpdateTo, userId, Page.getOrgId());
				if (response.ok) {
					const newVote = await response.json();
					const oldVal = parseInt(counterEl.innerText);
					const value = newVote.value;
					counterEl.innerText = oldVal + value - userVal;
					userVal = `${value}`;
					voterWrapper.dataset.userval = userVal;
					colorThumbs(voterWrapper);
				} else {
					console.log(response);
					console.log(await response.json());
				}
			});
		});
		
	}
};

function colorThumbs(voterWrapper) {
	const userVal = voterWrapper.dataset.userval;
	
	Page.queryAll(".voter", voterWrapper).forEach(voter => {
		if (userVal === voter.dataset.value) {
			voter.classList.remove("outline");
		} else {
			voter.classList.add("outline");
		}
	});
}