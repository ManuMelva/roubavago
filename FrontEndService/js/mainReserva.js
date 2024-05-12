const urlParams = new URLSearchParams(window.location.search);
const hotelId = urlParams.get('hotelId');
const roomId = urlParams.get('roomId');

async function getNamesHotelQuarto() {
    const apiUrl = `http://127.0.0.1:5000/api/hotels/${hotelId}`;
    const apiQuartoUrl = `http://localhost:5138/api/Quartos/${roomId}?hotelId=${hotelId}`;

    try {
        const response = await fetch(apiUrl);
        const reponseQuarto = await fetch(apiQuartoUrl);

        if (response.ok && reponseQuarto.ok) {
            const hotel = await response.json();
            const quarto = await reponseQuarto.json();

            const hotelContainer = document.getElementById('container');

            const hotelNameInput = hotelContainer.querySelector('#hotel-name');
            const roomNumberInput = hotelContainer.querySelector('#room-number');

            hotelNameInput.value = hotel.name;
            roomNumberInput.value = quarto.numeroQuarto;
        }
    } catch (error) {
        console.error('Erro inesperado:', error);
    }
}

getNamesHotelQuarto();

const bookingForm = document.querySelector('form[action="/booking-confirmation"]'); 

bookingForm.addEventListener('submit', async (event) => {
  event.preventDefault();

  const formData = new FormData(bookingForm);

  const data = {
    idhotel: hotelId,
    idquarto: roomId,
    dataIni: formData.get('checkIn'),
    dataFin: formData.get('checkOut'),
  };

  try {
    const response = await fetch('http://127.0.0.1:5001/reservar', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json', 
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error(`API request failed with status ${response.json()}`);
    }

    alert('Sua reserva foi registrada com sucesso!');
    window.location.href = 'index.html';

    console.log('Booking data sent successfully!');

  } catch (error) {
    console.error('Error sending booking data:', error);
  }
});