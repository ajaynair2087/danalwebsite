



function addToCart(item) {
    //if cookie with JSON array of items doesn't exist, create JSON array coookie else take array. 
    //If item doesn't exist in array, add item to array else increment counter of item in array.

    //check if cookie with JSON array of items exits

    var htmlEncodedItem = item;
    var serializedItem = htmlDecode(htmlEncodedItem);
    var deserializedItem = jQuery.parseJSON(serializedItem);

    var itemsList = [];

    if ($.cookie('itemsList')) {
        var serItemsList = $.cookie('itemsList');
        var existingItemsInCartArray = JSON.parse(serItemsList);
        existingItemsInCartArray.push(deserializedItem);
        var serializedItemList = JSON.stringify(existingItemsInCartArray);
        $.cookie('itemsList', serializedItemList);
        window.location.href = "/Account/Cart";

    }
    else {
        //serialize item object
        itemsList.push(deserializedItem)
        var serializedItemList = JSON.stringify(itemsList);

        //create itemsListCookie and add serializedItemList
        $.cookie('itemsList', serializedItemList);

    }
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}




