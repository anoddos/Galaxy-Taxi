// wwwroot/js/googlemaps.js
let map;
let clickListener;

window.initializeMap = function () {
    const mapOptions = {
        center: { lat: 37.7749, lng: -122.4194 }, // San Francisco coordinates
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("googleMap"), mapOptions);
};

window.loadGoogleMapsScript = function () {
    const apiKey = 'AIzaSyBUv46bXQK1BEw1cadMWQ4nv50MyMo6R6M';
    const script = document.createElement('script');
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&callback=initializeMap`;
    script.defer = true;
    document.head.appendChild(script);
};

window.enableMapClick = function () {
    clickListener = map.addListener('click', function (event) {
        const latitude = event.latLng.lat();
        const longitude = event.latLng.lng();
        DotNet.invokeMethodAsync('YourAssemblyName', 'LocationSelected', latitude, longitude);
    });
};

window.disableMapClick = function () {
    if (clickListener) {
        clickListener.remove();
    }
};
