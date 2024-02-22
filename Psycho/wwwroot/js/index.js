const frames = document.querySelectorAll('.question-frame');
let hasDuplicates = false;

const submitButton = document.querySelector('.custom-button');

frames.forEach(frame => {

    const selects = frame.querySelectorAll('select');

    selects.forEach(select => {

        select.addEventListener('change', () => {

            const values = [];

            selects.forEach(s => {
                values.push(s.value);
            });

            const uniqueValues = new Set(values);

            if (uniqueValues.size !== values.length) {
                frame.style.border = '2px solid #F19393';
                showError();
                hasDuplicates = true;
            } else {
                frame.style.border = '';
                hideError();
                hasDuplicates = false;
            }

        });

    });

});

// Получаем элемент для вывода сообщения
const errorMessage = document.getElementById('error-message');

// Функции показа/скрытия  
function showError() {
    errorMessage.style.display = 'block';
}

function hideError() {
    errorMessage.style.display = 'none';
}

var button = document.querySelector(".custom-button");

function checkFields() {
    let lastName = document.getElementById('lastName').value;
    let firstName = document.getElementById('firstName').value;
    let middleName = document.getElementById('middleName').value;

    if (lastName.trim() !== '' && firstName.trim() !== '' && middleName.trim() !== '') {
        return true;
    } else {
        return false;
    }
}

button.addEventListener('click', function (event) {
    if (hasDuplicates) { // Укажите здесь ваше условие
        event.preventDefault();
        //button.setAttribute("disabled", "true");
        Swal.fire({
            icon: 'error',
            title: 'Ошибка!',
            text: 'Сначала исправьте ошибки в полях! Цифры не должны повторяться.'
        });
    } else {
        if (checkFields()) {
            // Здесь можно добавить код для отправки POST запроса
            Swal.fire({
                icon: 'success',
                title: 'Успешно!',
                text: 'Тест успешно пройден, отчёт скачан в формате .txt!'
            });
            //button.setAttribute("disabled", "false");
        }

    }
});