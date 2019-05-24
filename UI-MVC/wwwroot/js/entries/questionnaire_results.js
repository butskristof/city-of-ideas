import 'chart.js';

let cvs = document.querySelectorAll(".result-graph");
for (let i = 0; i < cvs.length; ++i) {
	let ctx = cvs[i].getContext('2d');
	cvs[i].height = 200;
	let chart = new Chart(ctx, {
		type: 'bar',
		data: {
			labels: questionData[i].labels,
			datasets: [{
				label: "Aantal stemmen",
				data: questionData[i].values,
				borderWidth: 1
			}]
		},
		options: {
			scales: {
				yAxes: [{
					ticks: {
						beginAtZero: true
					}
				}]
			},
			maintainAspectRatio: false
		}
	});
}

