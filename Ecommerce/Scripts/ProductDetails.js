$(function () {   
    $("#submit").click(function () {
        var quantity = $("#quantity").val();
        var productId = $("#product-id").val();       
        $.post("/Home/AddToCart", { ProductId: productId, Quantity: quantity }, function (result) {
            $("#cart-quantity").text("(" + result.CartQuantity + ")");
            $("#message").empty();
            $("#message").append("<div class='alert alert-success' role='alert'>" + result.Message + "<a href='/Home/Cart'> Go To Cart</a> or <a href='/Home/Index'> Continue Shopping</a></div>");
        });
    });
});
