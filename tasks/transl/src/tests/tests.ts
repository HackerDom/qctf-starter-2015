/// <reference path="../app.ts"/>
/// <reference path="../Scripts/typings/jasmine/jasmine.d.ts"/>

describe("Translator", () => {
	var e2m = Translator.english2martian();
	it("should translate to martians", () => {
		expect(e2m("Hello WORLD!!!")).toBe("○▭▱◜▶ ▥▻△◉◂▤▮▭");
		expect(e2m("martian method knowledge error bad good")).toBe("◊▱◓▷◙▰◁ ▩○◌▷◓◜ ▷○◓▲▶□◇▪◀ ◄◒◍◌◛ ▢▩◎ ◑◊▵▱ ");
		expect(e2m("good martian method knowledge error bad")).toBe("◓◌◓◜ ◓○●◅▹▼▹ ◓△◍◘▮▱ ▬□▭▦◊▣◘◎◄ ▥◉▣◜◒ ▼◉▱ ");
	})

	it("should translate to english", () => {
		// TODO
	});
});
