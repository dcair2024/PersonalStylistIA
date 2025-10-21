document.addEventListener("DOMContentLoaded", () => {
    const burger = document.querySelector(".burger");
    const nav = document.querySelector(".nav-links");

    if (!burger || !nav) {
        console.warn("Menu responsivo: .burger ou .nav-links não encontrados.");
        return;
    }

    // Toggle visual
    burger.addEventListener("click", (e) => {
        e.stopPropagation();
        nav.classList.toggle("show");
    });

    // Fecha o menu ao clicar fora
    document.addEventListener("click", (e) => {
        if (!nav.contains(e.target) && !burger.contains(e.target)) {
            nav.classList.remove("show");
        }
    });

    // Fecha no Escape
    document.addEventListener("keyup", (e) => {
        if (e.key === "Escape") nav.classList.remove("show");
    });
});
