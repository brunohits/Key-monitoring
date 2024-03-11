document.addEventListener('DOMContentLoaded', function() {
    // Получение данных с сервера при загрузке страницы
    getDataFromServer();

    // Обработчик события для кнопки поиска
    const searchButton = document.getElementById('searchButton');
    searchButton.addEventListener('click', function() {
        searchApplications();
    });
});

async function getDataFromServer() {
    const url = 'https://localhost:7266/api/application/applicationsList';
    const options = {
        method: 'GET',
        headers: {
            'accept': 'application/json',
            //'status': 1, // Установка параметра status в заголовке запроса
            'page': 1, // Установка параметра page в заголовке запроса
            'size': 5 // Установка параметра size в заголовке запроса
        }
    };

    try {
        const response = await fetch(url, options);
        const data = await response.json();
        console.log(data);
        createTable(data.list);
    } catch (error) {
        console.error('Ошибка при получении данных с сервера:', error);
    }
}

function createTable(applications) {
    const tableBody = document.querySelector('.table tbody');
    tableBody.innerHTML = '';

    applications.forEach(function(application, index) {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${index + 1}</td>
            <td>${formatDateTime(application.date)}</td>
            <td>${application.status}</td>
            <td>${application.ownerName}</td>
            <td>${application.ownerRole}</td>
            <td>${application.keyNumber} (${application.cabinetNumber})</td>
            <td></td>
            <td>${formatDateTime(application.pairStart)}</td>
            <td>
                <button class="btn btn-outline-success">Принять</button>
                <button class="btn btn-outline-danger">Отклонить</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function formatDateTime(dateTimeStr) {
    const dateTime = new Date(dateTimeStr);
    const formattedDateTime = dateTime.toLocaleString('ru-RU');
    return formattedDateTime;
}

async function searchApplications() {
    const inputName = document.getElementById('inputName').value;
    const url = `https://localhost:7266/api/application/applicationsList?partOfName=${inputName}`;
    const options = {
        method: 'GET',
        headers: {
            'accept': 'application/json',
            //'status': 1, // Установка параметра status в заголовке запроса
            'page': 2, // Установка параметра page в заголовке запроса
            'size': 8 // Установка параметра size в заголовке запроса
        }
    };

    try {
        const response = await fetch(url, options);
        const data = await response.json();
        console.log(data);
        createTable(data.list);
    } catch (error) {
        console.error('Ошибка при поиске заявок:', error);
    }
}
