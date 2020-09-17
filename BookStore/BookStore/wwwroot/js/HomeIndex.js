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
                var html = BooksInHTML(data.value);
                $("#book-section").html(html);
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
                    var html = BooksInHTML(data.value);
                    $("#book-section").html(html);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        }
        
    });

    var skip = 6;
    const take = 3;

    $("#load-more").click(function () {

        $.ajax({
            url: "books/loadmore",
            method: "get",
            dataType: "json",
            data: {
                 take: take,
                 skip: skip,
             },
            success: function (data) {
                if (data.value.length == 0) {
                    $("#load-more").text("You reached the end");
                }
                else {
                    var html = BooksInHTML(data.value);
                    $("#book-section-more").html(html);
                    skip += take;
                }
              
                console.log(data);
            },
            error: function (data) {
                console.log(data);
            }
        });

        console.log(skip);
    });

    function BooksInHTML(data) {
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

        return html;
    }
});