const hotelRoomsElement = document.getElementById('hotel-rooms');
const urlParams = new URLSearchParams(window.location.search);
const hotelId = urlParams.get('hotelId');
const checkin = urlParams.get('checkIn');
const checkout = urlParams.get('checkOut');
const containers = document.querySelector('#hotel-rooms');

async function fetchAvailableRooms() {
    const apiUrl = `http://localhost:5138/api/Quartos?hotelId=${hotelId}`;

    try {
        const response = await fetch(apiUrl);

        if (response.ok) {
            const availableRooms = await response.json();

            hotelRoomsElement.textContent = '';
            if (availableRooms.length > 0) {
                let usedNumbers = new Set();
                availableRooms.forEach(async room => {
                    let isRoomAvailable = true;

                    const apiReservaUrl = `http://127.0.0.1:5001/reservar/${hotelId}/${room.id}`;
                    const responseReserva = await fetch(apiReservaUrl);
                    try {
                        if (responseReserva.ok) {
                            const checkRooms = await responseReserva.json();

                            const checkinDate = new Date(checkin);
                            const checkOutDate = new Date(checkout);

                            checkRooms.forEach(reserve => {
                                const dataIni = new Date(reserve.dataIni);
                                const dataFin = new Date(reserve.dataFin);
                                dataIni.setHours(dataIni.getHours() + 3);
                                dataFin.setHours(dataFin.getHours() + 3);
                                
                                if ((dataIni.getTime() === checkinDate.getTime()) || (dataFin.getTime() === checkOutDate.getTime() && dataIni.getTime() === checkinDate.getTime())) {
                                    isRoomAvailable = false;
                                    return;
                                }
                            });
                        }
                    } catch (error) {
                    }

                    if (!isRoomAvailable) return;

                    const roomElement = document.createElement('div');
                    roomElement.classList.add('col-md-4');
                    roomElement.classList.add('hotel-content');

                    let randomNumber;
                    do {
                        randomNumber = Math.floor(Math.random() * 7) + 1;
                    } while (usedNumbers.has(randomNumber));

                    usedNumbers.add(randomNumber);

                    const roomHtml = `
						<div class="hotel-grid" id="hotel-grid" style="background-image: url(images/quarto${randomNumber}.jpeg);">
						<a class="book-now text-center" href="#"><i class="ti-calendar" id="reservar"></i> Reserve já</a>
                        <input type="hidden" name="roomId" value="${room.id}">
						</div>
						<div class="desc">
						<h3><a href="#">Quarto Número - ${room.numeroQuarto}</a></h3>
						<p>Com uma quantidade de cama(s) incríveis - ${room.quantidadeCamas}</p>
						</div>
					`;

                    roomElement.innerHTML = roomHtml;

                    hotelRoomsElement.appendChild(roomElement);
                });
            } else {
                hotelRoomsElement.textContent = 'Não há quartos disponíveis para as datas selecionadas.';
            }
        } else {
            console.error('Erro ao buscar quartos disponíveis:', response.statusText);
            hotelRoomsElement.textContent = 'Erro ao buscar quartos disponíveis. Tente novamente mais tarde.';
        }
    } catch (error) {
        console.error('Erro inesperado:', error);
        hotelRoomsElement.textContent = 'Erro inesperado. Tente novamente mais tarde.';
    }
}

fetchAvailableRooms();

const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (mutation.addedNodes.length) {
            const addedNodes = Array.from(mutation.addedNodes);
            addedNodes.forEach(newNode => {
                const reservarButtons = newNode.querySelectorAll('.book-now');

                reservarButtons.forEach(button => {
                    button.addEventListener('click', async (event) => {

                        event.preventDefault();

                        const hotelGridElement = button.parentElement;
                        const roomIdInput = hotelGridElement.querySelector('input[name="roomId"]');
                        const roomId = roomIdInput.value;


                        const url = new URL('FrontEndService/reservaHotel.html', window.location.origin);
                        url.searchParams.append('hotelId', hotelId);
                        url.searchParams.append('roomId', roomId);

                        const confirmReservation = confirm('Confirmar reserva?');
                        if (!confirmReservation) {
                            return;
                        }

                        window.location.href = url.href;
                    });
                });
            });
        }
    });
});

observer.observe(containers, { childList: true });