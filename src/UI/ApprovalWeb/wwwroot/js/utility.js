//alert("Utility script loaded successfully!");
function fetchData(url, query, callback) {
    $.ajax({
    url: url,
    type: "GET",
    data: { query: query },
    success: function (data) {
        callback(data);
    },
    error: function () {
        console.error("Error fetching data.");
    }
    });
}