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
				data: questionData[i].values,
				backgroundColor: [
					'rgba(255,99,99,0.8)',
					'rgba(88,80,141,0.8)',
					'rgba(0,63,92,0.8)',
					'rgba(188,80,144,0.8)',
					'rgba(255,166,0,0.8)'
				],
				borderWidth: 1
			}]
		},
		options: {
			legend: {
				display: false
			},
			scales: {
				yAxes: [{
					scaleLabel: {
						display: true,
						labelString: "Aantal stemmen"
					},
					ticks: {
						beginAtZero: true
					}
				}]
			},
			maintainAspectRatio: false
		}
	});
}

