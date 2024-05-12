const hotelSelect = document.getElementById('hotel-select');
const checkAvailabilityButton = document.getElementById('check-availability-link');
const dateStartInput = document.getElementById('date-start');
const dateEndInput = document.getElementById('date-end');

document.addEventListener('DOMContentLoaded', function () {
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

checkAvailabilityButton.addEventListener('click', async (event) => {
    event.preventDefault();

    const validationErrors = [];
    if (!hotelSelect.value) {
        validationErrors.push('Selecione um hotel da lista.');
    }

    if (!checkInDateIsValid(dateStartInput.value)) {
      validationErrors.push('Data de Check-in inválida. Utilize o formato AAAA-MM-DD.');
    }
  
    if (!checkOutDateIsValid(dateStartInput.value, dateEndInput.value)) {
      validationErrors.push('Data de Check-out inválida. A data deve ser posterior ao Check-in.');
    }

    if (validationErrors.length > 0) {
        console.error('Erros de validação:', validationErrors);
        return; 
    }

    const selectedHotelId = hotelSelect.value;
    const checkIn = dateStartInput.value;
    const checkOut = dateEndInput.value;

    const url = new URL('FrontEndService/quartos.html', window.location.origin);
    url.searchParams.append('hotelId', selectedHotelId);
    url.searchParams.append('checkIn', checkIn);
    url.searchParams.append('checkOut', checkOut);

    window.location.href = url.href; 
});

function checkInDateIsValid(dateString) {
    const dateRegex = /^\d{2}\/\d{2}\/\d{4}$/;
    return dateRegex.test(dateString);
}

function checkOutDateIsValid(checkInDateString, checkOutDateString) {
    const checkInDate = new Date(checkInDateString);
    const checkOutDate = new Date(checkOutDateString);
    return checkOutDate > checkInDate;
}