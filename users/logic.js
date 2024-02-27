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
  
        //maxPagination = data.pagination.count;
        //updatePagination(maxPagination);
      })
      .catch(error => {
        console.error('Ошибка', error);
      });
  }
  const url = `https://localhost:7266/api/account/Search/Users`;
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
    const searchValue = inputName.value;
    const encodedSearchValue = encodeURIComponent(searchValue);
    const url = `https://localhost:7266/api/account/Search/Users?Name=${encodedSearchValue}`;

    get(url);
});