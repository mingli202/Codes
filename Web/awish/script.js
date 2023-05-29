function french() {
    const fr = document.getElementById('french');
    fr.style.display = 'block';

    const en = document.getElementById('english');
    en.style.display = 'none';
}

function english() {
    const fr = document.getElementById('french');
    fr.style.display = 'none';

    const en = document.getElementById('english');
    en.style.display = 'block';
}

// function googleTranslateElementInit() {
//     new google.translate.TranslateElement({pageLanguage: 'en', layout: google.translate.TranslateElement.InlineLayout.HORIZONTAL}, 'google_translate_element');
// }