//раскоммитить псле мёрджа -> var token = localStorage.getItem('token');
    
var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiNDlmOWNiODctMDY0ZC00MWIyLTlkMjYtNzQzN2ExNmNjMDNiIiwibmJmIjoxNzEwMTAyMzIwLCJleHAiOjE3MTAxMDU5MjAsImlzcyI6IktleS1Nb25pdG9yaW5nIiwiYXVkIjoiU3R1ZGVudEFuZFRlYWNoZXIifQ.LG1vjxglfm3nd2dBWVyFFINorjHibD36lY0uSmCcqwQ";
console.log(token) 


//раскоммитить после мёрджа -> getOfficeName('https://localhost:7266/api/account/profile', token);
getKeyList(token);

async function getOfficeName(url, token) {
    return fetch(url, {
      method: 'GET',
      headers: new Headers({
        "Authorization": `Bearer ${token}`
      }),
    })
      .then(response => response.json())
      .then(async data => {
        console.log(data);
        document.getElementById('user').textContent = data.fullName;
        if (data.role !== 'DeanOffice'){
            alert('Ошибка доступа');
            window.location.href = '/autorization/autorization.html';
        }
      })
      .catch(error => {
        console.error('Ошибка', error);
        alert('Ошибка доступа');
        window.location.href = '/autorization/autorization.html';
      });
  }

// Получение списка всех ключей
async function getKeyList(token) {
    const url = 'https://localhost:7266/api/key/GetFullKeyList';
    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: new Headers({
                "Authorization": `Bearer ${token}`
              }),
        });
        const data = await response.json();
        if (data.list) {
            const keys = data.list.map(item => item.cabinetNumber);
            console.log(keys);
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
            headers: new Headers({
                "Authorization": `Bearer ${token}`
              }),
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
    console.log("запуск")
    getKeyList();
    getUserList();
};
