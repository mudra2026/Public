function toggleMenu() {
	const menu = document.getElementById('peNavLinks');
	if (menu) {
		menu.classList.toggle('active');
	}
}

document.addEventListener('click', function (event) {
	const menu = document.getElementById('peNavLinks');
	const button = document.querySelector('.pe-menu-button');
	if (menu && menu.classList.contains('active') && !menu.contains(event.target) && !button.contains(event.target)) {
		menu.classList.remove('active');
	}
});
