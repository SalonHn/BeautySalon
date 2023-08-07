let deferredPrompt;

window.addEventListener('beforeinstallprompt', (e) => {
    // Evita que el evento se propague más allá de este punto.
    e.preventDefault();

    // Guarda el evento para mostrar el mensaje más tarde.
    deferredPrompt = e;

    // Muestra el mensaje de instalación personalizado.
    // Puedes utilizar un elemento HTML, un modal, o cualquier otro enfoque que desees.
    installPWA();
    const installButton = document.getElementById('install-button');
    installButton.style.display = 'block';

    const installHr = document.getElementById('install-hr');
    installHr.style.display = 'block';
});

// Función para iniciar la instalación cuando el usuario hace clic en el botón.
function installPWA() {
    if (deferredPrompt) {
        // Muestra el mensaje del sistema para instalar la PWA.
        deferredPrompt.prompt();

        // Espera a que el usuario responda al mensaje.
        deferredPrompt.userChoice.then((choiceResult) => {
            if (choiceResult.outcome === 'accepted') {
                console.log('Usuario aceptó instalar la PWA.');
            } else {
                console.log('Usuario rechazó instalar la PWA.');
            }

            // Limpia la referencia al evento.
            deferredPrompt = null;
        });
    }
}