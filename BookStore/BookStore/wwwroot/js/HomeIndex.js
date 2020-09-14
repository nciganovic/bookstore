$(document).ready(function () {
    $("#all-categories").change(function () {
        var id = $(this).val();
        console.log(id);
        $("#message").html("");
        
        $.ajax({
            url: "books/category",
            method: "get",
            dataType: "json",
            data: {
                id: id
            },
            success: function (data) {
                DisplayBooksInHTML(data.value);
                console.log(data.value);
            },
            error: function (data) {
                console.log(data);
            }
        });

    });

    $("#search-button").click(function () {
        var search = $("#search-text").val().trim();

        if (search != "") {
            $.ajax({
                url: "books/search",
                method: "get",
                dataType: "json",
                data: {
                    search: search
                },
                success: function (data) {
                    if (data.value.length == 0) {
                        $("#message").html(`<p class='text-center mt-3 mb-3'>'${search}' not found.`);
                    }
                    else {
                        $("#message").html(`<p class='text-center mt-3 mb-3'>Results of search '${search}'`);
                    }
                    DisplayBooksInHTML(data.value);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
        
    });

    function DisplayBooksInHTML(data) {
        var html = "";
        for (d of data) {
            html += `<div class="card col-3 p-0 ml-2 mr-2 mt-5" style="width: 18rem;">
                        <a class="text-decoration-none text-dark" href="Books/${d.encryptedId}">`;
            if (d.photoName != null) {
                html += `<img class="w-100" src="uploads/images/${d.photoName}" alt="${d.title}" />`
            }
            else {
                html += `<img class="w-100" src="~/images/nobookimg.png" alt="${d.title}" />`
            }

            html += `   <div class="card-body">
                            <h5 class="card-title">${d.title}</h5>
                            <p class="card-text">${d.categoryName}</p>
                        </div>
                    </a>
                </div> `;
        }

        $("#book-section").html(html);
    }
});