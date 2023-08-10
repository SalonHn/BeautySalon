// service-worker.js

self.addEventListener('install', function (event) {
    // Realizar la caché de recursos necesarios

    // Activar el nuevo Service Worker y tomar el control
    self.skipWaiting();
});

self.addEventListener('push', event => {
    console.log(event.data.json());
    if (event.data) {
        const notificationData = event.data.json();
        event.waitUntil(
            self.registration.showNotification(notificationData.notification.title, {
                body: notificationData.notification.body,
                icon: '/img/pwa-icon/192x192.png', // Reemplaza con la ruta de tu icono de notificación
            })
        );
    }
});

// Escucha el evento 'pushreceived' personalizado para recibir notificaciones desde la página
self.addEventListener('pushreceived', event => {
    // Envía la notificación al Service Worker cuando se reciba el evento 'pushreceived'
    console.log("Llego pushreceived");
    self.dispatchEvent(new PushEvent('push', { data: event.detail }));
});