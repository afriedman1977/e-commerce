$(function () {
    var oId = $(".table").data('order-id');

    $(".fulfilled").click(function () {
        var orderDetailsId = $(this).closest('tr').data('order-details-id');
        var button = $(this);
        $.post("/Admin/DetailFulfilled", { odId: orderDetailsId, orderId: oId }, function () {
            button.attr('disabled', true);
        });
    });

    $("#delivery").click(function () {
        $.post("/Admin/UpdateStatus", { orderId: oId, status: "OutForDelivery" }, function () {
            $("#delivery").attr('disabled', true);
        });
    });

    $("#delivered").click(function () {
        $.post("/Admin/UpdateStatus", { orderId: oId, status: "Delivered" }, function () {
            $("#delivered").attr('disabled', true);
        });
    });
});