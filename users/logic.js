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
  const url = `https://localhost:7266/api/account/Search/Users?Size=20`;
  get(url)

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
                <td>${item.role === 'Student' ? 'Студент' : item.role === 'Teacher' ? 'Преподаватель' : item.role === 'DeanOffice' ? 'Деканат' : 'Не определено'}`;
        
        if (item.role === 'Student' || item.role === 'Teacher') {
            tableHTML += `
                <a href="#" title="${item.role === 'Student' ? 'Сменить роль на преподавателя' : 'Сменить роль на студента'}">
                    <img src="change_role(1).png" alt="change_role.png" class="img-fluid" width="8%">
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

const inputName = document.getElementById('inputName');
const searchButton = document.getElementById('searchButton');

searchButton.addEventListener('click', () => {
    searchPatients();
});

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