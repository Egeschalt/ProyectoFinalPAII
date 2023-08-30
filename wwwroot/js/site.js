// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function abrirPopup() {
    document.getElementById("popup").style.display = "block";
}

// Cierra la vista flotante
function cerrarPopup() {
    document.getElementById("popup").style.display = "none";
}

// Evento para abrir la vista flotante
document.getElementById("mostrarPopup").addEventListener("click", abrirPopup);

// Evento para cerrar la vista flotante
document.getElementById("cerrarPopup").addEventListener("click", cerrarPopup);