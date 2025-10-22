/// burgue.js — Controle do menu responsivo
document.addEventListener("DOMContentLoaded", () => {
    const burger = document.querySelector(".burger");
    const nav = document.querySelector(".nav-links");

    if (!burger || !nav) {
        console.warn("Menu responsivo: elementos não encontrados.");
        return;
    }

    // Abre/fecha menu
    burger.addEventListener("click", (e) => {
        e.stopPropagation();
        nav.classList.toggle("show");
    });

    // Fecha ao clicar fora
    document.addEventListener("click", (e) => {
        if (!nav.contains(e.target) && !burger.contains(e.target)) {
            nav.classList.remove("show");
        }
    });

    // Fecha com Esc
    document.addEventListener("keyup", (e) => {
        if (e.key === "Escape") nav.classList.remove("show");
    });
});
