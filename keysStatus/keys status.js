var token = localStorage.getItem('token');
console.log(token)

const url = `https://localhost:7266/api/key/GetFullKeyList`;
get(url);

async function get(url) {
  return fetch(url, {
    method: 'GET',
    headers: {
      accept: 'application/json',
    },
  })
    .then(response => response.json())
    .then(data => {
      console.log(url);
      console.log(data);
      createElement(data);
    })
    .catch(error => {
      console.error('Ошибка', error);
    });
}

async function createElement(data) {
    const container = document.querySelector('.element'); // Находим контейнер, в котором будем добавлять элементы

    data.list.forEach(item => {
        const newElement = document.createElement('div');
        newElement.classList.add('container', 'mt-2', 'col-12', 'col-lg-6', 'px-0');
        newElement.innerHTML = `
            <div class="p-1 blue rounded shadow-sm py-2 mx-1">
                <div class="d-flex align-items-center">
                    <h6 class="text-white fw-bold px-2">К. ${item.cabinetNumber}</h6>
                    <h6></h6>
                    <div class="d-flex">
                        <h6 class="bg-${item.keyStatus === 1 ? 'warning' : 'success'}-subtle rounded text-${item.keyStatus === 1 ? 'warning' : 'success'} fw-bold px-1">
                            ${item.keyStatus === 1 ? 'На руках у' : 'В Деканате'}
                        </h6>
                        <font class="px-1" color="#DFDFDF">
                            ${item.ownerName ? item.ownerName : ''}
                        </font>
                    </div>
                    <a class="btn ms-auto" data-bs-toggle="collapse" href="#${item.id}" role="button" aria-expanded="false" aria-controls="collapseExample" onclick="schedule('${item.id}', document.getElementById('currentDate-${item.id}').textContent)">
                        &#11167;
                    </a>
                </div>
                <div class="collapse" id="${item.id}">
                    <div class="card">
                        <div>
                            <div style="display: flex; justify-content: space-between; align-items: center;">
                                <div class="btn" onclick="slideDate('left', '${item.id}'); schedule('${item.id}', document.getElementById('currentDate-${item.id}').textContent)">&#10094;</div>
                                <p id="currentDate-${item.id}" class="my-0"></p>
                                <div class="btn" onclick="slideDate('right', '${item.id}'); schedule('${item.id}', document.getElementById('currentDate-${item.id}').textContent)">&#10095;</div>
                            </div>
                        </div>
                        <div id="table-${item.id}">
                        </div>
                    </div>
                </div>
            </div>
        `;
        container.appendChild(newElement); // Добавляем созданный элемент в контейнер
        updateDate(item.id); // Обновляем отображение текущей недели для данного элемента
    });
}



//-----------------------------
let currentDate = new Date(); // Инициализируем текущую дату
updateDate(); // Обновляем отображение текущей недели

function slideDate(direction, id) {
    if (direction === 'left') {
        currentDate.setDate(currentDate.getDate() - 7); // Сдвигаем дату на 7 дней влево
    } else {
        currentDate.setDate(currentDate.getDate() + 7); // Сдвигаем дату на 7 дней вправо
    }
    updateDate(id); // Обновляем отображение текущей недели
}

function updateDate(id) {
    let startOfWeek = new Date(currentDate);
    startOfWeek.setDate(currentDate.getDate() - currentDate.getDay() + (currentDate.getDay() === 0 ? -6 : 1)); // Начало текущей недели

    let endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(startOfWeek.getDate() + 6); // Конец текущей недели

    let options = { year: 'numeric', month: '2-digit', day: '2-digit' };
    let formattedStart = startOfWeek.toLocaleDateString('ru-RU', options); // Форматируем начало недели
    let formattedEnd = endOfWeek.toLocaleDateString('ru-RU', options); // Форматируем конец недели

    document.getElementById(`currentDate-${id}`).textContent = formattedStart + " - " + formattedEnd; // Обновляем отображение текущей недели
}

function schedule(id, startDate) {
    const formattedDate = convertDateFormat(startDate);
    console.log(`Вызвана функция Schedule для элемента с id: ${id} и первой датой: ${formattedDate}`);
    getSchedule(id, formattedDate);
}


function convertDateFormat(startDate) {
    const dates = startDate.split(" - "); // Разделяем строку на две даты
    const firstDate = dates[0]; // Берем первую дату

    const dateParts = firstDate.split("."); // Разделяем дату на части
    const formattedDate = `${dateParts[2]}-${dateParts[1]}-${dateParts[0]}T00:00:00.000Z`; // Форматируем дату в нужный вид
    return formattedDate;
}

//-------------------------------------------------
async function getSchedule(idKey, data){
    return fetch('https://localhost:7266/api/key/GetKeyInfoOnWeek', {
        method: 'GET',
        headers: {
          accept: 'application/json',
          id: idKey,
          Start: data
        },
      })
        .then(response => response.json())
        .then(data => {
          console.log(data);
          createScheduleTable(data,idKey);
        })
        .catch(error => {
          console.error('Ошибка', error);
        });
}

//-------------------------------------------------

function createScheduleTable(scheduleData, idKey) {
    const daysOfWeekRussian = ['Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб']; // Russian days of the week
    const daysOfWeekEnglish = ['mn', 'tu', 'we', 'th', 'fr', 'st']; // English days of the week
    const timeSlots = ['8:45', '10:35', '12:25', '14:45', '16:35', '18:25'];
    const table = document.createElement('table');
    table.classList.add('table', 'table-bordered', 'table-sm', 'my-0');
    const thead = document.createElement('thead');
    const tbody = document.createElement('tbody');

    // Create table header with days of the week in Russian
    const headerRow = document.createElement('tr');
    headerRow.innerHTML = '<th scope="col">Время</th>';
    daysOfWeekRussian.forEach(day => {
        headerRow.innerHTML += `<th scope="col">${day}</th>`;
    });
    thead.appendChild(headerRow);

    // Fill the table with data from scheduleData
    timeSlots.forEach(time => {
        const row = document.createElement('tr');
        row.innerHTML = `<td>${time}</td>`;
        daysOfWeekEnglish.forEach((day, index) => {
            const userData = scheduleData[day].find(item => item.pairStart.includes(time));
            const userName = userData ? userData.userName : '';
            const role = userData ? userData.role : '';
            row.innerHTML += `<td>${role === 'DeanOffice'? `<div class="p-1 rounded py-2 mx-1" style="background-color:rgb(255, 77, 0)">${userName}</div>`: 
            role === 'Teacher'? `<div class="p-1 rounded py-2 mx-1" style="background-color:rgb(255, 239, 91)">${userName}</div>` : 
            role === 'Student'? `<div class="p-1 rounded py-2 mx-1" style="background-color:lightgreen">${userName}</div>` : 
            role === 'NotСonfirmed'? `<div class="p-1 rounded py-2 mx-1" style="background-color:rgb(128, 128, 128)">${userName}</div>` : 
            ``}</td>`;
        });
        tbody.appendChild(row);
    });

    table.appendChild(thead);
    table.appendChild(tbody);

    const currentDayElement = document.getElementById(`table-${idKey}`);
    currentDayElement.innerHTML = '';
    currentDayElement.appendChild(table);
}

// Call the function with your data
createScheduleTable(scheduleData, 'yourIdKey');