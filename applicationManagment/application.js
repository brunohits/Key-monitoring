document.addEventListener('DOMContentLoaded', function() {
    // Получение данных с сервера при загрузке страницы
    getDataFromServer();

    // Обработчик события для кнопки поиска
    const searchButton = document.getElementById('searchButton');
    searchButton.addEventListener('click', function() {
        searchApplications();
    });

    // Обработчики событий для кнопок "Принять" и "Отклонить"
    const tableBody = document.querySelector('.table tbody');
    tableBody.addEventListener('click', function(event) {
        const target = event.target;
        if (target.classList.contains('btn-outline-success')) {
            // Нажата кнопка "Принять"
            const applicationId = target.closest('tr').getAttribute('data-id');
            changeApplicationStatus(applicationId, 1); // 1 - новый статус "Принята"
        } else if (target.classList.contains('btn-outline-danger')) {
            // Нажата кнопка "Отклонить"
            const applicationId = target.closest('tr').getAttribute('data-id');
            changeApplicationStatus(applicationId, 2); // 2 - новый статус "Отклонена"
        }
    });
});

async function changeApplicationStatus(applicationId, newStatus) {
    const url = 'https://localhost:7266/api/application/ChangeApplicationStatus';
    const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiNDlmOWNiODctMDY0ZC00MWIyLTlkMjYtNzQzN2ExNmNjMDNiIiwibmJmIjoxNzEwMTMyMzg1LCJleHAiOjE3MTAxMzU5ODUsImlzcyI6IktleS1Nb25pdG9yaW5nIiwiYXVkIjoiU3R1ZGVudEFuZFRlYWNoZXIifQ.6-UqmlKFi3-Rk2HcvVm4_smdgp_8p33kX_ezbcALMQo";
    const options = {
        method: 'POST',
        headers: {
            'accept': 'application/json',
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            id: applicationId,
            status: newStatus
        })
    };

    try {
        const response = await fetch(url, options);
        const data = await response.json();
        console.log(data); // Выводим ответ сервера в консоль для отладки
        // Дополнительная логика при успешном изменении статуса (если необходимо)
    } catch (error) {
        console.error('Ошибка при изменении статуса заявки:', error);
    }
}
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
