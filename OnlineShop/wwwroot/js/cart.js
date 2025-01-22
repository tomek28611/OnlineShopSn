function updateCart(productId, count) {
    $.ajax({
        url: '/Cart/UpdateCart',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ productId: productId, count: count }),
        success: function (result) {
            $(".item_count").html(result);
            if (count == 0) {
                $("#" + productId).remove();
            }
            getMiniCart();
        },
        error: function () {
            alert('Error product not found or QTY is not enough!');
        }
    });
}

function getMiniCart() {
    $.ajax({
        url: '/Cart/SmallCart',
        type: 'GET',
        success: function (result) {
            $(".mini_cart").html(result);
        },
        error: function () {
            alert('Error fetching mini cart');
        }
    });
}

$(document).ready(function () {
    getMiniCart();
    $(".slick-slide").click(function () {
        $(".slick-track").css("transform", "");
    });
});
