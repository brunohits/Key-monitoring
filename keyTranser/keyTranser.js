//раскоммитить псле мёрджа -> var token = localStorage.getItem('token');
    
var token = localStorage.getItem('token');
console.log(token)


//раскоммитить после мёрджа -> getOfficeName('https://localhost:7266/api/account/profile', token);
getOfficeName('https://localhost:7266/api/account/profile', token);
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
        console.log(data.name);
        if (data.name) {
            const users1 = data.name.map(item => ({ name: item.name, id: item.id }));
            console.log(users1);
            insertUsersIntoDropdown(users1);
            
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
    dropdown.innerHTML = '';
    users.forEach(user => {
        const option = document.createElement('option');
        option.text = user.name;
        console.log(user.name);
        option.value = user.id;
        dropdown.add(option);
    });
}

document.getElementById('transferKeyBtn').addEventListener('click', function() {
    var userSelect = document.getElementById('usersDropdown'); 
    var keySelect = document.getElementById('keysDropdown'); 
    if (userSelect.selectedIndex === -1 || keySelect.selectedIndex === -1) {
        alert('Пожалуйста, выберите пользователя и ключ.');
    } else {
        var userId = userSelect.value; // Получаем id выбранного пользователя
        var keyNumber = keySelect.value; // Получаем номер выбранного ключа
        sendEmailRequest(userId, keyNumber); // Вызываем функцию для отправки POST-запроса на сервер
    }
});

// Функция для отправки POST-запроса на отправку email
async function sendEmailRequest(userId, keyNumber) {
    var url = `https://localhost:7266/api/account/send/email?id=${userId}&numberRoom=${keyNumber}`;

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({}) // Пустое тело запроса, так как параметры передаются в URL
        });

        if (response.ok) {
            alert('Код успешно отправлен на почту пользователя.');
        } else {
            console.log(userId);
            alert('Произошла ошибка при отправке email.');
        }
    } catch (error) {
        console.error('Ошибка', error);
        console.log(userId);
        alert('Произошла ошибка при отправке email.');
    } 
}

document.getElementById('sendCodeBtn').addEventListener('click', function() {
    var codeInput = document.getElementById('codeInput');
    if (codeInput.value.trim() === "") {
        alert('Пожалуйста, введите код из письма.');
    } else {
        var codeNumber = codeInput.value.trim(); // Получаем введённый код
        sendCodeRequest(codeNumber); // Вызываем функцию для отправки POST-запроса на сервер
    }
});

// Функция для отправки POST-запроса на отправку кода
async function sendCodeRequest(codeNumber) {
    var url = `https://localhost:7266/api/account/send/code?number=${codeNumber}`;

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify({}) // Пустое тело запроса, так как параметры передаются в URL
        });

        if (response.ok) {
            alert('Код успешно отправлен на сервер.');
            window.location.reload(true);
        } else {
            alert('Произошла ошибка при отправке кода.');
        }
    } catch (error) {
        console.error('Ошибка', error);
        alert('Произошла ошибка при отправке кода.');
    }
}


// Вызов функций для получения списка ключей и пользователей при загрузке страницы
window.onload = function () {
    console.log("запуск")
    getKeyList();
    getUserList();
};
