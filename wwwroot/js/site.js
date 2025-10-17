// wwwroot/js/zoom.js
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.image-zoom-container').forEach(container => {
        const img = container.querySelector('img');
        if (!img) return;

        let zoomed = false;
        let lastTap = 0;

        // Desktop
        container.addEventListener('mousemove', e => {
            if (window.innerWidth <= 768) return;
            const rect = container.getBoundingClientRect();
            const x = ((e.clientX - rect.left) / rect.width) * 100;
            const y = ((e.clientY - rect.top) / rect.height) * 100;
            img.style.transformOrigin = `${x}% ${y}%`;
            img.style.transform = "scale(2.5)";
        });

        container.addEventListener('mouseleave', () => {
            img.style.transform = "scale(1)";
        });

        // Mobile - double tap
        container.addEventListener('touchstart', e => {
            const now = Date.now();
            if (now - lastTap < 300) {
                zoomed = !zoomed;
                img.style.transform = zoomed ? "scale(2.2)" : "scale(1)";
                e.preventDefault();
            }
            lastTap = now;
        });

        container.style.touchAction = 'none';
    });
});
