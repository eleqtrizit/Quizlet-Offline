"use strict";

var flashcards = {};

function filter() {
	flashcards = {};
	let input = document.getElementById("searchTerm");
	let table = document.getElementById("results");
	table.innerHTML = "";

	for (let i = 0; i < json.length; i++) {
		if (json[i].question.toLowerCase().includes(input.value.toLowerCase())) {
			let tr = document.createElement("tr");
			let q = document.createElement("td");
			let a = document.createElement("td");
			q.innerHTML = json[i].question;
			a.innerHTML = json[i].answer;
			tr.appendChild(q);
			tr.appendChild(a);
			table.appendChild(tr);
		}
	}
}
