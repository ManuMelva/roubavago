const hotelRoomsElement = document.getElementById('hotel-rooms');

fetch('http://127.0.0.1:5000/api/hotels/') 
    .then(response => response.json())
    .then(data => {
        let usedNumbers = new Set();
        data.forEach(room => {
            const roomElement = document.createElement('div');
            roomElement.classList.add('col-md-4');
            roomElement.classList.add('hotel-content');
            const availableNumbers = new Set([1, 2, 3, 4, 5, 6, 7]); 

            let randomNumber;
            do {
                randomNumber = Math.floor(Math.random() * 7) + 1;
            } while (usedNumbers.has(randomNumber));

            usedNumbers.add(randomNumber);

            const roomHtml = `
						<div class="hotel-grid" style="background-image: url(images/quarto${randomNumber}.jpeg);">
						<a class="book-now text-center" href="reservaHotel.html"><i class="ti-calendar"></i> Reserve já</a>
						</div>
						<div class="desc">
						<h3><a href="#">${room.name}</a></h3>
						<p>${room.city}</p>
						</div>
					`;

            roomElement.innerHTML = roomHtml;

            hotelRoomsElement.appendChild(roomElement);
        });
    })
    .catch(error => {
        console.error('Error fetching hotel rooms:', error);
    });