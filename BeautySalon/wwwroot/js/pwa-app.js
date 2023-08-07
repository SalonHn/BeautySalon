let deferredPrompt;

window.addEventListener('beforeinstallprompt', (e) => {
    // Evita que el evento se propague m�s all� de este punto.
    e.preventDefault();

    // Guarda el evento para mostrar el mensaje m�s tarde.
    deferredPrompt = e;

    // Muestra el mensaje de instalaci�n personalizado.
    // Puedes utilizar un elemento HTML, un modal, o cualquier otro enfoque que desees.
    installPWA();
    const installButton = document.getElementById('install-button');
    installButton.style.display = 'block';

    const installHr = document.getElementById('install-hr');
    installHr.style.display = 'block';
});

// Funci�n para iniciar la instalaci�n cuando el usuario hace clic en el bot�n.
function installPWA() {
    if (deferredPrompt) {
        // Muestra el mensaje del sistema para instalar la PWA.
        deferredPrompt.prompt();

        // Espera a que el usuario responda al mensaje.
        deferredPrompt.userChoice.then((choiceResult) => {
            if (choiceResult.outcome === 'accepted') {
                console.log('Usuario acept� instalar la PWA.');
            } else {
                console.log('Usuario rechaz� instalar la PWA.');
            }

            // Limpia la referencia al evento.
            deferredPrompt = null;
        });
    }
}