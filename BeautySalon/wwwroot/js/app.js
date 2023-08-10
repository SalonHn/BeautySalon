
import { initializeApp } from '../firebase-app.js';
import { getMessaging, getToken } from '../firebase-messaging.js';

const firebaseConfig = {
    apiKey: "AIzaSyDkCCL0nBfahJrjtVcJC7OYU0b5K55jK10",
    authDomain: "beautysalon-eb280.firebaseapp.com",
    projectId: "beautysalon-eb280",
    storageBucket: "beautysalon-eb280.appspot.com",
    messagingSenderId: "639231951098",
    appId: "1:639231951098:web:7da8c23b34a792c6d70943",
    measurementId: "G-ZYQC97XMHK"
};

// Inicializa Firebase
const firebaseApp = initializeApp(firebaseConfig);


Notification.requestPermission().then(permission => {
    if (permission === 'granted') {
        console.log('Permiso para mostrar notificaciones concedido.');

        // Obtener el token de la suscripci�n
        const messaging = getMessaging(firebaseApp);
        getToken(messaging).then(token => {
            // Env�a el token de la suscripci�n al Service Worker
            navigator.serviceWorker.controller?.postMessage({ action: 'subscribe', token: token });
            console.log(token);
        }).catch(error => {
            console.error('Error al obtener el token de suscripci�n:', error);
        });
    } else {
        console.warn('Permiso para mostrar notificaciones denegado.');
    }
}).catch(error => {
    console.error('Error al solicitar el permiso para mostrar notificaciones:', error);
});