function initialize() {
    var mapOptions = {
        zoom: 14,
        center: new google.maps.LatLng(51.8127673, 4.659293599999955),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var map = new google.maps.Map(document.getElementById("location-canvas"),
    mapOptions);

    var contentstring = "Festivalbureau Big Rivers<br/>Slikveld 1<br/>3311 VT Dordrecht";

    var infowindow = new google.maps.InfoWindow({
        content: contentstring,
        maxWidth: 200
    });

    var marker = new google.maps.Marker({
        map: map,
        draggable: false,
        animation: google.maps.Animation.DROP,
        position: new google.maps.LatLng(51.8127673, 4.659293599999955),
        title: "Bigrivers kantoor"
    });

    google.maps.event.addListener(marker, "click", function () {
        infowindow.open(map, marker);
    });
}

google.maps.event.addDomListener(window, "resize", initialize);
google.maps.event.addDomListener(window, "load", initialize);