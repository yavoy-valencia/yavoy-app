let mapa = null;
let marcadorOrigen = null;
let marcadorDestino = null;
let dotnetHelper = null;

window.iniciarMapa = (helper) => {
    dotnetHelper = helper;

    // Centrado inicial en Valencia, Carabobo
    mapa = L.map('mapa-container').setView([10.1621, -68.0077], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
    }).addTo(mapa);

    mapa.on('click', (e) => {
        colocarMarcador(e.latlng.lat, e.latlng.lng);
    });
};

function colocarMarcador(lat, lng) {
    // Si no hay origen, este clic define el origen. Si ya hay origen, define destino.
    if (marcadorOrigen === null) {
        marcadorOrigen = L.marker([lat, lng], { draggable: true })
            .addTo(mapa)
            .bindPopup('Origen').openPopup();

        marcadorOrigen.on('dragend', () => actualizarCoordenadas());
        dotnetHelper.invokeMethodAsync('ActualizarOrigen', lat, lng);
    } else if (marcadorDestino === null) {
        marcadorDestino = L.marker([lat, lng], { draggable: true, icon: L.icon({
            iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
            iconSize: [25, 41], iconAnchor: [12, 41]
        })}).addTo(mapa).bindPopup('Destino').openPopup();

        marcadorDestino.on('dragend', () => actualizarCoordenadas());
        dotnetHelper.invokeMethodAsync('ActualizarDestino', lat, lng);
    }
}

function actualizarCoordenadas() {
    if (marcadorOrigen) {
        const pos = marcadorOrigen.getLatLng();
        dotnetHelper.invokeMethodAsync('ActualizarOrigen', pos.lat, pos.lng);
    }
    if (marcadorDestino) {
        const pos = marcadorDestino.getLatLng();
        dotnetHelper.invokeMethodAsync('ActualizarDestino', pos.lat, pos.lng);
    }
}

window.reiniciarMapa = () => {
    if (marcadorOrigen) { mapa.removeLayer(marcadorOrigen); marcadorOrigen = null; }
    if (marcadorDestino) { mapa.removeLayer(marcadorDestino); marcadorDestino = null; }
};

window.obtenerUbicacionActual = () => {
    return new Promise((resolve, reject) => {
        if (!navigator.geolocation) {
            reject('Geolocalización no soportada');
            return;
        }
        navigator.geolocation.getCurrentPosition(
            (posicion) => {
                const lat = posicion.coords.latitude;
                const lng = posicion.coords.longitude;
                colocarMarcador(lat, lng);
                mapa.setView([lat, lng], 15);
                resolve({ lat, lng });
            },
            (error) => reject(error.message)
        );
    });
};

window.marcarPuntoDesdeCoordenadas = (lat, lng) => {
    colocarMarcador(lat, lng);
    mapa.setView([lat, lng], 15);
};