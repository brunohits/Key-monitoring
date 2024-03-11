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
  