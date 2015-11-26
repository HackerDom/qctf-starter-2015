window.onload = () => {
	var input = <HTMLTextAreaElement>document.getElementById("input");
	var output = document.getElementById("output");

	var e2m = document.getElementById("e2m");
	var e2mTranslator = Translator.english2martian();
	e2m.onclick = () => output.textContent = e2mTranslator(input.value);

	var m2e = document.getElementById("m2e");
	var m2eTranslator = Translator.martian2english();
	m2e.onclick = () => output.textContent = m2eTranslator(input.value);
};