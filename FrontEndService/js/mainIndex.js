document.addEventListener('DOMContentLoaded', function() {
    const hotelSelect = document.getElementById('hotel-select');

    fetch('http://127.0.0.1:5000/api/hotels/') 
    .then(response => response.json())
    .then(data => {
        data.forEach(hotel => {
            const option = document.createElement('option');
            option.value = hotel.id; 
            option.textContent = hotel.name; 
            hotelSelect.appendChild(option);
        });

        new SelectFx(hotelSelect);
    })
    .catch(error => {
        console.error('Error fetching hotel data:', error);
    });
});