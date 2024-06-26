const urlParams = new URLSearchParams(window.location.search);
const hotelId = urlParams.get('hotelId');
const roomId = urlParams.get('roomId');
let hotel;
let quarto;

async function getNamesHotelQuarto() {
    const apiUrl = `http://127.0.0.1:5000/api/hotels/${hotelId}`;
    const apiQuartoUrl = `http://localhost:5138/api/Quartos/${roomId}?hotelId=${hotelId}`;

    try {
        const response = await fetch(apiUrl);
        const reponseQuarto = await fetch(apiQuartoUrl);

        if (response.ok && reponseQuarto.ok) {
            hotel = await response.json();
            quarto = await reponseQuarto.json();

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

    const dataEmail = {
      email: formData.get('email'),
      name: formData.get('name'),
      hotelName: hotel.name,
      checkIn: formData.get('checkIn'),
      checkOut: formData.get('checkOut'),
      numeroQuarto: quarto.numeroQuarto,
      hotelEndereco: hotel.address,
      hotelCidade: hotel.city
    };

    const responseEmail = await fetch('http://127.0.0.1:5111/api/Emails', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json', 
      },
      body: JSON.stringify(dataEmail),
    });

    if (!responseEmail.ok) {
      throw new Error(`API request failed with status ${response.json()}`);
    }

    alert('Sua reserva foi registrada com sucesso!');
    window.location.href = 'index.html';

    console.log('Booking data sent successfully!');

  } catch (error) {
    console.error('Error sending booking data:', error);
  }
});