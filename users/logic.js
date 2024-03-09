var token = localStorage.getItem('token');
console.log(token)

getOfficeName('https://localhost:7266/api/account/profile', token);

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
      createTable(data.name);

      maxPagination = data.pagination.count;
      console.log(maxPagination);
      updatePagination(maxPagination);
    })
    .catch(error => {
      console.error('Ошибка', error);
    });
}

//--------------------------------------------------------------

function createTable(data) {
  const tableBody = document.querySelector('.table tbody');
  tableBody.innerHTML = '';
  let tableHTML = '';

  data.forEach(item => {
    tableHTML += `
            <tr>
                <td>${item.name}</td>
                <td>${formatDataTime(item.createDate)}</td>
                <td>${item.email}</td>
                <td>${item.phone}</td>
                <td>${item.role === 'Student' ? 'Студент' : item.role === 'Teacher' ? 'Преподаватель' : item.role === 'DeanOffice' ? 'Деканат' : item.role === 'NotСonfirmed' ? 'Не определено' : 'Неизвестно'}`;

    if (item.role === 'Student') {
      tableHTML += `
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Teacher')" title="Сменить роль на Преподаватель"}">
          <img src="change_role_teacher(1).png" alt="change_role(1).png" class="img-fluid" width="20" style="">
        </a>
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'DeanOffice')" title="Сменить роль на Деканат"}">
          <img src="change_role_office(1).png" alt="change_role.png" class="img-fluid" width="20">
        </a>`;
    }

    if (item.role === 'Teacher') {
      tableHTML += `
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Student')" title="Сменить роль на Студент"}">
          <img src="change_role_student(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'DeanOffice')" title="Сменить роль на Деканат"}">
          <img src="change_role_office(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>`;
    }

    if (item.role === 'DeanOffice') {
      tableHTML += `
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Student')" title="Сменить роль на Студент"}">
          <img src="change_role_student(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Teacher')" title="Сменить роль на Преподаватель"}">
          <img src="change_role_teacher(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>`;
    }
    if (item.role === 'NotСonfirmed') {
      tableHTML += `
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Student')" title="Сменить роль на Студент"}">
          <img src="change_role_student(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'Teacher')" title="Сменить роль на Преподаватель"}">
          <img src="change_role_teacher(1).png" alt="change_role(1).png" class="img-fluid" width="20">
        </a>
        <a href="#" style="text-decoration: none;" onclick="changeRole('${item.id}', 'DeanOffice')" title="Сменить роль на Деканат"}">
          <img src="change_role_office(1).png" alt="change_role(1).png" class="img-fluid" width="20" style="max-width: 100px;">
        </a>`;
    }

    tableHTML += `</td></tr>`;
  });

  tableBody.innerHTML = tableHTML; // Добавляем собранную таблицу в tableBody
}

//------------------------------------------------------------------

function formatDataTime(originalDate) {
  const dateParts = originalDate.split("T")[0].split("-");
  const timeParts = originalDate.split("T")[1].split(":");
  const day = dateParts[2];
  const month = dateParts[1];
  const year = dateParts[0];
  const hours = timeParts[0];
  const minutes = timeParts[1];
  const formattedDate = `${day}.${month}.${year} ${hours}:${minutes}`;
  return formattedDate;
}

//----------------------------------------------------------------------
async function working() {
  const url = `https://localhost:7266/api/account/Search/Users?Size=20`;
  get(url)

  const inputName = document.getElementById('inputName');
  const searchButton = document.getElementById('searchButton');

  searchButton.addEventListener('click', () => {
    searchPatients();
  });
}

//----------------------------------------------------------------------

function searchPatients() {
  const searchValue = inputName.value;
  const encodedSearchValue = encodeURIComponent(searchValue);
  const url = `https://localhost:7266/api/account/Search/Users?Name=${encodedSearchValue}&Page=${page}&Size=20`;

  get(url);
}

//----------------------------------------------------------------------

let page = 1;

function updatePagination(maxPagination) {
  const pagination = document.getElementById('pagination');
  pagination.innerHTML = '';
  const currentPage = parseInt(page); // текущая страница
  const totalPages = maxPagination; // общее количество страниц
  let startPage, endPage;
  if (totalPages <= 5) {
    // Если общее количество страниц меньше или равно 5, то отображаем все страницы
    startPage = 1;
    endPage = totalPages;
  } else {
    // Иначе вычисляем начальную и конечную страницы
    if (currentPage <= 3) {
      startPage = 1;
      endPage = 5;
    } else if (currentPage + 2 >= totalPages) {
      startPage = totalPages - 4;
      endPage = totalPages;
      console.log(totalPages)
    } else {
      startPage = currentPage - 2;
      endPage = currentPage + 2;
    }
  }

  // Элементы для номеров страниц и добавление их в пагинацию
  for (let i = startPage; i <= endPage; i++) {
    const li = document.createElement('li');
    li.classList.add('page-item');
    const a = document.createElement('a');
    a.classList.add('page-link');
    a.href = '#';
    a.innerText = i;
    if (i === currentPage) {
      li.classList.add('active'); // выделяем текущую страницу
    }
    li.appendChild(a);
    pagination.appendChild(li);
  }

  const firstPageLink = document.createElement('a');
  firstPageLink.classList.add('page-link');
  firstPageLink.href = '#';
  firstPageLink.innerHTML = '&laquo;';
  const lastPageLink = document.createElement('a');
  lastPageLink.classList.add('page-link');
  lastPageLink.href = '#';
  lastPageLink.innerHTML = '&raquo;';

  const firstPageItem = document.createElement('li');
  firstPageItem.classList.add('page-item');
  firstPageItem.appendChild(firstPageLink);
  const lastPageItem = document.createElement('li');
  lastPageItem.classList.add('page-item');
  lastPageItem.appendChild(lastPageLink);

  pagination.insertBefore(firstPageItem, pagination.firstChild);
  pagination.appendChild(lastPageItem);
}

pagination.addEventListener('click', (event) => {
  event.preventDefault();
  const link = event.target;
  if (!link.classList.contains('page-link')) {
    return;
  }
  if (link.innerText === '«') {
    page = page - 1; // Получаем предыдущий номер страницы
  } else if (link.innerText === '»') {
    page = parseInt(page) + 1; // Получаем следующий номер страницы
  }
  if (page <= 1) {
    page = 1;
  } else if (page >= maxPagination) {
    page = maxPagination;
  }
  if (link.innerText >= 1 && link.innerText <= maxPagination) {
    page = link.innerText; // Получите номер страницы из текста ссылки
  }

  searchPatients(page);
});


//----------------------------------------------------------------------

function changeRole(id, currentRole) {
  console.log(`Изменение роли для пользователя с id ${id} на роль: ${currentRole}`);
  //  if (currentRole === 'Student')
  //    put(id, 'Teacher', token);
  //    if (currentRole === 'Teacher')
  //    put(id, 'Student', token);
  put(id, currentRole, token);
}

//----------------------------------------------------------------------

async function put(id, role, token) {
  const url = `https://localhost:7266/api/account/Change/Role?roleEnum=${role}&idUser=${id}`;
  console.log(token);
  return fetch(url, {
    method: 'PUT',
    headers: new Headers({
      'Content-Type': 'application/json',
      "Authorization": `Bearer ${token}`
    })
  })
    .then(response => response.json())
    .then(result => {
      console.log(result);
      const errorMessage = document.getElementById('errorMessage');
      errorMessage.textContent = '';
      console.log(result);
      window.location.reload(true);
    })
    .catch(error => {
      console.error('Ошибка', error);
      window.location.reload(true);
    });
}



//--------------------------------------------------------
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
      if (data.role === 'DeanOffice'){
        working();
      }
      else{
        alert('Вы не являетесь деканом');
      }
    })
    .catch(error => {
      console.error('Ошибка', error);
    });
}
