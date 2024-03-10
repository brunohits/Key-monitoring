var token = localStorage.getItem('token');

const searchButton = document.getElementById('user');

searchButton.addEventListener('click', function() {
    logout();
});
async function logout() {
    try {
        const url = 'https://localhost:7266/api/account/logout';
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            },
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const result = await response.json();
        console.log(result);
        location.reload();

    } catch (error) {
        console.error('Ошибка', error);
    }
    token = null;
    localStorage.removeItem('token');
    window.location.href = '/autorization/autorization.html';
}