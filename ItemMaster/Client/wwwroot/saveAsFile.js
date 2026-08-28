function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}


window.setZoom = function (zoomValue) {
    console.log("Zooming to: " + zoomValue);
    adjustStickyOnZoom();
    const table = document.querySelector('#tableMain .mud-table-container .mud-table-body');
    var str = zoomValue + '%';
    if (table) {
        table.style.zoom = str;
    }
};

window.updateStickyColumns = function (selector, stickyColumnsCount) {
    const table = document.querySelector(selector);
    if (!table) return;

    const rows = table.querySelectorAll('tr'); // Chọn tất cả các hàng trong bảng
    console.log(rows);
    rows.forEach((row, index) => {
        if (index > 0) {
            const cells = row.querySelectorAll('th, td'); // Chọn tất cả th và td trong hàng
            console.log(cells);

       
            // Duyệt qua các cột cần sticky, tính toán giá trị "left"
            for (let i = 0; i < 9; i++) {
                const cell = cells[i];
                if (cell) {
                    cell.classList.add('sticky-left'); // Thêm class CSS cho cột sticky
                    cell.style.left = `${i * 100}px`; // Điều chỉnh khoảng cách left giữa các cột
                    cell.style.zIndex = i === 0 ? 3 : 2; // Cột đầu tiên sẽ có z-index cao hơn
                }
            }
        }
        
    });
};

 

// Function to check if zoom occurred and adjust sticky/left
function adjustStickyOnZoom() {
    const zoomLevel = window.devicePixelRatio; // Detect zoom level
    const stickyElements = document.querySelectorAll('.sticky-left, [style*="left"]'); // Find all sticky elements

    stickyElements.forEach(element => {
        element.style.position = 'static';  // Remove sticky behavior
        element.style.left = 'auto';        // Reset left property
    });
}

// Detect zoom or resize and adjust sticky columns
/*window.addEventListener('resize', adjustStickyOnZoom);*/

// Initial call when the page loads
window.onload = function () {
    adjustStickyOnZoom();
};


