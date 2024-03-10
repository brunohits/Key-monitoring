// Получение списка всех ключей
async function getKeyList() {
    const url = 'https://localhost:7266/api/key/GetFullKeyList';
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'accept': 'application/json'
            }
        });
        const data = await response.json();
        if (data.list) {
            const keys = data.list.map(item => item.cabinetNumber);
            insertKeysIntoDropdown(keys);
        }
    } catch (error) {
        console.error('Ошибка', error);
    }
}

// Получение списка всех пользователей
async function getUserList() {
    const url = 'https://localhost:7266/api/account/Search/Users';
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'accept': 'application/json'
            }
        });
        const data = await response.json();
        if (data.name) {
            const users = data.name.map(item => item.name);
            insertUsersIntoDropdown(users);
        }
    } catch (error) {
        console.error('Ошибка', error);
    }
}

// Вставка списка ключей в выпадающий список
function insertKeysIntoDropdown(keys) {
    const dropdown = document.getElementById('keysDropdown');
    dropdown.innerHTML = ''; // Очистка выпадающего списка перед добавлением новых элементов
    keys.forEach(key => {
        const option = document.createElement('option');
        option.text = key;
        dropdown.add(option);
    });
}

// Вставка списка пользователей в выпадающий список
function insertUsersIntoDropdown(users) {
    const dropdown = document.getElementById('usersDropdown');
    dropdown.innerHTML = ''; // Очистка выпадающего списка перед добавлением новых элементов
    users.forEach(user => {
        const option = document.createElement('option');
        option.text = user;
        dropdown.add(option);
    });
}

// Вызов функций для получения списка ключей и пользователей при загрузке страницы
window.onload = function () {
    getKeyList();
    getUserList();
};
